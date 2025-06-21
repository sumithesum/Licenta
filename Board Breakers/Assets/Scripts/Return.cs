using FishNet.Managing.Scened;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour
{

    public static  IEnumerator WaitAndChangeScene(int WhiteWon , string sceneName)
    {
    
        yield return new WaitForSeconds(3f);
        GameManager.ReturnFromMinigame(WhiteWon);
        OnlineSend.Local.ClsoeSceneChangeToServer(sceneName);

    }
}
