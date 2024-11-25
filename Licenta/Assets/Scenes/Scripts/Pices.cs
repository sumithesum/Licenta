using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static Utils;


public class Pices :MonoBehaviour
{

    public PieceData data = new PieceData();
    private bool isDragging = false;
    private Vector3 initialPos;
    private Vector3 offset;



    private void OnMouseDown()
    {
        isDragging = true;
        initialPos = this.transform.position;
        offset = initialPos - this.GetMouseWorldPosition();
        

    }

    private void OnMouseDrag()
    {
        this.transform.position = this.GetMouseWorldPosition() + offset;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    public void Move()
    {
            ////Actual logic of movement to be implemented;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = -1f; 
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }



    // Start is called before the first frame update
    void Start()
    {
       
    }

   
   

    public GameObject getArtPiece()
    {

        string path = "Art/" + data.type.ToString();

        if (data.isWhite)
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
