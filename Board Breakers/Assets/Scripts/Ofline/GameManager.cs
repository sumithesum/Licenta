
using Steamworks;
using System;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static Utils;

public class GameManager : MonoBehaviour
{
    public static int? enPassantTargetIndex = null;
    public static bool isWhite = true;

    public static bool isWhiteStatic = true;

    public bool isWhiteInCheck = false;
    public bool isBlackInCheck = false;

    public int blackKingPos = 59;
    public int whiteKingPos = 3;

    private GameObject whitePrefab;
    private GameObject blackPrefab;
    private GameObject plane;
    [SerializeField]
    public static GameObject[] board = new GameObject[64];
    public static GameObject[] tiles = new GameObject[64];

    public static Vector3  LastTakerPos;
    public static Vector3  LastTakenPos;

    public static bool inMinigame = false;

    public static GameManager instance;

    private bool createEmptyMap()
    {
        bool alb = true;

        for (int i = 0; i < 64; i++)
        {
            if (i % 8 == 0)
                alb = alb == true ? false : true;
            if (!alb)
            {
                tiles[i] = Instantiate(blackPrefab, new Vector3(startPosition.x + i % 8, startPosition.y + (i / 8)), Quaternion.identity);
                alb = true;
            }
            else
            {
                tiles[i] = Instantiate(whitePrefab, new Vector3(startPosition.x + i % 8, startPosition.y + (i / 8)), Quaternion.identity);
                alb = false;
            }

        }
        return true;
    }

    public static void changeTurn()
    {
        isWhiteStatic = !isWhiteStatic;
    }

