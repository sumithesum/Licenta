using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ListTiles : MonoBehaviour
{
    [SerializeField]
    public static List<GameObject> Tiles = new List<GameObject>();

    void Start()
    {
        
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            Tiles.Add(child.gameObject);
        }

        //printListNames();
    }

    public void printListNames()
    {
        string list = "";
        for(int i = 0; i < Tiles.Count; i ++)
        {
            list += Tiles[i].name + " : " + i +"\n";
        }
        print(list);
    }
}
