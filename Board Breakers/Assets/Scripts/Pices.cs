using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using static Utils;
using static GameManager;
using FishNet.Object;
using static OnlineSend;
using Unity.VisualScripting;



public class Pices : NetworkBehaviour
{
    [SerializeField]
    public PieceData data = new PieceData();

    private Vector3 initialALPos;
    private Vector3 initialPos;
    private Vector3 offset;


    public void ReturnToInitialPos()
    {
        this.gameObject.transform.position = initialPos;
    }

    private void OnMouseDown()
    {
        initialALPos = initialPos;
        initialPos = this.transform.position;
        offset = initialPos - this.GetMouseWorldPosition();
        

    }


    private void OnMouseDrag()
    {
        this.transform.position = this.GetMouseWorldPosition() + offset;
    }

    private void OnMouseUp()
    {
        StartCoroutine( Move(false));
    }

    public IEnumerator Move(bool recived)
    {

  

        ///Check if the new pos is the same as old pos (more or less)
        if (this.transform.position.x < (initialPos.x + 0.5f) && this.transform.position.x > (initialPos.x - 0.5f) &&
            this.transform.position.y < (initialPos.y + 0.5f) && this.transform.position.y > (initialPos.y - 0.5f))
        {
            this.transform.position = initialPos;
        }

        ///Check if is the player turn (100% will need to be changed in the future)
        ///And check if the next pos to be moved is on the board 

        if ((this.name.EndsWith("White(Clone)") && !isWhite) || (this.name.EndsWith("Black(Clone)") && isWhite))
            this.transform.position = initialPos;

        else
        {


            if (((this.name.EndsWith("White(Clone)") && (isWhiteStatic == true)) || (this.name.EndsWith("Black(Clone)") && !(isWhiteStatic == true))) &&
            (this.transform.position.x <= endPosition.x && this.transform.position.x >= (startPosition.x - 0.5f) &&
            this.transform.position.y <= endPosition.y && this.transform.position.y >= (startPosition.y - 0.5f))
            )
            {
                int movement = Movement(initialPos, this.transform.position, recived);

                if (movement == 0)
                {
                    this.transform.position = initialPos;

                }
                else
                {
                    ActualMovement(initialPos, this.transform.position, movement);
                    if (!recived)
                    {
                        OnlineSend.Send(initialPos, this.transform.position);

                    }

                    this.transform.position = new Vector3((int)this.transform.position.x + 0.5f, (int)this.transform.position.y + 0.5f, -1);

                    printBoard();
                    yield return new WaitForSeconds(1.5f);
                }

            }

            else
            {
                this.transform.position = initialPos;
            }
        }
           
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
        initialPos = this.transform.position;
    }

   
   

    public Material getArtPiece()
    {
        string path = "Art/Materials/" + data.type.ToString();

        if (data.isWhite)
            path += "White";
        else 
            path += "Black";


        return Resources.Load<Material>(path);


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



    public bool ckechIfKingIsAlive(bool isWhite)
    {
        if (isWhite)
            ;
        return true;
    }

}
