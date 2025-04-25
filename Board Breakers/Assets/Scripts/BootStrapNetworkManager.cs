using FishNet.Object;
using FishNet.Connection;
using FishNet.Managing.Scened;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class BootStrapNetworkManager : NetworkBehaviour
{
    private static BootStrapNetworkManager instance;

    public static List<GameObject> SpawnPoints;

    [SerializeField] GameObject playerMainGame;
    public void Awake()
    {
        instance = this;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
    }



    public static void changeNetworkScene(string sceneName , string[] scenesToClose)
    {
        
        instance.CloseScenes(scenesToClose);

        SceneLoadData sld = new SceneLoadData(sceneName);

        NetworkConnection[] cons = instance.ServerManager.Clients.Values.ToArray();
        instance.SceneManager.LoadConnectionScenes(cons, sld);


        SpawnPlayers(sceneName);

        
    }

    [ServerRpc(RequireOwnership = false)]
    void CloseScenes(string[] scenesToClose)
    {
        

        closeScenesObserver(scenesToClose);
        
    }

    [ObserversRpc]
    void closeScenesObserver(string[] scenesToClose)
    {
        
        foreach (var scnename in scenesToClose)
        {
   
         UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scnename);
            
        }
    }


    public static void SpawnPlayers(string SceneName)
    {
        GameObject prefab = instance.getPlayerPrefab(SceneName);
        updateSpawnPoints(SceneName);

        int index = 0;
        foreach (NetworkConnection conn in instance.ServerManager.Clients.Values)
        {

            
            Vector3 spawnPos = SpawnPoints[index].transform.position;
            Quaternion spawnRot = SpawnPoints[index].transform.rotation;

            GameObject player = Instantiate(prefab, spawnPos, spawnRot);
            instance.Spawn(player, conn);
            index++;
        }
    }

    public GameObject getPlayerPrefab(string SceneName)
    {
        switch (SceneName)
        {
            case "MainGame":
                return playerMainGame;
                
        }
        return null;
    }

    public static void updateSpawnPoints(string SceneName)
    {
        SpawnPoints = new List<GameObject>();

        switch (SceneName)
        {
            case "MainGame":

                GameObject P1 = new GameObject();
                P1.transform.position = new Vector3(0f,0f,0f);
                P1.transform.rotation = new Quaternion(0f, 0f, 0f,0f);
                P1.transform.localScale = new Vector3(1f,1f,1f);
                P1.name = "P1";

                SpawnPoints.Add(P1);

                GameObject P2 = new GameObject();
                P2.transform.position = new Vector3(1f, 0f, 0f);
                P2.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                P2.transform.localScale = new Vector3(1f, 1f, 1f);
                P2.name = "P2";

                SpawnPoints.Add(P2);
                break;

        }
        
    }

    


}
