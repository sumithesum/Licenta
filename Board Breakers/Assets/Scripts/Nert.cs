using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;


public class Nert : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
            gameObject.GetComponent<Nert>().enabled = false;
    }
}
