using FishNet.Object;
using FishNet.Connection;
using FishNet.Managing.Scened;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using FishNet;
using System.Data;


public class BootStrapNetworkManager : NetworkBehaviour
{
    private static BootStrapNetworkManager instance;

    public static List<GameObject> SpawnPoints;

    [SerializeField] GameObject playerMainGame, playerX0;

 
   public static Dictionary<string, GameObject[]> GamePlayers = new Dictionary<string, GameObject[]>();

    public static Camera GetMainCameraFromScene(string sceneName)
    {
        var scene = SceneManager.GetScene(sceneName);

        if (!scene.isLoaded)
        {
            Debug.LogWarning("Scena " + sceneName + " nu e încărcată!");
            return null;
        }

        foreach (GameObject rootObj in scene.GetRootGameObjects())
        {
            Camera cam = rootObj.GetComponentInChildren<Camera>();
            if (cam != null && cam.CompareTag("MainCamera"))
                return cam;
        }

        return null;
    }

    public void Awake()
    {
        instance = this;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public static void changeNetworkScene(string sceneName, string[] scenesToClose)
    {
        instance.CloseScenes(scenesToClose);
        if(sceneName != "") { 
            SceneLoadData sld = new SceneLoadData(sceneName);
            NetworkConnection[] cons = instance.ServerManager.Clients.Values.ToArray();

            instance.SceneManager.LoadConnectionScenes(cons, sld);

            instance.StartCoroutine(instance.WaitAndSpawn(sceneName));
            }
        }
    private IEnumerator WaitAndSpawn(string sceneName)
    {
        while (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            yield return null;
        }

        SpawnPlayers(sceneName);
    }

    [ServerRpc(RequireOwnership = false)]
    void CloseScenes(string[] scenesToClose)
    {
        closeScenesObserver(scenesToClose);
    }

    [ObserversRpc(ExcludeOwner = false)]
    void closeScenesObserver(string[] scenesToClose)
    {
        if (scenesToClose[0] == "")
            return;



        foreach (var scnename in scenesToClose)
        {
            if (GamePlayers.ContainsKey(scnename) && scnename != "MainGame")
            {
                foreach (var player in GamePlayers[scnename])
                    Destroy(player);
                GamePlayers.Remove(scnename);
                print("Am scos totul din : " + scnename);
            }

            if(UnityEngine.SceneManagement.SceneManager.GetSceneByName(scnename).isLoaded)
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scnename);

        }
    }

    public static void SpawnPlayers(string SceneName)
    {
        GameObject prefab = instance.getPlayerPrefab(SceneName);
        updateSpawnPoints(SceneName);

        
        if (!GamePlayers.ContainsKey(SceneName))
        {
            GamePlayers[SceneName] = new GameObject[instance.ServerManager.Clients.Count];
        }

        int index = 0;
        foreach (NetworkConnection conn in instance.ServerManager.Clients.Values)
        {
            Vector3 spawnPos = SpawnPoints[index].transform.position;
            Quaternion spawnRot = SpawnPoints[index].transform.rotation;

            GameObject player = Instantiate(prefab, spawnPos, spawnRot);
            GamePlayers[SceneName][index] = player;
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
            case "X0-Online":
                return playerX0;
            case "1":
            case "2":
            case "3":
                return playerX0;
        }
        return null;
    }

    public static void updateSpawnPoints(string SceneName)
    {
        switch (SceneName)
        {
            case "MainGame":
                SpawnPoints = new List<GameObject>();
                GameObject P1 = new GameObject();
                P1.transform.position = new Vector3(0f, 0f, 0f);
                P1.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                P1.transform.localScale = new Vector3(1f, 1f, 1f);
                P1.name = "P1";

                SpawnPoints.Add(P1);

                GameObject P2 = new GameObject();
                P2.transform.position = new Vector3(1f, 0f, 0f);
                P2.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                P2.transform.localScale = new Vector3(1f, 1f, 1f);
                P2.name = "P2";

                SpawnPoints.Add(P2);
                break;
            case "X0-Online":
                SpawnPoints = new List<GameObject>();
                SpawnPoints.Add(new GameObject("X0P1") { transform = { position = new Vector3(0, 0, 0) } });
                SpawnPoints.Add(new GameObject("X0P2") { transform = { position = new Vector3(1, 0, 0) } });
                break;
            case "1":
            case "2":
            case "3":
                SpawnPoints = new List<GameObject>();
                SpawnPoints.Add(new GameObject("X0P1") { transform = { position = new Vector3(0, 0, 0) } });
                SpawnPoints.Add(new GameObject("X0P2") { transform = { position = new Vector3(1, 0, 0) } });
                break;
        }           
    }
}

