using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class Piece : ScriptableObject
{
    public PieceType type;
    public Sprite aspect;
    public PieceColor color;

}

public enum PieceColor
{
    Black, 
    White
}

public enum PieceType
{
    Pion,
    Tour, 
    Cavalier,
    Fou,
    Dame,
    Roi
}