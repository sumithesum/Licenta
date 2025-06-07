using FishNet.Connection;
using FishNet.Object;
using FishNet;
using UnityEngine;
using Unity.VisualScripting;
using System.Collections;

public class CameraManager : NetworkBehaviour
{
    public override void OnStartClient()
    {
        GameObject camera;
        base.OnStartClient();
        if (IsOwner)
        {
            gameObject.AddComponent<GameManager>();

            camera = Cameras.instance.spawnCameraMain();

            camera.SetActive(true);
            camera.tag = "MainCamera";
            RequestPlayerColorServerRpc(PlayerHost.isHost);

        }
        else
        {
            gameObject.GetComponent<CameraManager>().enabled = false;

        }
    }

    [ServerRpc]
    private void RequestPlayerColorServerRpc(bool isHost)
    {

        TargetReceivePlayerColor(base.Owner, isHost);
    }

    [TargetRpc]
    private void TargetReceivePlayerColor(NetworkConnection conn, bool isWhite)
    {
        Debug.Log($"[Client] Am primit culoarea de la server: {isWhite}");

        // Fix: Access the static field 'isWhite' using the class name 'GameManager' instead of an instance reference.  
        GameManager.isWhite = isWhite;
        GameManager gm = gameObject.GetComponent<GameManager>();
    }
}