    public static void printBoard()
    {
        
        string final = "";
        for (int i = 7; i > -1; i--)
        {
            string line = "";
            for (int j = 0; j < 8; j++)
                if (board[i * 8 + j].name != "Null")
                    line += board[i * 8 + j].name + "_(" + (i * 8 + j) + ")" + "    ";
                else
                    line += board[i * 8 + j].name + "_(" + (i * 8 + j) + ")" + "                               ";
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
        Material mat = board[i].GetComponent<Pices>().getArtPiece();
        board[i] = Instantiate(plane, new Vector3(xPosition, yPosition, -1),Quaternion.identity );
        board[i].AddComponent<BoxCollider2D>();
        board[i].AddComponent<Pices>();
        board[i].GetComponent<Pices>().data.type = type;
        board[i].GetComponent<Pices>().data.isWhite = isWhite;
        board[i].GetComponent<Renderer>().material = mat;
        board[i].gameObject.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
        board[i].name = type.ToString() + (isWhite ? "White" : "Black") + "(Clone)";
        
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

        InstanceCreator(4, PiecesTypes.King, true);

        ///King Black

        InstanceCreator(60, PiecesTypes.King, false);

        ///Queen White

        InstanceCreator(3, PiecesTypes.Queen, true);

        ///Queen Black

        InstanceCreator(59, PiecesTypes.Queen, false);


        //foreach(GameObject piece in board)
        //{
        //    if (piece.GetComponent<Pices>().data.type != PiecesTypes.Null)
        //    {
        //        piece.AddComponent<BoxCollider2D>();
        //        piece.GetComponent<BoxCollider2D>();
        //        print(piece.GetComponent<Pices>().data.type.ToString() + "  " + piece.transform.position);

        //    }
        //}


        changeReferencePoint();
        return true;
    }

    private void Update()
    {
        if (!inMinigame)
        {
            if (PlayerHost.isHost)
            {

                Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
             
            }
            else
            {
                Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            if (!tiles[0].gameObject.activeSelf)
                open();
        }
    }

    void Start()
    {
        board = new GameObject[64];

        whitePrefab = Resources.Load<GameObject>("Tiles/Tile_White");
        blackPrefab = Resources.Load<GameObject>("Tiles/Tile_Green");
        plane = Resources.Load<GameObject>("Art/Plane");

        instance = this;
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
        changeReferencePoint();

    }

    /// <summary>
    /// Changes the camera and pieces rotation on start
    /// </summary>

    public void changeReferencePoint()
    {
       
        if (PlayerHost.isHost)
        {
         
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            for (int i = 0; i < 64; i++)
                board[i].transform.rotation = Quaternion.Euler(90f, 180f, 0f);
        }
        else
        {
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            for (int i = 0; i < 64; i++)
                board[i].transform.rotation = Quaternion.Euler(270f, 0f, 0f);
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        

    }

    public static void updateGraphics()
    {
        for(int i = 0; i < 64; i++ )
            if (board[i].gameObject.name != "Null")
            {
                int x = i % 8;
                int y = i / 8;
                Vector3 endPos = new Vector3(x + 0.5f, y + 0.5f, -1); 

                board[i].transform.position = endPos;
            }
    }

    public static void close()
    {
        print("closing");
        foreach (var piese in board)
            piese.SetActive(false);
        foreach (var piese in tiles)
            piese.SetActive(false);
    }
    public static void open()
    {
        foreach (var piese in board)
            piese.SetActive(true);
        foreach (var piese in tiles)
            piese.SetActive(true);
    }

    public static void ActualMovement(Vector3 startPos, Vector3 endPos , int movement)
    {
        int indexStart = ((int)startPos.y) * 8 + ((int)startPos.x);
        int indexEnd = ((int)endPos.y) * 8 + ((int)endPos.x);

        SoundManager.instance.PlaySoundFX(0, Camera.main.gameObject.transform);

        if (movement == 2)
        {
            close();

            LastTakerPos = startPos;
            LastTakenPos = endPos;

            inMinigame = true;
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        }
        else if (movement == 1)
        {
            board[indexStart].GetComponent<Pices>().data.firstMove = false;
            GameObject aux = board[indexStart];
            board[indexStart] = board[indexEnd];
            board[indexEnd] = aux;
        }
        else if(movement == 3)
        {
            
            //regele
            board[indexStart].GetComponent<Pices>().data.firstMove = false;
            GameObject aux = board[indexStart];
            board[indexStart] = board[indexEnd];
            board[indexEnd] = aux;

            //turnul
            int rookStart = indexStart + 3;
            int rookEnd = indexStart + 1;

            board[rookStart].GetComponent<Pices>().data.firstMove = false;
            aux = board[rookStart];
            board[rookStart] = board[rookEnd];
            board[rookEnd] = aux;

            float x = board[indexStart].transform.position.x - 1;
            Vector3 pozEnd = new Vector3(x, board[indexStart].transform.position.y, -1);
            board[rookStart].gameObject.transform.position = pozEnd;
        }
        else if (movement == 4)
        {

            //regele
            board[indexStart].GetComponent<Pices>().data.firstMove = false;
            GameObject aux = board[indexStart];
            board[indexStart] = board[indexEnd];
            board[indexEnd] = aux;

            //turnul
            int rookStart = indexStart - 4;
            int rookEnd = indexStart - 1;
            board[rookStart].GetComponent<Pices>().data.firstMove = false;
            aux = board[rookStart];
            board[rookStart] = board[rookEnd];
            board[rookEnd] = aux;

            float x = board[indexStart].transform.position.x + 1;
            Vector3 pozEnd = new Vector3(x, board[indexStart].transform.position.y, -1);
            board[rookStart].gameObject.transform.position = pozEnd;
        }
        else if (movement == 5)
        {
            close();
            

            LastTakerPos = startPos;
            LastTakenPos = board[indexEnd - 8].transform.position;

            inMinigame = true;
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        }
        updateGraphics();


    }


    public static int valoare(string nume)
    {
        switch (nume)
        {
            case "Pawn": return 1;
            case "Knight":
            case "Bishop": return 3;
            case "Rook": return 5;
            case "Queen": return 8;
            case "Knig": return 10;
        }
        return -1;
    }

    public static void destroyBoards()
    {
        for (int i = 0; i < 64; i++)
        {
            Destroy(board[i]);
            Destroy(tiles[i]);
        }
        Destroy(instance.gameObject);
        Destroy(Camera.main.gameObject);
    }

    ///E functioanl cu bugguri ?
    /// WhiteWon = 1 //WhiteWin
    /// WhiteWon = 0 //BlackWin
    /// WhiteWon = 3 Draw

    public static void ReturnFromMinigame(int whiteWon) 
    {
        print("Returning with case: " + whiteWon );
        open();
        inMinigame = false;
        ///1 atacatorul castiga
        ///2 atacatul castiga
        ///3 Draw
        string pieceNameTaker = "";
        string pieceNameTaken = "";

        int caz = 3;

        bool isWhiteAttaker = true;
            

        int indexStart = ((int)LastTakerPos.y) * 8 + ((int)LastTakerPos.x);
        int indexEnd = ((int)LastTakenPos.y) * 8 + ((int)LastTakenPos.x);


        if (!board[indexStart].GetComponent<Pices>().data.isWhite)
        {
            isWhiteAttaker = false;
        }

        if (whiteWon == 3)
        {
            caz = 3;
        }
        else
        {
            if ((whiteWon == 1) && isWhiteAttaker)          ///White Ataca si castiga
                caz = 1;
            else if ((whiteWon == 2) && !isWhiteAttaker)    ///Black Ataca si castiga
                caz = 1;
            else if ((whiteWon == 2) && isWhiteAttaker)     ///White Ataca si nu castiga
                caz = 2;
            else if ((whiteWon == 1) && !isWhiteAttaker)    ///Black Ataca si nu castiga
                caz = 2;
        }
        print("Cazul este : " + caz);
        Match match = Regex.Match(board[indexStart].name, @"^(Pawn|Knight|Bishop|Rook|Queen|King)");
        if (match.Success)
        {
            pieceNameTaker = match.Value;
        }
        match = Regex.Match(board[indexEnd].name, @"^(Pawn|Knight|Bishop|Rook|Queen|King)");
        if (match.Success)
        {
            pieceNameTaken = match.Value;
        }

        if (caz == 3)
        {
            if (valoare(pieceNameTaker) > valoare(pieceNameTaken))
            {
                Destroy(board[indexEnd]);
                board[indexEnd] = board[indexStart];
                board[indexStart] = new GameObject("Null");
            }
            else
            {
                board[indexStart].transform.position = LastTakerPos;
            }
        }
        else if(caz == 1)
        {
            if (pieceNameTaken.StartsWith("King"))
                OnlineSend.Local.closeGameServer();

            Destroy(board[indexEnd]);
            board[indexEnd] = board[indexStart];
            board[indexStart] = new GameObject("Null");
            
        }
        else
        {

            board[indexStart].transform.position = LastTakerPos;
        }

        updateGraphics();
        //Destroy(board[indexEnd]);
        //board[indexEnd] = board[indexStart];
        //board[indexStart] = new GameObject("Null");
    }

  
    public static int Movement(Vector3 startPos, Vector3 endPos,bool recived)
    {
        
        ///Transform the position in board index`s
        int indexStart = ((int)startPos.y) * 8 + ((int)startPos.x);
        int indexEnd = ((int)endPos.y) * 8 + ((int)endPos.x);
        int movement = indexEnd - indexStart;

        if (recived)
        {
            board[indexStart].gameObject.transform.position =
                new Vector3((int)endPos.x + 0.5f, (int)endPos.y + 0.5f, -1);
        }

        ///Ok now the logic for movement of evry piece (and the atack logic) 
        if(!recived)
            if ((board[indexStart].GetComponent<Pices>().data.isWhite != isWhiteStatic))
                return 0;



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
                        

                        return 2;
                    }
                    if ((movement == +7 || movement == +9) && board[indexEnd].name == "Null" && enPassantTargetIndex == indexEnd)
                    {

                        return 5;
                    }


                    bool isAPiece = false;



                    if (board[indexStart + 8].name != "Null")
                        isAPiece = true;

                    if (isAPiece)
                    {

                        return 0;
                    }

                    if (!board[indexStart].GetComponent<Pices>().data.firstMove && movement == 16)
                    {

                        return 0;
                    }

                    if (board[indexStart].GetComponent<Pices>().data.firstMove && movement == 16)
                    {

                        if (board[indexStart + 16].name != "Null")
                            isAPiece = true;

                        if (isAPiece)
                        {

                            return 0;
                        }


                        enPassantTargetIndex = indexStart + 8;
                        return 1;

                    }

                    if (movement == 8)
                    {
                       

                        return 1;
                    }



                    return 0;

                }

            case "PawnBlack(Clone)":
                {

                    ///#fara ampersant for now
                    ///Is an enemy on the path
                   

                    if ((movement == -9 && board[indexEnd].name != "Null" && board[indexEnd].GetComponent<Pices>().data.isWhite) ||
                       (movement == -7 && board[indexEnd].name != "Null" && board[indexEnd].GetComponent<Pices>().data.isWhite))
                    {
                       

                        return 2;
                    }

                    if ((movement == -7 || movement == -9) && board[indexEnd].name == "Null" && enPassantTargetIndex == indexEnd)
                    {

                        Destroy(board[indexEnd + 8]);
                        return 1;
                    }

                    bool isAnEnemy = false;



                    if (board[indexStart - 8].name != "Null")
                        isAnEnemy = true;

                    if (isAnEnemy)
                    {

                        return 0;
                    }

                    if (!board[indexStart].GetComponent<Pices>().data.firstMove && movement == -16)
                    {

                        return 0;
                    }

                    if (board[indexStart].GetComponent<Pices>().data.firstMove && movement == -16)
                    {

                        if (board[indexStart - 16].name != "Null")
                            isAnEnemy = true;

                        if (isAnEnemy)
                        {

                            return 0;
                        }
                        enPassantTargetIndex = indexStart - 8;

                        return 1;

                    }

                    if (movement == -8)
                    {
                        

                        return 1;
                    }



                    return 0;

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
                            

                            return 1;
                        }
                        else if (board[indexEnd].GetComponent<Pices>().data.isWhite != board[indexStart].GetComponent<Pices>().data.isWhite)
                        {
                           
                            return 2;
                        }
                    }

                    return 0;
                }


