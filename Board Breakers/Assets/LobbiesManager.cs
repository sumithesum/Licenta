using Firebase.Database;
using Firebase.Extensions;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbiesManager : MonoBehaviour
{
    public Transform contentPanel; 
    public GameObject lobbyButtonPrefab;
    public float refreshTime = 1;

    private void Start()
    {
        StartCoroutine(AutoRefresh());   
    }

    public IEnumerator AutoRefresh()
    {
        while (true)
        {
            RefreshLobbyList();
            yield return new WaitForSeconds(refreshTime);
            print("Sa facut un refresh");
        }
    }
    private void OnEnable()
    {
        RefreshLobbyList();
    }
    private void OnDisable()
    {
        RefreshLobbyList();
    }
    public void RefreshLobbyList()
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject); 

        FirebaseDatabase.DefaultInstance
            .GetReference("lobbies")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Eroare la citirea lobby-urilor: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    foreach (DataSnapshot lobbySnapshot in snapshot.Children)
                    {
                        string creatorName = lobbySnapshot.Child("creatorName").Value?.ToString();
                        string lobbyId = lobbySnapshot.Child("lobbyId").Value?.ToString();

                        GameObject btnObj = Instantiate(lobbyButtonPrefab, contentPanel);
                        btnObj.GetComponentInChildren<TMP_Text>().text = $"Join: {creatorName}";
                        print(creatorName + "  :  " + lobbyId);
                        btnObj.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            JoinLobby(lobbyId);
                        });
                    }
                }
            });
    }

    private void JoinLobby(string lobbyId)
    {
        CSteamID steamID = new CSteamID(Convert.ToUInt64(lobbyId));
        print(lobbyId);
        BootstrapManager.JoinById(steamID); 
    }
}
