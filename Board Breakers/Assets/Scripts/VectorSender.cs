using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class VectorSender : NetworkBehaviour
{

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner)
        {
            gameObject.GetComponent<VectorSender>().enabled = false; 
        }
    }

    void Update()
    {


        if (IsOwner && Input.GetKeyDown(KeyCode.Space))
        {
            print("Sended");
            Vector3 vec1 = new Vector3(Random.Range(-5f, 5f), 0, 0);
            Vector3 vec2 = new Vector3(0, Random.Range(-5f, 5f), 0);
            SendVectorsServer(vec1, vec2);
        }
    }

   
    [ServerRpc]
    private void SendVectorsServer(Vector3 v1, Vector3 v2)
    {
        ReceiveVectorsObservers(v1, v2);
        print("[SV] trimis");
    }

    
    [ObserversRpc(ExcludeOwner = true)]
    private void ReceiveVectorsObservers(Vector3 v1, Vector3 v2)
    {
        Debug.Log($"Am primit vectorii: {v1} și {v2}");

        
    }
}