            case "BishopWhite(Clone)":
            case "BishopBlack(Clone)":
                {
                    int direction = 0;

                    if (movement % 7 == 0)
                    {
                        direction = 7;
                    }
                    else
                    {
                        direction = 9;

                    }
                    
                    for (int i = direction; i < movement * (movement / Math.Abs(movement)); i += direction)
                        {
                
                            if (board[i * (movement / Math.Abs(movement)) + indexStart].name != "Null")
                                return 0;
                        }


                    if (movement % 7 == 0 || movement % 9 == 0)
                    {
                        if (board[indexEnd].name == "Null")
                        {
                            
                            return  1;
                        }
                        if (board[indexEnd].GetComponent<Pices>().data.isWhite != board[indexStart].GetComponent<Pices>().data.isWhite)
                        {
                          
                            return 2;
                        }

                    }
                    return 0;
                }


            case "RookWhite(Clone)":
            case "RookBlack(Clone)":
                {
                    ///Quite Hard
                    ///
                    ///Momenta fara logica de Castleling (cred ca o sa fie facuta la rege)

                    //Verificam daca e pe diagonala.
                    if (
                        ((int)(indexStart / 8) != (int)(indexEnd / 8)) &&
                        (indexStart % 8 != indexEnd % 8)
                       )
                        return 0;


                    //Checks if the piece is moving vertycaly
                    if(movement % 8 == 0)
                    {
                        if (movement > 0)
                        {
                            for (int i = 1; i < movement / 8; i++)
                                if (board[indexStart + 8 * i].name != "Null")
                                {
                                    return 0;
                                }
                        }
                        else
                            for (int i = 1; i < movement / 8 * (-1); i++)
                                if (board[indexStart - 8 * i].name != "Null")
                                {
                                    return 0;
                                }
                        //verific daca e o piesa in drum
                    }


                    if (board[indexEnd].name == "Null")
                    {
                       

                        return 1;
                    }
                    //Culori diferite
                    else if (board[indexEnd].GetComponent<Pices>().data.isWhite != board[indexStart].GetComponent<Pices>().data.isWhite)
                    {

                       
                        return 2;

                    }


                    
                    return 0;
                }

