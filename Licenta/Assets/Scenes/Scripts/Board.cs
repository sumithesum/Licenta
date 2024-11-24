using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Utils;

public class BoardCreate : MonoBehaviour
{

    [SerializeField]
    public Vector2 startPosition = new Vector2(1.5f, 0.5f);

    private GameObject whitePrefab;
    private GameObject blackPrefab;

     Pices [] board = new Pices[64];

    private bool createEmptyMap()
    {
        bool alb = false;

        for (int i = 0; i < 64; i++) {
            if (i % 8 == 0)
                alb = alb == true ? false : true;
            if (!alb) {
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

    void printBoard()
    {
        string final = "";
        for (int i = 7; i > -1; i--) {
            string line = "";
            for (int j = 0; j < 8; j++)
                line += board[i * 8 + j].type.ToString() + "    ";
             final += line + "\n" + "\n"; 
            }
        print(final);
        }


    private bool createStartingPieces(){

        for (int i = 0; i < 64; i++)
            board[i] = new Pices();

        ///White Pawns

        for (int i = 8; i < 16; i++) {
            board[i].type = PiecesTypes.Pawn;
            board[i].isWhite = true;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float) 0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition,yPosition,-1), Quaternion.identity);
        }

        ///Black Pawns

        for (int i = 48; i < 56; i++){
            board[i].type = PiecesTypes.Pawn;
            board[i].isWhite = false;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        ///Rooks White

        {
            int i = 0;
            board[i].type = PiecesTypes.Rook;
            board[i].isWhite = true;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        {
            int i = 7;
            board[i].type = PiecesTypes.Rook;
            board[i].isWhite = true;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        ///Rooks Black
        
        {
            int i = 56;
            board[i].type = PiecesTypes.Rook;
            board[i].isWhite = false;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        {
            int i = 63;
            board[i].type = PiecesTypes.Rook;
            board[i].isWhite = false;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        ///Knight White
       
        {
            int i = 1;
            board[i].type = PiecesTypes.Knight;
            board[i].isWhite = true;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        {
            int i = 6;
            board[i].type = PiecesTypes.Knight;
            board[i].isWhite = true;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        ///Knight Black
        
        {
            int i = 57;
            board[i].type = PiecesTypes.Knight;
            board[i].isWhite = false;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        {
            int i = 62;
            board[i].type = PiecesTypes.Knight;
            board[i].isWhite = false;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        ///Bishop White

        {
            int i = 2;
            board[i].type = PiecesTypes.Bishop;
            board[i].isWhite = true;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        {
            int i = 5;
            board[i].type = PiecesTypes.Bishop;
            board[i].isWhite = true;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        ///Bishop Black

        {
            int i = 58;
            board[i].type = PiecesTypes.Bishop;
            board[i].isWhite = false;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        {
            int i = 61;
            board[i].type = PiecesTypes.Bishop;
            board[i].isWhite = false;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        ///King White
        
        {
            int i = 3;
            board[i].type = PiecesTypes.King;
            board[i].isWhite = true;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        ///King Black

        {
            int i = 59;
            board[i].type = PiecesTypes.King;
            board[i].isWhite = false;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        ///Queen White

        {
            int i = 4;
            board[i].type = PiecesTypes.Queen;
            board[i].isWhite = true;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }

        ///Queen Black

        {
            int i = 60;
            board[i].type = PiecesTypes.Queen;
            board[i].isWhite = false;
            float xPosition = startPosition.x + i % 8;
            float yPosition = startPosition.y + (i / 8) - (float)0.45;
            Instantiate(board[i].getArtPiece(), new Vector3(xPosition, yPosition, -1), Quaternion.identity);
        }



        return true;
    }


    private void Start()
    {
        
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

    
}
