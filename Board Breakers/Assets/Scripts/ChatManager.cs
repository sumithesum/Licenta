using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;

public class ChatManager : MonoBehaviour
{
    public Transform chatHolder;
    public GameObject msgElement;
    public TMP_InputField playerMessage;
    public GameObject msgInput;

    // Listă care ține evidența mesajelor afișate
    private List<GameObject> displayedMessages = new List<GameObject>();

    private void Start()
    {
        DontDestroyOnLoad(this);   
    }

    private void OnEnable()
    {
        // Client primește mesajele de la server
        InstanceFinder.ClientManager.RegisterBroadcast<Message>(onMessageReceivedWithChannel);
        // Server primește mesajele de la clienți
        InstanceFinder.ServerManager.RegisterBroadcast<Message>(onClientMessageReceivedWithChannel);
    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.UnregisterBroadcast<Message>(onMessageReceivedWithChannel);
        InstanceFinder.ServerManager.UnregisterBroadcast<Message>(onClientMessageReceivedWithChannel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TrySendMessage();
        }
        else if (Input.GetKeyDown(KeyCode.Slash))
        {

            chatHolder.gameObject.SetActive(!chatHolder.gameObject.activeSelf);
            msgInput.gameObject.SetActive(!msgInput.gameObject.activeSelf);
        }
    }

    private void TrySendMessage()
    {
        if (string.IsNullOrWhiteSpace(playerMessage.text))
            return;

        Message msg = new Message()
        {
            username = PlayerHost.username,
            message = playerMessage.text.Trim()
        };

        playerMessage.text = "";

        // Trimite DOAR de pe client. Serverul îl va retransmite.
        if (InstanceFinder.IsClient)
            InstanceFinder.ClientManager.Broadcast(msg);
    }

    public void SendMessageOnChat(string user , string mesasge)
    {
        if (string.IsNullOrWhiteSpace(mesasge))
            return;

        Message msg = new Message()
        {
            username = user,
            message = mesasge
        };

        if (InstanceFinder.IsClient)
            InstanceFinder.ClientManager.Broadcast(msg);
    }

    // Server primește mesaj de la client
    private void onClientMessageReceivedWithChannel(NetworkConnection conn, Message msg, FishNet.Transporting.Channel channel)
    {
        // Server îl transmite tuturor
        InstanceFinder.ServerManager.Broadcast(msg);
    }

    // Client primește mesaj de la server
    private void onMessageReceivedWithChannel(Message msg, FishNet.Transporting.Channel channel)
    {
        DisplayMessage(msg);
    }

    // Afișează un mesaj nou în UI și menține maxim 5
    private void DisplayMessage(Message msg)
    {
        GameObject finalMsg = Instantiate(msgElement, chatHolder);
        finalMsg.GetComponent<TextMeshProUGUI>().text = $"{msg.username}: {msg.message}";
        displayedMessages.Add(finalMsg);

        // Dacă avem mai mult de 5 mesaje, ștergem primul
        if (displayedMessages.Count > 5)
        {
            Destroy(displayedMessages[0]);
            displayedMessages.RemoveAt(0);
        }
    }

    // Structura mesajului transmis
    public struct Message : IBroadcast
    {
        public string username;
        public string message;
    }
}
