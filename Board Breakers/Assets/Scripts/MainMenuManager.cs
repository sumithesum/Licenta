using System;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;

    [SerializeField] private GameObject menuScreem, lobbyscreen , soundScreen , lobbyJoin, chat;
    [SerializeField] private TMP_InputField lobbyInput;

    [SerializeField] private Button startGameButton;

    [SerializeField] private TextMeshProUGUI lobbyTitle, lobbyIdText;


    private void Start()
    {
        OpenMainMenu();
    }


    private void Awake()
    {
        instance = this;
    }

    public void createLobby()
    {

        BootstrapManager.CreateLobby();

    }

    void closeAllScreens()
    {
        menuScreem.SetActive(false);
        lobbyscreen.SetActive(false);
        soundScreen.SetActive(false);
        lobbyJoin.SetActive(false);
        chat.SetActive(false);
        
    }

    public void OpenMainMenu()
    {
        closeAllScreens();
        menuScreem.SetActive(true);
    }

   
    public static void lobbyEntered(string name, bool isHost)
    {
        instance.lobbyTitle.text = name;
        instance.startGameButton.gameObject.SetActive(isHost);
        instance.lobbyIdText.text = BootstrapManager.currentLobbyId.ToString();
        instance.openLobby();
    }


    public void openLobby()
    {
        closeAllScreens();
        lobbyscreen.SetActive(true);
        chat.SetActive(true);
    }
    
    public void openLobbyJoin()
    {
        closeAllScreens();
        lobbyJoin.SetActive(true);
    }

    public void leaveLobby()
    {
        OpenMainMenu();
        BootstrapManager.leaveLobby();
        
    }

    public void joinLobby()
    {
        CSteamID steamID = new CSteamID(Convert.ToUInt64(lobbyInput.text));
        print("Joining");
        BootstrapManager.JoinById(steamID);
    }

    public void exit()
    {
        
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit(); 
        
    }

    public void startGame()
    {
        string[] scenesToClose = new string[] { "MainMenu" };

        BootStrapNetworkManager.changeNetworkScene("MainGame", scenesToClose);
    } 

    public void soundMenu()
    {
        closeAllScreens();
        soundScreen.SetActive(true);
    }

    public void leaveSound()
    {
        closeAllScreens();
        menuScreem.SetActive(true);
    }
    
    public void SaveToClipboard()
    {
        GUIUtility.systemCopyBuffer = lobbyIdText.text;
        
    }

}
