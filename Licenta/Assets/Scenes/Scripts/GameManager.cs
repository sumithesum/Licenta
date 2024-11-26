using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Utils;

public class GameManager : MonoBehaviour
{




    private GameObject whitePrefab;
    private GameObject blackPrefab;
    [SerializeField]
    public static GameObject[] board = new GameObject[64];

    private bool createEmptyMap()
    {
        bool alb = false;

        for (int i = 0; i < 64; i++)
        {
            if (i % 8 == 0)
                alb = alb == true ? false : true;
            if (!alb)
            {
                Instantiate(blackPrefab, new Vector3(startPosition.x + i % 8, startPosition.y + (i / 8)), Quaternion.identity);
                alb = true;
            }
            else
            {
                Instantiate(whitePrefab, new Vector3(startPosition.x + i % 8, startPosition.y + (i / 8)), Quaternion.identity);
                alb = false;
            }

        }
        return true;
    }



    public static void printBoard()
    {
        string final = "";
        for (int i = 7; i > -1; i--)
        {
            string line = "";
            for (int j = 0; j < 8; j++)
                if (board[i * 8 + j].name != "Null")
                    line += board[i * 8 + j].name + "(" + (i * 8 + j) + ")" + "    ";
                else
                    line += board[i * 8 + j].name + "(" + (i * 8 + j) + ")" + "                               ";
            final += line + "\n" + "\n";
        }
        print(final);
    }

