using FishNet.Managing;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    private static BootstrapManager instance;

    public void Awake()
    {
        instance = this;
    }

    [SerializeField] private string menuName = "MainMenu";
    [SerializeField] private NetworkManager _netmanager;
    [SerializeField] private FishySteamworks.FishySteamworks _fishySteamworks;

    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEnter;

    public static ulong currentLobbyId;

    private void Start()
    {
        LobbyCreated = Callback<LobbyCreated_t>.Create(onLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(onJoinRequest);
        LobbyEnter = Callback<LobbyEnter_t>.Create(onLobbyEnter);
        
    }

    public void GoToMenu() {
        SceneManager.LoadScene(menuName, LoadSceneMode.Additive);
    }

    public static void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
        //OnLobbycreated si onLobbyEnter vor rula instant dupa asta (desigur ca din cauza ca faci un lobby)
    }

    public void onLobbyCreated(LobbyCreated_t callback)
    {
       

        if (callback.m_eResult != EResult.k_EResultOK)
        {
            print("Lobby creation error");
            return;
        }

        currentLobbyId = callback.m_ulSteamIDLobby;
        SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyId),"HostAddress" , SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyId),"name" , SteamFriends.GetPersonaName().ToString() + "`s lobby");
        _fishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        _fishySteamworks.StartConnection(true);
        print("Lobby created");
    }
    
    public void onJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }


    public void onLobbyEnter(LobbyEnter_t callback)
    {
        currentLobbyId = callback.m_ulSteamIDLobby;

        CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(new CSteamID(currentLobbyId));
        PlayerHost.isHost = (lobbyOwner == SteamUser.GetSteamID());
        PlayerHost.username = SteamFriends.GetPersonaName();
        print(PlayerHost.username + "   este username playerului current ?");

        MainMenuManager.lobbyEntered(SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyId), "name"), _netmanager.IsServerStarted);

        _fishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyId), "HostAddress"));
        _fishySteamworks.StartConnection(false);

    }

    public static void JoinById(CSteamID steamID)
    {
        
        if (SteamMatchmaking.RequestLobbyData(steamID))
            SteamMatchmaking.JoinLobby(steamID);
        else
            print("Failed to join lobby with ID: " + steamID.m_SteamID);
    }

    public static void leaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(currentLobbyId));
        currentLobbyId = 0;

        instance._fishySteamworks.StopConnection(false);
        if(instance._netmanager.IsServerStarted)
            instance._fishySteamworks.StopConnection(true);
    }

}
