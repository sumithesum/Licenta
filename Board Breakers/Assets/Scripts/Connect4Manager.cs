using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using FishNet.Object;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class Connect4Manager : NetworkBehaviour
{



    public static bool isHostTurn = false;
    public static bool isGameFinished = false;

    [SerializeField]
    public Material Red;

    [SerializeField]
    public Material Blue;

    [SerializeField]List<Linie> linii = new List<Linie>();


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
        {

            waitForlevelToLoad();
            isGameFinished = false;

        }
        else
        {
            gameObject.GetComponent<Connect4Manager>().enabled = false;

        }
    }

    public void updateLinii()
    {
        linii.Clear();
        linii.Add(new Linie()); 

        
        Scene sceneCuLinii = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Connect4");
        if (!sceneCuLinii.isLoaded)
        {
            Debug.LogWarning("Scena cu liniile nu este încarcata!");
            return;
        }

        GameObject[] rootObjects = sceneCuLinii.GetRootGameObjects();
        Dictionary<string, Linie> liniiGasite = new Dictionary<string, Linie>();

        
        foreach (GameObject root in rootObjects)
        {
            Linie[] toateLiniile = root.GetComponentsInChildren<Linie>(true);
            foreach (Linie linie in toateLiniile)
            {
                if (!liniiGasite.ContainsKey(linie.name))
                    liniiGasite.Add(linie.name, linie);
            }
        }

       
        for (int i = 1; i <= 6; i++)
        {
            string nume = "Linie" + i;
            if (liniiGasite.TryGetValue(nume, out Linie linie))
            {
                linie.id = i;
                linie.copii.Clear();
                linie.copii.Add(null); 

                foreach (Transform child in linie.transform)
                    linie.copii.Add(child.gameObject);

                linii.Add(linie);
            }
            else
            {
                Debug.LogWarning($"Nu am gasit obiectul {nume} in scena {sceneCuLinii.name}");
            }
        }
    }

    public IEnumerator waitForlevelToLoad()
    {
       
        yield return new WaitForSeconds(1f);

        updateLinii();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = renderer.material;

                    if (renderer.gameObject.name != "1" &&
                        renderer.gameObject.name != "2" &&
                        renderer.gameObject.name != "3" &&
                        renderer.gameObject.name != "4" &&
                        renderer.gameObject.name != "5" &&
                        renderer.gameObject.name != "6" &&
                        renderer.gameObject.name != "7")
                        return;
                    updateLinii();
                    
                    int coloana = int.Parse(renderer.gameObject.name) ;
                    print(coloana + " : " + isHostTurn + " : " + PlayerHost.isHost);
                    
                    if (checkIfTurn() && !isGameFinished)
                    {
                        int linie = isSpace(coloana);
                        print(linie);
                        if (linie != -1)
                            sendMoveToServer(linie, coloana , PlayerHost.isHost);
                            //PutCircle(linie, coloana);

                    }
                }
            }
        }
    }

    public bool checkIfTurn()
    {
        
        return isHostTurn == PlayerHost.isHost;
    }

    public int isSpace(int poz)
    {
        updateLinii();
        for (int i = 1; i <= 6; i++) {
            Linie linie = linii[i];
            if (linie.copii[poz].GetComponent<Renderer>().material.name == "Invisible (Instance)")
                return linie.id;
               
        }
        return -1;

    }

    //public void PutCircle(int linie , int coloana)
    //{
    //    if (PlayerHost.isHost)
    //        linii[linie].copii[coloana].GetComponent<Renderer>().material = Blue;
    //    else
    //        linii[linie].copii[coloana].GetComponent<Renderer>().material = Red;
    //    PlayerHost.isHost = !PlayerHost.isHost;
    //    isHostTurn = !isHostTurn;
        
    //    checkifwon();
    //}

    void checkifwon()
    {
        updateLinii();
        for (int i = 6; i >= 1; i--)
        {
            
              
            ///Verificam pe aceasi linie
            for (int poz = 1; poz <= 7; poz++)
            {
                if (poz <= 4)
                {
                    string mat1 = linii[i].copii[poz].GetComponent<Renderer>().material.name;
                    string mat2 = linii[i].copii[poz + 1].GetComponent<Renderer>().material.name;
                    string mat3 = linii[i].copii[poz + 2].GetComponent<Renderer>().material.name;
                    string mat4 = linii[i].copii[poz + 3].GetComponent<Renderer>().material.name;
                    
                    if (mat1 == mat2 && mat2 == mat3 && mat3 == mat4 && mat4 != "Invisible (Instance)")
                    {
                        //Sa castigat
                        print("Sa gastigat linie de la poz " + i + ":" + poz);
                        checkWhoWon(i, poz, false);
                    }
                }
            }




            if (i >= 4)
            {
                //Veficam jos
                for (int poz = 1; poz <= 7; poz++)
                {
                    string mat1 = linii[i].copii[poz].GetComponent<Renderer>().material.name;
                    string mat2 = linii[i - 1].copii[poz].GetComponent<Renderer>().material.name;
                    string mat3 = linii[i - 2].copii[poz].GetComponent<Renderer>().material.name;
                    string mat4 = linii[i - 3].copii[poz].GetComponent<Renderer>().material.name;

                    if (mat1 == mat2 && mat2 == mat3 && mat3 == mat4 && mat4 != "Invisible (Instance)")
                    {
                        //Sa castigat
                        print("Sa gastigat jos de la poz " + i + ":" + poz);
                        checkWhoWon(i, poz, false);
                    }
                }


                for (int poz = 1; poz <= 7; poz++)
                {
                    //Verificam pe diagonala principala
                    if (poz <= 4)
                    {
                        string mat1 = linii[i].copii[poz].GetComponent<Renderer>().material.name;
                        string mat2 = linii[i - 1].copii[poz + 1].GetComponent<Renderer>().material.name;
                        string mat3 = linii[i - 2].copii[poz + 2].GetComponent<Renderer>().material.name;
                        string mat4 = linii[i - 3].copii[poz + 3].GetComponent<Renderer>().material.name;

                        if (mat1 == mat2 && mat2 == mat3 && mat3 == mat4 && mat4 != "Invisible (Instance)")
                        {
                            //Sa castigat
                            print("Sa gastigat diagonala principala de la poz " + i + ":" + poz);
                            checkWhoWon(i, poz, false);
                        }
                    }
                    //Veficam pe diagonala secundara
                    else
                    {
                        string mat1 = linii[i].copii[poz].GetComponent<Renderer>().material.name;
                        string mat2 = linii[i - 1].copii[poz - 1].GetComponent<Renderer>().material.name;
                        string mat3 = linii[i - 2].copii[poz - 2].GetComponent<Renderer>().material.name;
                        string mat4 = linii[i - 3].copii[poz - 3].GetComponent<Renderer>().material.name;

                        if (mat1 == mat2 && mat2 == mat3 && mat3 == mat4 && mat4 != "Invisible (Instance)")
                        {
                            //Sa castigat
                            print("Sa gastigat diagonala secundara de la poz " + i + ":" + poz);
                            checkWhoWon(i, poz, false);
                        }
                    }
                    if(poz == 4)
                    {
                        string mat1 = linii[i].copii[poz].GetComponent<Renderer>().material.name;
                        string mat2 = linii[i - 1].copii[poz - 1].GetComponent<Renderer>().material.name;
                        string mat3 = linii[i - 2].copii[poz - 2].GetComponent<Renderer>().material.name;
                        string mat4 = linii[i - 3].copii[poz - 3].GetComponent<Renderer>().material.name;

                        if (mat1 == mat2 && mat2 == mat3 && mat3 == mat4 && mat4 != "Invisible (Instance)")
                        {
                            //Sa castigat
                            print("Sa gastigat diagonala secundara de la poz " + i + ":" + poz);
                            checkWhoWon(i, poz, false);
                        }
                    }
                }


            }
        }

        int numarCompletate = 0;
        for (int i = 6; i >= 1; i--)
            for (int j = 1; j <= 7; j++)
                if (linii[i].copii[j].GetComponent<Renderer>().material.name == "Invisible (Instance)")
                    numarCompletate++;
        if(numarCompletate >= 42)
        {
            checkWhoWon(-1, -1, true);
        }
    }

    public void checkWhoWon(int linie , int coloana , bool isDraw)
    {
        if(isDraw)
            StartCoroutine(Return.WaitAndChangeScene(3, "Connect4"));

        Material m = linii[linie].copii[coloana].GetComponent<Renderer>().material;
        if (m.name == (Blue.name + " (Instance)"))
        {
            print("Host-ul a castigat");
            StartCoroutine(Return.WaitAndChangeScene(1, "Connect4"));
        }
        else
        {
            print("Host-ul nu a castigat");
            StartCoroutine(Return.WaitAndChangeScene(2, "Connect4"));
        }
        
    }
    [ServerRpc(RequireOwnership = false)]
    public void gameFinishedServer()
    {
        gameFinishedObserver();

    }

    [ObserversRpc]
    private void gameFinishedObserver()
    {
        isGameFinished = true;
    }


    [ServerRpc(RequireOwnership = false)]
    public void sendMoveToServer(int linie, int coloana , bool isHost)
    {
        changeMaterial(linie,coloana,isHost);
        
    }

    [ObserversRpc]
    private void changeMaterial(int linie,int coloana , bool isHost)
    {
        updateLinii();
        print("[Client]: Am primit : " + linie + " : " + coloana + " : " + isHost);
        if (isHost)
            linii[linie].copii[coloana].GetComponent<Renderer>().material = Blue;
        else
            linii[linie].copii[coloana].GetComponent<Renderer>().material = Red;

        isHostTurn = !isHostTurn;
        
        
        checkifwon();

    }

}
