using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{
    [SerializeField] public GameObject Camera;

    public static Vector3 cameraMain = new Vector3(4f, 4f, -2f);
    
    public static Cameras instance ;

        private void Start()
    {
        instance = this;
    }

    public GameObject spawnCameraMain()
    {
        return Instantiate(Camera, cameraMain, Quaternion.identity);
    }
}
