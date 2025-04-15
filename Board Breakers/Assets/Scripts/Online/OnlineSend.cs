using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class OnlineSend : NetworkBehaviour
{
    public static OnlineSend Local;

    public override void OnStartClient()
    {
        if (IsOwner)
            Local = this;
    }

    public void SendMoveFromLocal(Vector3 start, Vector3 end)
    {
        SendMoveToServer(start, end);
    }

    [ServerRpc]
    private void SendMoveToServer(Vector3 startPos, Vector3 endPos)
    {
        Debug.Log($"[Server] Received move: {startPos} -> {endPos}");

        if (NetworkObject != null)
        {
            AddObserversManually(); 

            Debug.Log($"[Server] Observers count: {NetworkObject.Observers.Count}");
            foreach (var observer in NetworkObject.Observers)
            {
                Debug.Log($"[Server] Observer: {observer.ClientId}");
            }
            SendMoveToOtherClient(startPos, endPos, this);
        }
        else
        {
            Debug.LogWarning("[Server] NetworkObject is null!");
        }
    }

    [ObserversRpc(ExcludeOwner = true)]
    private void SendMoveToOtherClient(Vector3 startPos, Vector3 endPos, OnlineSend script)
    {
        ServerManager.Spawn(gameObject);
        Debug.Log($"[Client] Received move from server: {startPos} -> {endPos}");
        Movement(startPos, endPos);
    }

    [ServerRpc]
    private void AddObserversManually()
    {
        print("[Salut] " + NetworkManager.ServerManager.Clients.Count);
        foreach (var connectionPair in NetworkManager.ServerManager.Clients)
        {
            var connection = connectionPair.Value;
            print(connectionPair.Key);
            if (!NetworkObject.Observers.Contains(connection))
            {
                
                AddDefaultObserverConditions(NetworkObject, connection);
                Debug.Log($"[Server] Added observer: {connection.ClientId}");
            }
        }
    }

    private void AddDefaultObserverConditions(NetworkObject networkObject, FishNet.Connection.NetworkConnection connection)
    {
        
        if (networkObject != null && connection != null)
        {
            networkObject.Observers.Add(connection);
        }
    }
}