    void InstanceCreator(int i, PiecesTypes type, bool isWhite = true)
    {
        board[i] = new GameObject();
        board[i].AddComponent<Pices>();
        board[i].GetComponent<Pices>().data.type = type;
        board[i].GetComponent<Pices>().data.isWhite = isWhite;
        float xPosition = startPosition.x + i % 8;
        float yPosition = startPosition.y + (i / 8) - (float)0.45;
        board[i] = Instantiate(board[i].GetComponent<Pices>().getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        board[i].AddComponent<BoxCollider2D>();
        board[i].AddComponent<Pices>();
    }

    private bool createStartingPieces()
    {

        for (int i = 16; i < 48; i++)
            board[i] = new GameObject("Null");


        ///White Pawns

        for (int i = 8; i < 16; i++)
        {
            InstanceCreator(i, PiecesTypes.Pawn);
        }

        ///Black Pawns

        for (int i = 48; i < 56; i++)
        {
            InstanceCreator(i, PiecesTypes.Pawn, false);
        }

        ///Rooks White


        InstanceCreator(0, PiecesTypes.Rook);


        InstanceCreator(7, PiecesTypes.Rook);


        ///Rooks Black


        InstanceCreator(56, PiecesTypes.Rook, false);


        InstanceCreator(63, PiecesTypes.Rook, false);

        ///Knight White

        InstanceCreator(1, PiecesTypes.Knight);

        InstanceCreator(6, PiecesTypes.Knight);

        ///Knight Black

        InstanceCreator(57, PiecesTypes.Knight, false);

        InstanceCreator(62, PiecesTypes.Knight, false);

        ///Bishop White
        InstanceCreator(2, PiecesTypes.Bishop);
        InstanceCreator(5, PiecesTypes.Bishop);

        ///Bishop Black
        InstanceCreator(58, PiecesTypes.Bishop, false);


        InstanceCreator(61, PiecesTypes.Bishop, false);

        ///King White

        InstanceCreator(3, PiecesTypes.King);

        ///King Black

        InstanceCreator(59, PiecesTypes.King, false);

        ///Queen White

        InstanceCreator(4, PiecesTypes.Queen);

        ///Queen Black

        InstanceCreator(60, PiecesTypes.Queen, false);


        //foreach(GameObject piece in board)
        //{
        //    if (piece.GetComponent<Pices>().data.type != PiecesTypes.Null)
        //    {
        //        piece.AddComponent<BoxCollider2D>();
        //        piece.GetComponent<BoxCollider2D>();
        //        print(piece.GetComponent<Pices>().data.type.ToString() + "  " + piece.transform.position);

        //    }
        //}



        return true;
    }


    private void Start()
    {
        board = new GameObject[64];

        whitePrefab = Resources.Load<GameObject>("Tiles/Tile_White");
        blackPrefab = Resources.Load<GameObject>("Tiles/Tile_Green");


        if (whitePrefab == null)
        {
            Debug.LogError("White prefab not found in Resources/Tiles/Tile_White");
        }

        if (blackPrefab == null)
        {
            Debug.LogError("Black prefab not found in Resources/Tiles/Tile_Green");
        }


        if (whitePrefab != null && blackPrefab != null)
            if (createEmptyMap())
                if (createStartingPieces())
                    printBoard();

    }


    public static bool Movement(Vector3 startPos, Vector3 endPos)
    {
        ///Transform the position in board index`s
        int indexStart = ((int)startPos.y) * 8 + ((int)startPos.x);
        int indexEnd = ((int)endPos.y) * 8 + ((int)endPos.x);
        int movement = indexEnd - indexStart;

        ///Ok now the logic for movement of evry piece (and the atack logic) 
       

        switch (board[indexStart].name)
        {
            case "PawnWhite(Clone)":
                {
                  
                    ///#fara ampersant for now
                    ///Is an enemy on the path

                    bool isAnEnemy = false;



                    if (board[indexStart + 8].name != "Null")
                        isAnEnemy = true;

                    if (isAnEnemy)
                    {
                        print("caz1");
                        return false;
                    }

                    if (!board[indexStart].GetComponent<Pices>().data.firstMove && movement == 16)
                    {
                        print("caz2");
                        return false;
                    }

                    if (board[indexStart].GetComponent<Pices>().data.firstMove && movement == 16)
                    {

                        if (board[indexStart + 16].name != "Null")
                            isAnEnemy = true;

                        if (isAnEnemy)
                        {
                            print("caz3");
                            return false;
                        }
                        board[indexStart].GetComponent<Pices>().data.firstMove = false;
                        GameObject aux = board[indexStart];
                        board[indexStart] = board[indexEnd];
                        board[indexEnd] = aux;
                        print("caz4");
                        return true;

                    }

                    if (movement == 8)
                    {
                        board[indexStart].GetComponent<Pices>().data.firstMove = false;
                        GameObject aux = board[indexStart];
                        board[indexStart] = board[indexEnd];
                        board[indexEnd] = aux;
                        print("caz5");
                        return true;
                    }



                    return false;

                }

            case "PawnBlack(Clone)":
                {

                    ///#fara ampersant for now
                    ///Is an enemy on the path

                    bool isAnEnemy = false;



                    if (board[indexStart - 8].name != "Null")
                        isAnEnemy = true;

                    if (isAnEnemy)
                    {
                        print("caz1");
                        return false;
                    }

                    if (!board[indexStart].GetComponent<Pices>().data.firstMove && movement == -16)
                    {
                        print("caz2");
                        return false;
                    }

                    if (board[indexStart].GetComponent<Pices>().data.firstMove && movement == -16)
                    {

                        if (board[indexStart - 16].name != "Null")
                            isAnEnemy = true;

                        if (isAnEnemy)
                        {
                            print("caz3");
                            return false;
                        }
                        board[indexStart].GetComponent<Pices>().data.firstMove = false;
                        GameObject aux = board[indexStart];
                        board[indexStart] = board[indexEnd];
                        board[indexEnd] = aux;
                        print("caz4");
                        return true;

                    }

                    if (movement == -8)
                    {
                        board[indexStart].GetComponent<Pices>().data.firstMove = false;
                        GameObject aux = board[indexStart];
                        board[indexStart] = board[indexEnd];
                        board[indexEnd] = aux;
                        print("caz5");
                        return true;
                    }



                    return false;

                }


        }



        return true;
    }


}
