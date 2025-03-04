using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils 
{
    public static bool isWhiteTurn = true;
    public static Vector3 startPosition = new Vector3(0.5f, 0.5f , -1f);
    public static Vector3 endPosition = new Vector3(8f, 8f , -1f);

    public  enum  PiecesTypes
    {
        Null,
        Pawn ,
        Knight ,
        Bishop ,
        Rook ,
        Queen ,
        King 
    }


    public static readonly Dictionary<PiecesTypes, int> Points = new Dictionary<PiecesTypes, int>
    {
        { PiecesTypes.Pawn, 1 },
        { PiecesTypes.Knight, 3 },
        { PiecesTypes.Bishop, 3 }, // Aceeasi valoare ca Knight, dar diferit simbol
        { PiecesTypes.Rook, 5 },
        { PiecesTypes.Queen, 9 },
        { PiecesTypes.King, -1 }
    };

}
