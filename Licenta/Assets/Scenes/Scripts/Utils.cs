using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{

    public enum PiecesTypes
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
