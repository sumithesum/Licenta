using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] public bool isP1;
    [SerializeField] public float speed = 0.05f;
    [SerializeField] public static PlayerControll inst1 , inst2;

    void Start()
    {
        if (isP1)
            inst1 = this;
        else
            inst2 = this;
    }

    // Update is called once per frame  
    void Update()
    {
        if (PlayerHost.isHost == isP1)
        {
            
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && gameObject.transform.position.y >= 0.5f )
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - speed, gameObject.transform.position.z);
                PingPongSender.inst.sendNewPositionServer(gameObject.transform.position.y);
            }
            else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && gameObject.transform.position.y <= 7.5f)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + speed, gameObject.transform.position.z);
                PingPongSender.inst.sendNewPositionServer(gameObject.transform.position.y);
            }
        }
    }
    
}
