using System;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;

    [SerializeField] private GameObject menuScreem, lobbyscreen;
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
    }

    public void leaveLobby()
    {
        OpenMainMenu();
        BootstrapManager.leaveLobby();
        
    }

    public void joinLobby()
    {
        CSteamID steamID = new CSteamID(Convert.ToUInt64(lobbyInput.text));
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


    
}
