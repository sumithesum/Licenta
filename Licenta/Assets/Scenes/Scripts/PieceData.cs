using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class PieceData 
{
    public bool isWhite = true;
    public bool firstMove = true;


    public PiecesTypes type = PiecesTypes.Null;

    public PieceData (bool isWhite_ , PiecesTypes type_)
    {
        type = type_;
        isWhite = isWhite_;
        firstMove = true;
    }
    public PieceData()
    {
        type = PiecesTypes.Null;
        isWhite = true;
        firstMove = true;
    }
}
