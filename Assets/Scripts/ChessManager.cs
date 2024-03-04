using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChessManager : MonoBehaviour
{
    public static ChessManager Main;

    public GameObject board;

    public List<Piece> pieces = new List<Piece>();

    public List<Case> cases = new List<Case>();

    public List<Piece> captureWhite = new List<Piece>();
    public List<Piece> captureBlack = new List<Piece>();
    public List<Image> captureWhite_UI = new List<Image>();
    public List<Image> captureBlack_UI = new List<Image>();

    public bool blackTurn;
    public GameObject markerWhite;
    public GameObject markerBlack;
    public GameObject panelWhiteWin;
    public GameObject panelBlackWin;

    public float timerWhite;
    public TextMeshProUGUI timerWhite_UI;
    public float timerBlack;
    public TextMeshProUGUI timerBlack_UI;

    public GameObject panelChoosePointBlack;
    public GameObject panelChoosePointWhite;


    private void Awake()
    {
        if(Main == null)
        {
            Main = this;
        }
        else if(Main != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        Restart();
        //InitBoard();
        //markerBlack.SetActive(blackTurn);
        //markerWhite.SetActive(!blackTurn);
    }

    private void Update()
    {
        if(blackTurn)
        {
            timerBlack += Time.deltaTime;
            timerBlack_UI.text = (int)(timerBlack / 60) + ":" + (int)(timerBlack % 60);
        }
        else
        {
            timerWhite += Time.deltaTime;
            timerWhite_UI.text = (int)(timerWhite / 60) + ":" + (int)(timerWhite % 60);
        }
    }

    void InitBoard()
    {
        for(int i = 0; i < board.transform.childCount; i++)
        {
            Case _c = board.transform.GetChild(i).GetComponent<Case>();
            
            Piece _piece = GetInitPiece(_c.pos);
            _c.InitPiece(_piece);
            

            cases.Add(_c);
        }
    }

    Piece GetInitPiece(Vector2 _pos)
    {
        if(_pos.y == 1)
        {
            return pieces.Find(_p => _p.type == PieceType.Pion && _p.color == PieceColor.Black);
        }
        else if (_pos.y == 6)
        {
            return pieces.Find(_p => _p.type == PieceType.Pion && _p.color == PieceColor.White);
        }
        else if (_pos.y == 0 && (_pos.x == 0 || _pos.x == 7))
        {
            return pieces.Find(_p => _p.type == PieceType.Tour && _p.color == PieceColor.Black);
        }
        else if (_pos.y == 0 && (_pos.x == 1 || _pos.x == 6))
        {
            return pieces.Find(_p => _p.type == PieceType.Cavalier && _p.color == PieceColor.Black);
        }
        else if (_pos.y == 0 && (_pos.x == 2 || _pos.x == 5))
        {
            return pieces.Find(_p => _p.type == PieceType.Fou && _p.color == PieceColor.Black);
        }
        else if (_pos.y == 0 && _pos.x == 3)
        {
            return pieces.Find(_p => _p.type == PieceType.Dame && _p.color == PieceColor.Black);
        }
        else if (_pos.y == 0 && _pos.x == 4)
        {
            return pieces.Find(_p => _p.type == PieceType.Roi && _p.color == PieceColor.Black);
        }
        else if (_pos.y == 7 && (_pos.x == 0 || _pos.x == 7))
        {
            return pieces.Find(_p => _p.type == PieceType.Tour && _p.color == PieceColor.White);
        }
        else if (_pos.y == 7 && (_pos.x == 1 || _pos.x == 6))
        {
            return pieces.Find(_p => _p.type == PieceType.Cavalier && _p.color == PieceColor.White);
        }
        else if (_pos.y == 7 && (_pos.x == 2 || _pos.x == 5))
        {
            return pieces.Find(_p => _p.type == PieceType.Fou && _p.color == PieceColor.White);
        }
        else if (_pos.y == 7 && _pos.x == 3)
        {
            return pieces.Find(_p => _p.type == PieceType.Dame && _p.color == PieceColor.White);
        }
        else if (_pos.y == 7 && _pos.x == 4)
        {
            return pieces.Find(_p => _p.type == PieceType.Roi && _p.color == PieceColor.White);
        }

        return null;
    }

    public void ClearSelections()
    {
        cases.ForEach(_c => _c.Select(false));
    }

    public Case GetSelectedCase()
    {
        return cases.Find(_c => _c.isSelected);
    }

    public Case GetCase(Vector2 _p)
    {
        return cases.Find(_c => _c.pos == _p);
    }

    public void Highlight(List<Vector2> _possibility)
    {
        _possibility.ForEach(_p => 
        {
            GetCase(_p).Highlight();
        });
    }

    public void Move(Case _start, Case _end)
    {
        if(_end.piece != null)
        {
            if(_end.piece.color == PieceColor.Black)
            {
                captureWhite.Add(_end.piece);
                Image _ui = captureWhite_UI.Find(_p => _p.sprite == null);
                _ui.sprite = _end.piece.aspect;
                _ui.color = Color.black;
            }
            else
            {
                captureBlack.Add(_end.piece);
                Image _ui = captureBlack_UI.Find(_p => _p.sprite == null);
                _ui.sprite = _end.piece.aspect;
                _ui.color = Color.white;
            }

            if(_end.piece.type == PieceType.Roi)
            {
                if(_end.piece.color == PieceColor.Black)
                {
                    panelWhiteWin.SetActive(true);
                }

                if (_end.piece.color == PieceColor.White)
                {
                    panelBlackWin.SetActive(true);
                }
            }
        }

        Piece _p = _start.piece;
        _end.InitPiece(_p);
        _start.InitPiece(null);

        if(_p.type == PieceType.Pion && _p.color == PieceColor.Black && _end.pos.y == 7)
        {
            m_pionCase = _end;
            panelChoosePointBlack.SetActive(true);
        }

        if (_p.type == PieceType.Pion && _p.color == PieceColor.White && _end.pos.y == 0)
        {
            m_pionCase = _end;
            panelChoosePointWhite.SetActive(true);
        }

        blackTurn = !blackTurn;
        markerBlack.SetActive(blackTurn);
        markerWhite.SetActive(!blackTurn);
    }

    private Case m_pionCase;

    public void ChoosePointBlack(int i)
    {
        panelChoosePointBlack.SetActive(false);
        switch(i)
        {
            case 0:
                m_pionCase.InitPiece(pieces.Find(_p => _p.type == PieceType.Cavalier && _p.color == PieceColor.Black));
                break;
            case 1:
                m_pionCase.InitPiece(pieces.Find(_p => _p.type == PieceType.Tour && _p.color == PieceColor.Black));
                break;
            case 2:
                m_pionCase.InitPiece(pieces.Find(_p => _p.type == PieceType.Dame && _p.color == PieceColor.Black));
                break;
            case 3:
                m_pionCase.InitPiece(pieces.Find(_p => _p.type == PieceType.Fou && _p.color == PieceColor.Black));
                break;
        }
    }
    public void ChoosePointWhite(int i)
    {
        panelChoosePointWhite.SetActive(false);
        switch (i)
        {
            case 0:
                m_pionCase.InitPiece(pieces.Find(_p => _p.type == PieceType.Cavalier && _p.color == PieceColor.White));
                break;
            case 1:
                m_pionCase.InitPiece(pieces.Find(_p => _p.type == PieceType.Tour && _p.color == PieceColor.White));
                break;
            case 2:
                m_pionCase.InitPiece(pieces.Find(_p => _p.type == PieceType.Dame && _p.color == PieceColor.White));
                break;
            case 3:
                m_pionCase.InitPiece(pieces.Find(_p => _p.type == PieceType.Fou && _p.color == PieceColor.White));
                break;
        }
    }

    public void Restart()
    {
        panelBlackWin.SetActive(false);
        panelWhiteWin.SetActive(false);
        timerBlack_UI.text = "00:00";
        timerBlack = 0;
        timerWhite_UI.text = "00:00";
        timerWhite = 0;
        captureBlack.Clear();
        captureWhite.Clear();
        captureBlack_UI.ForEach(_p =>
        {
            _p.sprite = null;
            _p.color = Color.clear;
        });
        captureWhite_UI.ForEach(_p =>
        {
            _p.sprite = null;
            _p.color = Color.clear;
        });
        InitBoard();
        blackTurn = false;
        markerBlack.SetActive(blackTurn);
        markerWhite.SetActive(!blackTurn);
        panelChoosePointBlack.SetActive(false);
        panelChoosePointWhite.SetActive(false);
    }
}
