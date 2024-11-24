using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static Utils;


public class Pices
{
    
    public bool isWhite = true;
    
    
    public Vector3 position ;

   
    public PiecesTypes type = PiecesTypes.Null;

    bool isDrgging = false;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

   
   

    public GameObject getArtPiece()
    {

        string path = "Art/" + type.ToString();

        if (isWhite)
            path += "White";
        else 
            path += "Black";

        return Resources.Load<GameObject>(path);


        //if (type == PiecesTypes.Pawn && isWhite)
        //    return Resources.Load<GameObject>("Art/PawnWhite");
        //else if (type == PiecesTypes.Pawn && !isWhite)
        //    return Resources.Load<GameObject>("Art/PawnBlack");

        //else if (type == PiecesTypes.Rook && isWhite)
        //    return Resources.Load<GameObject>("Art/RookWhite");
        //else if (type == PiecesTypes.Rook && !isWhite)
        //    return Resources.Load<GameObject>("Art/RookBlack");

        //else if (type == PiecesTypes.Knight && isWhite)
        //    return Resources.Load<GameObject>("Art/KnightWhite");
        //else if (type == PiecesTypes.Knight && !isWhite)
        //    return Resources.Load<GameObject>("Art/KnightBlack");

        //else if (type == PiecesTypes.Bishop && isWhite)
        //    return Resources.Load<GameObject>("Art/BishopWhite");
        //else if (type == PiecesTypes.Bishop && !isWhite)
        //    return Resources.Load<GameObject>("Art/PawnBlack");

    }

}
