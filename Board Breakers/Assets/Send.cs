using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class Send : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
            gameObject.GetComponent<Nert>().enabled = false;
    }

    private void Update()
    {
        if (Input.anyKey)
        {

        }
    }


    [ServerRpc]
    public void sendVector3(Vector3 v)
    {

    }

}
