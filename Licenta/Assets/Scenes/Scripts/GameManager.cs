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
        bool alb = true;

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

    void InstanceCreator(int i, PiecesTypes type, bool isWhite)
    {
        board[i] = new GameObject();
        board[i].AddComponent<Pices>();
        board[i].GetComponent<Pices>().data.type = type;
        board[i].GetComponent<Pices>().data.isWhite = isWhite;
        float xPosition = startPosition.x + i % 8;
        float yPosition = startPosition.y + (i / 8);
        board[i] = Instantiate(board[i].GetComponent<Pices>().getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        board[i].AddComponent<BoxCollider2D>();
        board[i].AddComponent<Pices>();
        board[i].GetComponent<Pices>().data.type = type;
        board[i].GetComponent<Pices>().data.isWhite = isWhite;

    }

    private bool createStartingPieces()
    {

        for (int i = 16; i < 48; i++)
            board[i] = new GameObject("Null");


        ///White Pawns

        for (int i = 8; i < 16; i++)
        {
            InstanceCreator(i, PiecesTypes.Pawn, true);
        }

        ///Black Pawns

        for (int i = 48; i < 56; i++)
        {
            InstanceCreator(i, PiecesTypes.Pawn, false);
        }

        ///Rooks White


        InstanceCreator(0, PiecesTypes.Rook, true);


        InstanceCreator(7, PiecesTypes.Rook, true);


        ///Rooks Black


        InstanceCreator(56, PiecesTypes.Rook, false);


        InstanceCreator(63, PiecesTypes.Rook, false);

        ///Knight White

        InstanceCreator(1, PiecesTypes.Knight, true);

        InstanceCreator(6, PiecesTypes.Knight, true);

        ///Knight Black

        InstanceCreator(57, PiecesTypes.Knight, false);

        InstanceCreator(62, PiecesTypes.Knight, false);

        ///Bishop White
        InstanceCreator(2, PiecesTypes.Bishop, true);
        InstanceCreator(5, PiecesTypes.Bishop, true);

        ///Bishop Black
        InstanceCreator(58, PiecesTypes.Bishop, false);


        InstanceCreator(61, PiecesTypes.Bishop, false);

        ///King White

        InstanceCreator(3, PiecesTypes.King, true);

        ///King Black

        InstanceCreator(59, PiecesTypes.King, false);

        ///Queen White

        InstanceCreator(4, PiecesTypes.Queen, true);

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

    /// <summary>
    /// Changes the camera and pieces rotation on player turn
    /// </summary>

    public static void changeReferencePoint()
    {
        
        if (isWhiteTurn)
        {
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            for (int i = 0; i < 64; i++)
                board[i].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            for (int i = 0; i < 64; i++)
                board[i].transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    
    }


    /// <summary>
    /// Veryfys if a movement is ok
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>

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



                    ///Capture Enemy
                    ///


                    if ((movement == +9 && board[indexEnd].name != "Null" && !board[indexEnd].GetComponent<Pices>().data.isWhite) ||
                        (movement == +7 && board[indexEnd].name != "Null" && !board[indexEnd].GetComponent<Pices>().data.isWhite))
                    {
                        Destroy(board[indexEnd]);
                        board[indexEnd] = board[indexStart];
                        board[indexStart] = new GameObject("Null");

                        return true;
                    }



                    bool isAPiece = false;



                    if (board[indexStart + 8].name != "Null")
                        isAPiece = true;

                    if (isAPiece)
                    {

                        return false;
                    }

                    if (!board[indexStart].GetComponent<Pices>().data.firstMove && movement == 16)
                    {

                        return false;
                    }

                    if (board[indexStart].GetComponent<Pices>().data.firstMove && movement == 16)
                    {

                        if (board[indexStart + 16].name != "Null")
                            isAPiece = true;

                        if (isAPiece)
                        {

                            return false;
                        }

                        board[indexStart].GetComponent<Pices>().data.firstMove = false;
                        GameObject aux = board[indexStart];
                        board[indexStart] = board[indexEnd];
                        board[indexEnd] = aux;

                        return true;

                    }

                    if (movement == 8)
                    {
                        board[indexStart].GetComponent<Pices>().data.firstMove = false;
                        GameObject aux = board[indexStart];
                        board[indexStart] = board[indexEnd];
                        board[indexEnd] = aux;

                        return true;
                    }



                    return false;

                }

            case "PawnBlack(Clone)":
                {

                    ///#fara ampersant for now
                    ///Is an enemy on the path


                    if ((movement == -9 && board[indexEnd].name != "Null" && board[indexEnd].GetComponent<Pices>().data.isWhite) ||
                       (movement == -7 && board[indexEnd].name != "Null" && board[indexEnd].GetComponent<Pices>().data.isWhite))
                    {
                        Destroy(board[indexEnd]);
                        board[indexEnd] = board[indexStart];
                        board[indexStart] = new GameObject("Null");

                        return true;
                    }



                    bool isAnEnemy = false;



                    if (board[indexStart - 8].name != "Null")
                        isAnEnemy = true;

                    if (isAnEnemy)
                    {

                        return false;
                    }

                    if (!board[indexStart].GetComponent<Pices>().data.firstMove && movement == -16)
                    {

                        return false;
                    }

                    if (board[indexStart].GetComponent<Pices>().data.firstMove && movement == -16)
                    {

                        if (board[indexStart - 16].name != "Null")
                            isAnEnemy = true;

                        if (isAnEnemy)
                        {

                            return false;
                        }
                        board[indexStart].GetComponent<Pices>().data.firstMove = false;
                        GameObject aux = board[indexStart];
                        board[indexStart] = board[indexEnd];
                        board[indexEnd] = aux;

                        return true;

                    }

                    if (movement == -8)
                    {
                        board[indexStart].GetComponent<Pices>().data.firstMove = false;
                        GameObject aux = board[indexStart];
                        board[indexStart] = board[indexEnd];
                        board[indexEnd] = aux;

                        return true;
                    }



                    return false;

                }


            ///Pentru restu de piese nu prea conteaza daca sunt albe sau negre deci le voi face identice
            case "KnightBlack(Clone)":
            case "KnightWhite(Clone)":
                {

                    if (movement == 15 || movement == 6 || movement == 17 || movement == 10 ||
                        movement == -10 || movement == -17 || movement == -6 || movement == -15)
                    {
                        if (board[indexEnd].name == "Null")
                        {
                            GameObject aux = board[indexStart];
                            board[indexStart] = board[indexEnd];
                            board[indexEnd] = aux;

                            return true;
                        }
                        else if (board[indexEnd].GetComponent<Pices>().data.isWhite != board[indexStart].GetComponent<Pices>().data.isWhite)
                        {
                            Destroy(board[indexEnd]);
                            board[indexEnd] = board[indexStart];
                            board[indexStart] = new GameObject("Null");
                            return true;
                        }
                    }

                    return false;
                }


            case "BishopWhite(Clone)":
            case "BishopBlack(Clone)":
                {
                   
                    if(movement % 7 == 0 || movement % 9 == 0)
                    {
                        if (board[indexEnd].name == "Null")
                        {
                            GameObject aux = board[indexEnd];
                            board[indexEnd] = board[indexStart];
                            board[indexStart] = aux;
                            return true;
                        }
                        if (board[indexEnd].GetComponent<Pices>().data.isWhite != board[indexStart].GetComponent<Pices>().data.isWhite)
                        {
                            Destroy(board[indexEnd]);
                            board[indexEnd] = board[indexStart];
                            board[indexStart] = new GameObject("Null");
                            return true;
                        }

                    }
                    return false;
                }


            case "RookWhite(Clone)":
            case "RookBlack(Clone)":
                {
                    ///Quite Hard
                    ///
                    ///Momenta fara logica de Cstleling (cred ca o sa fie facuta la rege)

                    //Verificam daca e pe diagonala.
                    if (
                        ((int)(indexStart / 8) != (int)(indexEnd / 8)) &&
                        (indexStart % 8 != indexEnd % 8)
                       )
                        return false;


                    //Checks if the piece is moving vertycaly
                    if(movement % 8 == 0)
                    {
                        if (movement > 0)
                        {
                            for (int i = 1; i < movement / 8; i++)
                                if (board[indexStart + 8 * i].name != "Null")
                                {
                                    print(indexStart + 8 * i + "    " + board[indexStart + 8 * i].name);
                                    return false;
                                }
                        }
                        else
                            for (int i = 1; i < movement / 8 * (-1); i++)
                                if (board[indexStart - 8 * i].name != "Null")
                                {
                                    print(indexStart - 8 * i + "    " + board[indexStart + 8 * i].name);
                                    return false;
                                }
                        //verific daca e o piesa in drum
                    }


                    if (board[indexEnd].name == "Null")
                    {
                        GameObject aux = board[indexStart];
                        board[indexStart] = board[indexEnd];
                        board[indexEnd] = aux;

                        return true;
                    }
                    //Culori diferite
                    else if (board[indexEnd].GetComponent<Pices>().data.isWhite != board[indexStart].GetComponent<Pices>().data.isWhite)
                    {

                        Destroy(board[indexEnd]);
                        board[indexEnd] = board[indexStart];
                        board[indexStart] = new GameObject("Null");
                        return true;

                    }


                    
                    return false;
                }

            case "QueenWhite(Clone)":
            case "QueenBlack(Clone)":
                {
                    
                    // make the project 3d not 2d
                    return false;
                }
        }



        return false;
    }


   
}
