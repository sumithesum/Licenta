using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Linie : MonoBehaviour
{
    public List<GameObject> copii = new List<GameObject>();
    public int id = -1;

    private void Awake()
    {
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            copii.Add(child.gameObject);
        
    }

    public void printh()
    {
        foreach(GameObject child in copii)
            print(child.name);
    }
}
