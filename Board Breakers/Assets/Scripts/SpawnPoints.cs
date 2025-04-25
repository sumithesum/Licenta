using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField]
    public static List<GameObject> SpawnPointss = new List<GameObject>();

    void Start()
    {

        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            SpawnPointss.Add(child.gameObject);
        }

        printListNames();
    }

    public void printListNames()
    {
        string list = "";
        for (int i = 0; i < SpawnPointss.Count; i++)
        {
            list += SpawnPointss[i].name + " : " + i + "\n";
        }
        print(list);
    }


}
