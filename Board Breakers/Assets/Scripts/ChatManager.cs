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

    
    private List<GameObject> displayedMessages = new List<GameObject>();
    public static ChatManager instance;

    private void Start()
    {
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            
            Destroy(this.gameObject);
        }
       
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
        if (instance != null)
        {
            InstanceFinder.ClientManager.UnregisterBroadcast<Message>(onMessageReceivedWithChannel);
            InstanceFinder.ServerManager.UnregisterBroadcast<Message>(onClientMessageReceivedWithChannel);
        }
    }

    private void OnDestroy()
    {
        instance = null;
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

  
    private void onClientMessageReceivedWithChannel(NetworkConnection conn, Message msg, FishNet.Transporting.Channel channel)
    {
        
        InstanceFinder.ServerManager.Broadcast(msg);
    }


    private void onMessageReceivedWithChannel(Message msg, FishNet.Transporting.Channel channel)
    {
        DisplayMessage(msg);
    }


    private void DisplayMessage(Message msg)
    {
        GameObject finalMsg = Instantiate(msgElement, chatHolder);
        finalMsg.GetComponent<TextMeshProUGUI>().text = $"{msg.username}: {msg.message}";
        displayedMessages.Add(finalMsg);

    
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
