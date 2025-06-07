using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour
{
    [SerializeField]   public int caz ;
   
    void Start()
    {
        StartCoroutine(WaitAndChangeScene());
    }

    public IEnumerator WaitAndChangeScene()
    {

        yield return new WaitForSeconds(3f);
        GameManager.ReturnFromMinigame(caz);
        OnlineSend.Local.ClsoeSceneChangeToServer(this.gameObject.scene.name);

        
    }
 
}
