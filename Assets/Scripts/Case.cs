using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Case : MonoBehaviour, IPointerClickHandler
{
    public Vector2 pos;
    public Piece piece;
    private Image aspect;

    private Image caseColor;
    private Color baseColor;

    public bool isSelected;
    public bool isHighlight;


    public void OnPointerClick(PointerEventData eventData)
    {
        if(isHighlight)
        {
            Case _startCase = ChessManager.Main.GetSelectedCase();
            ChessManager.Main.Move(_startCase, this);
            ChessManager.Main.ClearSelections();
            return;
        }

        ChessManager.Main.ClearSelections();

        if (piece == null)
        {
            return;
        }

        if ((ChessManager.Main.blackTurn && piece.color == PieceColor.White) || (!ChessManager.Main.blackTurn && piece.color == PieceColor.Black))
        {
            return;
        }

        Select(true);
        PreviewPath();
    }

    public void Select(bool _isSelect)
    {
        isSelected = _isSelect;
        if(_isSelect)
        {
            caseColor.color = Color.yellow;

        }
        else
        {
            isHighlight = false;
            caseColor.color = baseColor;
        }
    }

    public void Highlight()
    {
        isHighlight = true;
        if (piece == null)
        {
            caseColor.color = Color.cyan;
        }
        else 
        { 
            caseColor.color = Color.red; 
        }
    }

    public void PreviewPath()
    {
        if (piece == null)
            return;

        List<Vector2> possibility = new List<Vector2>();

        Vector2 _newPos;
        switch (piece.type)
        {
            case PieceType.Pion:
                int _mainDir = piece.color == PieceColor.Black ? 1 : -1;

                _newPos = pos + Vector2.up * _mainDir;
                if (ChessManager.Main.GetCase(_newPos).piece == null)
                {
                    CheckValidity(_newPos, ref possibility);
                }

                if((piece.color == PieceColor.Black && pos.y == 1) || (piece.color == PieceColor.White && pos.y == 6))
                {
                    _newPos = pos + Vector2.up * _mainDir * 2;
                    if (ChessManager.Main.GetCase(_newPos).piece == null)
                    {
                        CheckValidity(_newPos, ref possibility);
                    }
                }

                Vector2 _attack1 = pos + Vector2.up * _mainDir + Vector2.right;
                if(((_attack1.x < 0 || _attack1.x > 7 || _attack1.y < 0 || _attack1.y > 7) == false))
                {
                    if (ChessManager.Main.GetCase(_attack1).piece != null)
                    {
                        CheckValidity(_attack1, ref possibility);
                    }
                }

                Vector2 _attack2 = pos + Vector2.up * _mainDir - Vector2.right;
                if((_attack2.x < 0 || _attack2.x > 7 || _attack2.y < 0 || _attack2.y > 7) == false)
                {
                    if (ChessManager.Main.GetCase(_attack2).piece != null)
                    {
                        CheckValidity(_attack2, ref possibility);
                    }
                }
                break;

            case PieceType.Tour:
                for(int i = (int)pos.x + 1; i < 8; i++)
                {
                    _newPos = new Vector2(i,pos.y);
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }

                for (int i = (int)pos.x - 1; i >= 0; i--)
                {
                    _newPos = new Vector2(i, pos.y);
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }

                for (int i = (int)pos.y + 1; i < 8; i++)
                {
                    _newPos = new Vector2(pos.x, i);
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }

                for (int i = (int)pos.y - 1; i >= 0; i--)
                {
                    _newPos = new Vector2(pos.x, i);
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }
                break;

            case PieceType.Cavalier:
                _newPos = new Vector2(pos.x + 2, pos.y + 1);
                CheckValidity(_newPos, ref possibility);
                _newPos = new Vector2(pos.x + 2, pos.y - 1);
                CheckValidity(_newPos, ref possibility);

                _newPos = new Vector2(pos.x - 2, pos.y + 1);
                CheckValidity(_newPos, ref possibility);
                _newPos = new Vector2(pos.x - 2, pos.y - 1);
                CheckValidity(_newPos, ref possibility);

                _newPos = new Vector2(pos.x + 1, pos.y + 2);
                CheckValidity(_newPos, ref possibility);
                _newPos = new Vector2(pos.x - 1, pos.y + 2);
                CheckValidity(_newPos, ref possibility);

                _newPos = new Vector2(pos.x + 1, pos.y - 2);
                CheckValidity(_newPos, ref possibility);
                _newPos = new Vector2(pos.x - 1, pos.y - 2);
                CheckValidity(_newPos, ref possibility);
                break;
            
            case PieceType.Fou:
                for(Vector2 i = pos + new Vector2(1, 1); i.x < 8 && i.y < 8; i += new Vector2(1, 1))
                {
                    _newPos = i;
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }

                for (Vector2 i = pos + new Vector2(1, -1); i.x <8 && i.y >= 0; i += new Vector2(1, -1))
                {
                    _newPos = i;
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }

                for (Vector2 i = pos + new Vector2(-1, -1); i.x >= 0 && i.y >= 0; i += new Vector2(-1, -1))
                {
                    _newPos = i;
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }

                for (Vector2 i = pos + new Vector2(-1, 1); i.x >= 0 && i.y < 8; i += new Vector2(-1, 1))
                {
                    _newPos = i;
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }
                break;
            
            case PieceType.Dame:
                for (int i = (int)pos.x + 1; i < 8; i++)
                {
                    _newPos = new Vector2(i, pos.y);
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }
                for (int i = (int)pos.x - 1; i >= 0; i--)
                {
                    _newPos = new Vector2(i, pos.y);
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }
                for (int i = (int)pos.y + 1; i < 8; i++)
                {
                    _newPos = new Vector2(pos.x, i);
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }
                for (int i = (int)pos.y - 1; i >= 0; i--)
                {
                    _newPos = new Vector2(pos.x, i);
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }

                for (Vector2 i = pos + new Vector2(1, 1); i.x < 8 && i.y < 8; i += new Vector2(1, 1))
                {
                    _newPos = i;
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }

                for (Vector2 i = pos + new Vector2(1, -1); i.x < 8 && i.y >= 0; i += new Vector2(1, -1))
                {
                    _newPos = i;
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }

                for (Vector2 i = pos + new Vector2(-1, -1); i.x >= 0 && i.y >= 0; i += new Vector2(-1, -1))
                {
                    _newPos = i;
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }

                for (Vector2 i = pos + new Vector2(-1, 1); i.x >= 0 && i.y < 8; i += new Vector2(-1, 1))
                {
                    _newPos = i;
                    if (CheckValidity(_newPos, ref possibility) == false)
                        break;
                }
                break;
            
            case PieceType.Roi:
                _newPos = new Vector2(pos.x + 1, pos.y);
                CheckValidity(_newPos, ref possibility);

                _newPos = new Vector2(pos.x - 1, pos.y);
                CheckValidity(_newPos, ref possibility);

                _newPos = new Vector2(pos.x, pos.y + 1);
                CheckValidity(_newPos, ref possibility);

                _newPos = new Vector2(pos.x, pos.y - 1);
                CheckValidity(_newPos, ref possibility);


                _newPos = new Vector2(pos.x + 1, pos.y + 1);
                CheckValidity(_newPos, ref possibility);

                _newPos = new Vector2(pos.x + 1, pos.y - 1);
                CheckValidity(_newPos, ref possibility);

                _newPos = new Vector2(pos.x - 1, pos.y + 1);
                CheckValidity(_newPos, ref possibility);

                _newPos = new Vector2(pos.x - 1, pos.y - 1);
                CheckValidity(_newPos, ref possibility);
                break;
        }
        ChessManager.Main.Highlight(possibility);
    }

    private bool CheckValidity(Vector2 _pos, ref List<Vector2> _possibility)
    {
        if (_pos.x < 0 || _pos.x > 7 || _pos.y < 0 || _pos.y > 7)
            return false;

        Piece _piece = ChessManager.Main.GetCase(_pos).piece;
        if (_piece == null)
        {
            _possibility.Add(_pos);
            return true;
        }
        else if(_piece.color != piece.color)
        {
            _possibility.Add(_pos);
        }
        return false;
    }

    void Awake()
    {
        caseColor = GetComponent<Image>();
        baseColor = caseColor.color;

        int _id = transform.GetSiblingIndex();
        pos = new Vector2(_id % 8, _id / 8);
        GameObject _aspect = new GameObject("aspect");
        _aspect.transform.parent = this.transform;
        aspect = _aspect.AddComponent<Image>();
        _aspect.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        _aspect.GetComponent<RectTransform>().sizeDelta = Vector2.one * 50;
        _aspect.GetComponent<RectTransform>().localScale = Vector2.one;
        aspect.color = Color.clear;
    }

    public void InitPiece(Piece _piece)
    {
        if (_piece == null)
        {
            piece = null;
            aspect.sprite = null;
            aspect.color = Color.clear;
            return;
        }

        piece = _piece;
        aspect.sprite = piece.aspect;
        aspect.color = piece.color == PieceColor.Black ? Color.black : Color.white;
    }

    void Update()
    {
        
    }
}
