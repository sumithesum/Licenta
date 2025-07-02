using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undestroybale : MonoBehaviour
{
    // Start is called before the first frame update
    public static Undestroybale inst;
    void Start()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
