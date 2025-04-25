using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using static Utils;

public class OnlineSend : NetworkBehaviour
{
    public static OnlineSend Local;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner)
        {
            
            gameObject.GetComponent<OnlineSend>().enabled = false;
        }
        else
        {
            Local = this;
        }
    }

    public static void Send(Vector3 start, Vector3 end)
    {
        Local.SendMoveFromLocal(start, end);
    }

    public void SendMoveFromLocal(Vector3 start, Vector3 end)
    {
        SendMoveToServer(start, end);
    }

    [ServerRpc]
    private void SendMoveToServer(Vector3 startPos, Vector3 endPos)
    {
        Debug.Log($"[Server] Received move: {startPos} -> {endPos}");


        SendMoveToOtherClient(startPos, endPos, this);
    }

    [ObserversRpc(ExcludeOwner = true)]
    private void SendMoveToOtherClient(Vector3 startPos, Vector3 endPos, OnlineSend script)
    {


        if (Movement(startPos, endPos, true) == 2)
            BootStrapNetworkManager.changeNetworkScene("X0-Online", new string[] { "MainGame"});




        isWhiteTurn = !isWhiteTurn;
        


    }

  
    

}
