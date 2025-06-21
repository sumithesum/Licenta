using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ListTiles : MonoBehaviour
{

    public static List<GameObject> Tiles = new List<GameObject>();

    void Awake()
    {
        Tiles.Clear();
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            Tiles.Add(child.gameObject);
        }
        print(Tiles.Count);
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
