using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEditor;

public class PlayerManager : NetworkBehaviour
{
    
    public float speed = 0.02F;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
            gameObject.GetComponent<PlayerManager>().enabled = false;
        else
        {
            speed = 0.2f;
        }
        

    }

    void Update()
    {
        if (!IsOwner) return; 

        if(Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                gameObject.transform.position.y + speed, gameObject.transform.position.z);
            MoveServerRpc(gameObject.transform.position);
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                gameObject.transform.position.y - speed, gameObject.transform.position.z);
            MoveServerRpc(gameObject.transform.position);
        }
        if(gameObject.transform.position.y > 10)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                -10 + speed, gameObject.transform.position.z);
            MoveServerRpc(gameObject.transform.position);
        }
        else 
            if(gameObject.transform.position.y < -10)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                10 - speed, gameObject.transform.position.z);
            MoveServerRpc(gameObject.transform.position);
        }

        

    }

    public void OnCollisionEnter(Collision collision)
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y,
            gameObject.transform.localScale.z + 0.1f);
        speed += 0.03f;
        print("[Client] sa facut mai mare si rapid");
        
    }


    [ServerRpc]
    private void MoveServerRpc(Vector3 move)
    {
        MoveOnClienst(move);
    }

    [ObserversRpc(ExcludeOwner = true)]
    private void MoveOnClienst(Vector3 move)
    {
        transform.position = move;
    }

}
