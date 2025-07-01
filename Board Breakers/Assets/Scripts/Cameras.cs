using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{
    [SerializeField] public GameObject CameraX;

    public static Vector3 cameraMain = new Vector3(4f, 4f, -2f);
    
    public static Cameras instance ;

        private void Start()
    {
        instance = this;
    }

    public GameObject spawnCameraMain()
    {
        return Instantiate(CameraX, cameraMain, Quaternion.identity);
    }

    private void Update()
    {
        //if (Camera.main == null)
        //{
        //    //spawnCameraMain();
        //    print("Spawned");
        //}
    }
}
