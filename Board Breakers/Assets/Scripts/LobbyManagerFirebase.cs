using Firebase.Database;
using Firebase.Extensions;
using Steamworks;
using System.Collections;
using UnityEngine;


public class Lobby
{
    public string creatorName;
    public string lobbyId;

    public Lobby(string creatorName_,string lobbyId_)
    {
        creatorName = creatorName_;
        lobbyId = lobbyId_;
    }
}

public class LobbyManagerFirebase : MonoBehaviour
{
    static DatabaseReference db;

    private void Start()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
        PlayerHost.username = SteamFriends.GetPersonaName();
        print(db.ToString());
        //createLobby(PlayerHost.username + "2", "2");
        //getLobys();
    }

    //public void createLobby(string userName, string lobbyId)
    //{
    //    print(userName + " : " + lobbyId);
    //    Lobby lobby = new Lobby(userName , lobbyId);
    //    string json = JsonUtility.ToJson(lobby);
    //    db.Child("lobbies").Push().SetRawJsonValueAsync(json);
    //}

    public void createLobby()
    {
        BootstrapManager.CreateLobby();
        StartCoroutine(waitForNSec(2));
       
    }

    IEnumerator waitForNSec(float secs)
    {
        
        yield return new WaitForSeconds(secs);
        Lobby lobby = new Lobby(PlayerHost.username, PlayerHost.lobbyId);
        string json = JsonUtility.ToJson(lobby);
        var lobbyRef = db.Child("lobbies").Push();
        lobbyRef.SetRawJsonValueAsync(json);
        PlayerHost.isPublic = true;
        PlayerHost.lobbyFirebaseKey = lobbyRef.Key;
        print("Lobby sent");

    }
    public static void LeaveLobby()
    {
        if (!string.IsNullOrEmpty(PlayerHost.lobbyFirebaseKey))
        {
            db.Child("lobbies").Child(PlayerHost.lobbyFirebaseKey).RemoveValueAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                        Debug.Log("Lobby șters din Firebase.");
                    else
                        Debug.LogError("Eroare la ștergerea lobby-ului: " + task.Exception);
                });

            PlayerHost.lobbyFirebaseKey = null;
        }
    }

    public void getLobys()
    {
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

                        Debug.Log($"Lobby by {creatorName} → ID: {lobbyId}");
                    }
                }
            });
    }
}