            case "QueenWhite(Clone)":
            case "QueenBlack(Clone)":
                {
                    //Bishop
                    if (movement % 7 == 0 || movement % 9 == 0)
                    {
                        if (board[indexEnd].name == "Null")
                        {
                            
                            return 1;
                        }
                        if (board[indexEnd].GetComponent<Pices>().data.isWhite != board[indexStart].GetComponent<Pices>().data.isWhite)
                        {
                           
                            return 2;
                        }

                    }

                    //Rook

                    if (movement % 8 == 0)
                    {
                        if (movement > 0)
                        {
                            for (int i = 1; i < movement / 8; i++)
                                if (board[indexStart + 8 * i].name != "Null")
                                {
                                    print(indexStart + 8 * i + "    " + board[indexStart + 8 * i].name);
                                    return 0;
                                }
                        }
                        else
                            for (int i = 1; i < movement / 8 * (-1); i++)
                                if (board[indexStart - 8 * i].name != "Null")
                                {
                                    print(indexStart - 8 * i + "    " + board[indexStart + 8 * i].name);
                                    return 0;
                                }
                        //verific daca e o piesa in drum
                    }


                    if (board[indexEnd].name == "Null")
                    {
                        

                        return 1;
                    }
                    //Culori diferite
                    else if (board[indexEnd].GetComponent<Pices>().data.isWhite != board[indexStart].GetComponent<Pices>().data.isWhite)
                    {

                        return 2;

                    }



                    return 0;
                }

