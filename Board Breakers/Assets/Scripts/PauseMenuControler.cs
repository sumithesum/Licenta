using FishNet.Utility.Extension;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenuControler : MonoBehaviour
{

    public static string[] AllSecenes = { "1","2","3","MainGame","X0-Online" }; 

    [SerializeField] public GameObject Main, Sound , Panel;

    public Vector3 orginalpos =  new Vector3(10000f, 100000f, 1000f);

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<PauseMenuControler>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !UnityEngine.SceneManagement.SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            ///
            Main.gameObject.SetActive(!Main.gameObject.activeSelf);
            Panel.gameObject.SetActive(!Panel.gameObject.activeSelf);


            //Vector3 aux = Camera.main.transform.position;
            //Camera.main.transform.position = orginalpos;
            //orginalpos = aux;
        }
    }

    public void ReturnToMainMenu()
    {
        Application.Quit();
    }

    public void ReturnToGame()
    {
        print("Hiii");
        Main.gameObject.SetActive(false);
        Sound.gameObject.SetActive(false);
        Panel.gameObject.SetActive(false);

        //Vector3 aux = Camera.main.transform.position;
        //Camera.main.transform.position = orginalpos;
        //orginalpos = aux;
    }

    public void Sounds()
    {
        Sound.gameObject.SetActive(true);
        Main.gameObject.SetActive(false);
    }

    public void Mains()
    {
        Sound.gameObject.SetActive(false);
        Main.gameObject.SetActive(true);
    }




}
