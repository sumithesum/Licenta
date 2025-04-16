using FishNet.Connection;
using FishNet.Object;
using FishNet;
using UnityEngine;
using Unity.VisualScripting;

public class CameraManager : NetworkBehaviour
{
    [SerializeField]
    private Camera pCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
        {
            pCamera = Camera.main;
            gameObject.AddComponent<GameManager>();

            GameManager.isWhiteStatic = false; 
            RequestPlayerColorServerRpc();
        }
        else
        { 
            gameObject.GetComponent<CameraManager>().enabled = false;

        }
    }

    [ServerRpc]
    private void RequestPlayerColorServerRpc()
    {
        int count = InstanceFinder.ServerManager.Clients.Count;
        bool assignedColor = (count == 1); 

        Debug.Log($"[Server] Player count: {count}. Assigning white? {assignedColor}");

        TargetReceivePlayerColor(base.Owner, assignedColor);
    }

    [TargetRpc]
    private void TargetReceivePlayerColor(NetworkConnection conn, bool isWhite)
    {
        Debug.Log($"[Client] Am primit culoarea de la server: {isWhite}");

        GameManager.isWhiteStatic = isWhite;
        gameObject.GetComponent<GameManager>().changeReferencePoint();

    }
}
