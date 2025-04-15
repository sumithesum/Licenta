using FishNet.Connection;
using FishNet.Object;
using FishNet;
using UnityEngine;

public class CameraManager : NetworkBehaviour
{
    [SerializeField]
    private Camera pCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            pCamera = Camera.main;
            gameObject.AddComponent<GameManager>();

            GameManager.isWhiteStatic = false; 
            RequestPlayerColorServerRpc();
        }
        else
        {
            gameObject.GetComponent<GameManager>().enabled = false;

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
        Debug.Log($"[Client] Am primit culoarea de la server: isWhite={isWhite}");

        GameManager.isWhiteStatic = isWhite;
        gameObject.GetComponent<GameManager>().changeReferencePoint();

        // Activează camera dacă e local player
        if (base.IsOwner)
        {
            Camera.main.gameObject.SetActive(true);
        }
    }
}
