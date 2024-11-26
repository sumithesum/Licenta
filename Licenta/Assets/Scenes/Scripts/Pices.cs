using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static Utils;
using static GameManager;


public class Pices :MonoBehaviour
{
    [SerializeField]
    public PieceData data = new PieceData();
    
    [SerializeField]
    private Vector3 initialPos;
    private Vector3 offset;



    private void OnMouseDown()
    {
        
        initialPos = this.transform.position;
        offset = initialPos - this.GetMouseWorldPosition();
        

    }

    private void OnMouseDrag()
    {
        this.transform.position = this.GetMouseWorldPosition() + offset;
    }

    private void OnMouseUp()
    {
        Move();
    }

    public void Move()
    {
        ///Check if the new pos is the same as old pos (more or less)
        if (this.transform.position.x < (initialPos.x + 0.5f) && this.transform.position.x > (initialPos.x - 0.5f) &&
            this.transform.position.y < (initialPos.y + 0.95f) && this.transform.position.y > (initialPos.y - 0.05f))
        {
            this.transform.position = initialPos;
        }

        ///Check if is the player turn (100% will need to be changed in the future)
        ///And check if the next pos to be moved is on the board 
        

        else if (((this.name.EndsWith("White(Clone)") && isWhiteTurn) || (this.name.EndsWith("Black(Clone)") && !isWhiteTurn)) && 
            (this.transform.position.x <= endPosition.x && this.transform.position.x >= (startPosition.x - 0.5f) &&
            this.transform.position.y <= endPosition.y && this.transform.position.y >= (startPosition.y - 0.5f)) && 
            Movement(initialPos,this.transform.position))
        {
            if (isWhiteTurn)
                isWhiteTurn = false;
            else
                isWhiteTurn = true;

            this.transform.position = new Vector3((int)this.transform.position.x + 0.5f, (int)this.transform.position.y + 0.05f, -1); 

            printBoard();
        }
        else
            this.transform.position = initialPos;
           
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
