using UnityEngine;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using FishNet;
using System.Linq;



public class SceneLoader : NetworkBehaviour
{
    public static SceneLoader instance;

    public override void OnStartServer()
    {
        base.OnStartServer();
        if(instance == null)
            instance = this;

        nivele[0] = "PingPongOnline";
    }

    public Dictionary<int, string> nivele = new Dictionary<int, string>();

    private SceneLoadData loadedScene;

    [Server]
    public void LoadMinigameScene(int sceneId)
    {
        
        SceneLookupData lookup = new SceneLookupData(nivele[sceneId]);

        
        LoadOptions options = new LoadOptions
        {
            AutomaticallyUnload = false,
            AllowStacking = false,
            LocalPhysics = LocalPhysicsMode.None
        };

        
        loadedScene = new SceneLoadData
        {
            SceneLookupDatas = new SceneLookupData[] { lookup },
            ReplaceScenes = ReplaceOption.None,
            Options = options
        };

        
        InstanceFinder.SceneManager.LoadGlobalScenes(loadedScene);
    }

    [Server]
    public void UnloadMinigameScene(int sceneId)
    {
        SceneUnloadData unloadData = new SceneUnloadData
        {
            SceneLookupDatas = new SceneLookupData[] { new SceneLookupData(nivele[sceneId]) }
        };


        InstanceFinder.SceneManager.UnloadGlobalScenes(unloadData);
    }

}