            case "KingWhite(Clone)":
            case "KingBlack(Clone)":
                {
                    bool isWhite = board[indexStart].GetComponent<Pices>().data.isWhite;
                    bool firstMove = board[indexStart].GetComponent<Pices>().data.firstMove;

                    // Rocada scurta (king-side castling)
                    if (firstMove && (movement == 2))
                    {
                        int rookIndex = indexStart + 3;
                        if (board[rookIndex].name.Contains("Rook") &&
                            board[rookIndex].GetComponent<Pices>().data.firstMove &&
                            board[indexStart + 1].name == "Null" &&
                            board[indexStart + 2].name == "Null")
                        {
                            
                           
                            return 3;
                        }
                    }

                    
                    if (firstMove && (movement == -2))
                    {
                        int rookIndex = indexStart - 4;
                        if (board[rookIndex].name.Contains("Rook") &&
                            board[rookIndex].GetComponent<Pices>().data.firstMove &&
                            board[indexStart - 1].name == "Null" &&
                            board[indexStart - 2].name == "Null" &&
                            board[indexStart - 3].name == "Null")
                        {
                           
                            return 4;
                        }
                    }
                    if (movement == 1 || movement == -1 || movement == 8 || movement == -8 || movement == 7 || movement == -7 || movement == 9 || movement == -9)
                    {
                        if (board[indexEnd].name == "Null")
                        {
                           
                            return 1;
                        }
                        if (board[indexEnd].GetComponent<Pices>().data.isWhite != board[indexStart].GetComponent<Pices>().data.isWhite)
                        {
                           
                            return 2;
                        }
                    }

                    return 0;
                
                }
          
        }



        return 0;
    }


    

}
