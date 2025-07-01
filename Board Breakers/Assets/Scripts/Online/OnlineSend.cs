using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using static Utils;

public class OnlineSend : NetworkBehaviour
{
    public static OnlineSend Local;

    public static string lastScene;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner)
        {
            
            gameObject.GetComponent<OnlineSend>().enabled = false;
        }
        else
        {
            Local = this;
        }
    }

    public static void Send(Vector3 start, Vector3 end)
    {
        Local.SendMoveFromLocal(start, end);
    }

    public void SendMoveFromLocal(Vector3 start, Vector3 end)
    {
        SendMoveToServer(start, end);
    }

    [ServerRpc]
    private void SendMoveToServer(Vector3 startPos, Vector3 endPos)
    {
        Debug.Log($"[Server] Received move: {startPos} -> {endPos}");

        changeT();
        SendMoveToOtherClient(startPos, endPos);
    }

    [ObserversRpc(ExcludeOwner = true)]
    private void SendMoveToOtherClient(Vector3 startPos, Vector3 endPos)
    {

       

        print("is White Turn ?  : " +isWhiteStatic);
        int miscare = Movement(startPos, endPos, true);
        print(miscare);
        ActualMovement(startPos, endPos, miscare);
   
        if (miscare == 2)
            {
                print("Host-ul ar trebui sa schimbe");
                StartCoroutine(WaitAndChangeScene(chooseRandomScene()));
     
        }


    }
    [ObserversRpc]
    private void changeT()
    {
        changeTurn();
    }
    public string chooseRandomScene()
    {
        string [] scenes = new string[]
        {
            "X0-Online",
            "Connect4"
            //"1",
            //"2",
            //"3"
        };

        int random = Random.Range(0, scenes.Length);
        print(random);
        return scenes[random];
    }

    public  IEnumerator WaitAndChangeScene(string sceneName)
    {
        
        yield return new WaitForSeconds(0.5f);

        SendSceneChangeToServer(sceneName);
    }


    [ServerRpc(RequireOwnership = false)]
    private void SendSceneChangeToServer(string sceneName)
    {
        lastScene = sceneName;
        print(lastScene + " IS LASTSCENE");
        BootStrapNetworkManager.changeNetworkScene(sceneName, new string[] { "" });
    }


    [ServerRpc(RequireOwnership = false)]
    public void ClsoeSceneChangeToServer(string sceneToClose)
    {

        BootStrapNetworkManager.changeNetworkScene("", new string[] { sceneToClose });
    }

    //[ServerRpc]
    //public void closeCameraServer()
    //{
    //    closeCamereObserver();
    //}

    //[ObserversRpc]
    //private void closeCamereObserver()
    //{
    //    Destroy(Camera.main);
    //}

    [ServerRpc(RequireOwnership = false)]
    public void closeGameServer()
    {
        closeGame();
    }

    [ObserversRpc]
    private void closeGame()
    {

        Application.Quit();
        //destroyBoards();
        //BootStrapNetworkManager.returnToMainMenu();
    }
}
