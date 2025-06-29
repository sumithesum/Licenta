using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongSender : NetworkBehaviour
{


    public static PingPongSender inst;

    private void Awake()
    {
        inst = this;
    }
    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
    //    inst = this;
    //}

    [ServerRpc(RequireOwnership = false)]
    public void sendStartPositionServer(Vector3 dir)
    {
        print("[SERVER] AM PRIMIT PACHETUL");
        sendStartPositionObserver(dir);

    }

    [ObserversRpc]
    private void sendStartPositionObserver(Vector3 dir)
    {
        print("[CLIENT] AM PRIMIT PACHETUL");
        BilaControll.inst.setVelocity(dir);
    }

    [ServerRpc(RequireOwnership = false)]
    public void sendNewPositionServer(float y)
    {

        sendNewPositionObserver(y);

    }


    [ObserversRpc(ExcludeOwner = true)]
    private void sendNewPositionObserver(float y)
    {
        if (PlayerHost.isHost)
            PlayerControll.inst2.gameObject.transform.position = new Vector3(PlayerControll.inst2.gameObject.transform.position.x,
                                                                               y,
                                                                               PlayerControll.inst2.gameObject.transform.position.z);
        else
            PlayerControll.inst1.gameObject.transform.position = new Vector3(PlayerControll.inst1.gameObject.transform.position.x,
                                                                               y,
                                                                               PlayerControll.inst1.gameObject.transform.position.z);
    }

    [ServerRpc(RequireOwnership = false)]
    public void updateBallPosServer(float x, float y, Vector3 velocity)
    {
        updateBallPosObserver(x, y , velocity);
    }
    [ObserversRpc]
    private void updateBallPosObserver(float x, float y , Vector3 velocity)
    {
        print("NewPoz");
        BilaControll.inst.setPos(x, y);
        BilaControll.inst.setVelocity2(velocity);
    }

}
