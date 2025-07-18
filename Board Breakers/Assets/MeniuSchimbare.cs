using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeniuSchimbare : MonoBehaviour
{
    public static MeniuSchimbare inst;

    public GameObject piece;
    public static bool isMade;

    [SerializeField] public GameObject mainBody; 
    void Start()
    {
        mainBody.SetActive(false);
        inst = this;
        isMade = false;
    }

    public void show()
    {
        mainBody.SetActive(true);
    }
    public void close()
    {
        mainBody.SetActive(false);
    }
    public void returnCal()
    {
        mainBody.SetActive(false);
        piece.GetComponent<Pices>().changeType(Utils.PiecesTypes.Knight);
    }
    public void returnNebun()
    {
        mainBody.SetActive(false);
        piece.GetComponent<Pices>().changeType(Utils.PiecesTypes.Bishop);
    }
    public void returnRegina()
    {
        mainBody.SetActive(false);
        piece.GetComponent<Pices>().changeType(Utils.PiecesTypes.Queen);
    }
    public void returnTura()
    {
        mainBody.SetActive(false);
        piece.GetComponent<Pices>().changeType(Utils.PiecesTypes.Rook);
    }
}
