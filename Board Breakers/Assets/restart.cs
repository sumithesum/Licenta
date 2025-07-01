using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{
   
    void Start()
    {
        print("Restarting");
        
        GameObject temp = new GameObject("Temp");
        
        DontDestroyOnLoad(temp);

        CloseEveryScene();

        Scene ddolScene = temp.scene;
        GameObject[] rootObjects = ddolScene.GetRootGameObjects();

        foreach (GameObject obj in rootObjects)
        {
            
            if (obj.name != "Temp" || obj.name != "NetworkManager")
            {
                GameObject.Destroy(obj);
            }
           
        }

        
        GameObject.Destroy(temp);
        StartCoroutine(WAIT());
        
    }

    public IEnumerator WAIT()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainGame", LoadSceneMode.Additive);
    }

    public static void CloseEveryScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if(scene.name == "Bootstrap")
            {
                print("Am gasit ce nu trebuie inchis");
            }
            else if (scene.name != "Show")
            {
                SceneManager.UnloadSceneAsync(scene);
                Debug.Log("Closed scene: " + scene.name);
            }
        }
    }
}
