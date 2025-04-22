using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PlayerEmpty : NetworkBehaviour
{


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner)
        {
            gameObject.GetComponent<PlayerEmpty>().enabled = false;
        }
    }


}
