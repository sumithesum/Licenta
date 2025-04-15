using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMnager : NetworkBehaviour
{
    private bool gameStarted = false;

    public override void OnStartServer()
    {
        base.OnStartServer();

        
        InstanceFinder.ServerManager.OnRemoteConnectionState += ServerManager_OnRemoteConnectionState;
    }
    public override void OnStopClient()
    {
        base.OnStopClient();

        print("Connectat la server");    
    }

    private void ServerManager_OnRemoteConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Started)
        {
            int connectedPlayers = InstanceFinder.ServerManager.Clients.Count;

            Debug.Log($"[Server] Client {conn.ClientId} connected. Total players: {connectedPlayers}");

            
            if (connectedPlayers == 2 && !gameStarted)
            {
                gameStarted = true;

                
                Rpc_TwoPlayersConnected();
            }
        }
    }

    [ObserversRpc]
    private void Rpc_TwoPlayersConnected()
    {
        Debug.Log("[Client] Sunt 2 jucători conectați!");
    }
}
