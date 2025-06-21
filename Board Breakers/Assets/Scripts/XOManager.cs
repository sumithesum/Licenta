using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet;
using static ListTiles;
using FishNet.Object.Synchronizing;
using FishNet.CodeGenerating;
using static Test;
using System;
using System.Reflection;

public class XOManager : NetworkBehaviour
{


    public bool isCircle = true;


    public static bool isCircleTurn = false;

    [SerializeField]
    public Material X;

    [SerializeField]
    public Material O;




    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
        {
            

            isCircle = PlayerHost.isHost;
            waitForlevelToLoad();
            

            string s = isCircle ? "Cirlce" : "X";
            Test.StaticSetScore(s);
        }
        else
        {
            gameObject.GetComponent<XOManager>().enabled = false;

        }
    }
    public IEnumerator waitForlevelToLoad()
    {
        yield return new WaitForSeconds(1f);
    }


    [ServerRpc]
    private void RequestPlayerShapeServerRpc()
    {
        int count = InstanceFinder.ServerManager.Clients.Count;
        bool assignedShape = (count == 1);

        Debug.Log($"[Server] Player count: {count}. Assigning circle? {assignedShape}");

        TargetReceivePlayerShape(base.Owner, assignedShape);
    }

    [TargetRpc]
    private void TargetReceivePlayerShape(NetworkConnection owner, bool isShape)
    {
        Debug.Log($"[Client] Am primit shapeul de la server: {isShape}");

        isCircle = isShape;

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
                    

                    if (mat == X || mat == O) return;

                    print(hit.collider.gameObject.name);

                    if (isCircle == isCircleTurn)
                    {
                        int index = int.Parse(hit.collider.gameObject.name);


                        
                        SendToServerMove(index);
                  
                        
                        string s = (isCircle == isCircleTurn) + " : " + isCircleTurn + " : " + isCircle;
                        Test.StaticSetScore(s);
                    }
                }
            }
        }
    }

    //public void checkWhoWon(int x)
    //{
    //    if(ListTiles.Tiles[x].GetComponent<Renderer>().material.name == "X (Instance)")
    //    {
    //        print("X a castigat");
    //        StartCoroutine(Return.WaitAndChangeScene(2, "X0-Online"));
    //    }else
    //    {
    //        StartCoroutine(Return.WaitAndChangeScene(1, "X0-Online"));
    //        print("O a castigat");
    //    }
    //}
    public void checkWhoWon(int x)
    {
        Material m = ListTiles.Tiles[x].GetComponent<Renderer>().material;
        if (m.name == (X.name + " (Instance)"))
        {
            print("X a câștigat");
            StartCoroutine(Return.WaitAndChangeScene(2, "X0-Online"));
        }
        else
        {
            print("O a câștigat");
            StartCoroutine(Return.WaitAndChangeScene(1, "X0-Online"));
        }
    }



    //[ObserversRpc]
    //public void checks()
    //{
    //    // 1 2 3
    //    // 4 5 6
    //    // 7 8 9

    //    if(ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[2].GetComponent<Renderer>().material.name
    //        &&
    //        ListTiles.Tiles[3].GetComponent<Renderer>().material.name == ListTiles.Tiles[1].GetComponent<Renderer>().material.name
    //        &&
    //            (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "X (Instance)"
    //            ||
    //            ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "O (Instance)"))
    //    {
    //        ListTiles.Tiles[0].gameObject.SetActive(false);
    //        checkWhoWon(1);
    //    }

    //    else if (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[4].GetComponent<Renderer>().material.name
    //        &&
    //        ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[7].GetComponent<Renderer>().material.name
    //        &&
    //            (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "X (Instance)"
    //            ||
    //            ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "O (Instance)"))
    //    {
    //        ListTiles.Tiles[0].gameObject.SetActive(false);
    //        checkWhoWon(1);
    //    }
    //    else if (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[4].GetComponent<Renderer>().material.name
    //        &&
    //        ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[7].GetComponent<Renderer>().material.name
    //        &&
    //            (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "X (Instance)"
    //            ||
    //            ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "O (Instance)"))
    //    {
    //        ListTiles.Tiles[0].gameObject.SetActive(false);
    //        checkWhoWon(1);
    //    }
    //    else if (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[5].GetComponent<Renderer>().material.name
    //        &&
    //        ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[9].GetComponent<Renderer>().material.name
    //        &&
    //            (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "X (Instance)"
    //            ||
    //            ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "O (Instance)"))
    //    {
    //        ListTiles.Tiles[0].gameObject.SetActive(false);
    //        checkWhoWon(1);
    //    }
    //    else if (ListTiles.Tiles[4].GetComponent<Renderer>().material.name == ListTiles.Tiles[5].GetComponent<Renderer>().material.name
    //        &&
    //        ListTiles.Tiles[4].GetComponent<Renderer>().material.name == ListTiles.Tiles[6].GetComponent<Renderer>().material.name
    //        &&
    //            (ListTiles.Tiles[4].GetComponent<Renderer>().material.name == "X (Instance)"
    //            ||
    //            ListTiles.Tiles[4].GetComponent<Renderer>().material.name == "O (Instance)"))
    //    {
    //        ListTiles.Tiles[0].gameObject.SetActive(false);
    //        checkWhoWon(4);
    //    }
    //    else if (ListTiles.Tiles[7].GetComponent<Renderer>().material.name == ListTiles.Tiles[8].GetComponent<Renderer>().material.name
    //        &&
    //        ListTiles.Tiles[7].GetComponent<Renderer>().material.name == ListTiles.Tiles[9].GetComponent<Renderer>().material.name
    //        &&
    //            (ListTiles.Tiles[7].GetComponent<Renderer>().material.name == "X (Instance)"
    //            ||
    //            ListTiles.Tiles[7].GetComponent<Renderer>().material.name == "O (Instance)"))
    //    {
    //        ListTiles.Tiles[0].gameObject.SetActive(false);
    //        checkWhoWon(7);
    //    }
    //    else if (ListTiles.Tiles[2].GetComponent<Renderer>().material.name == ListTiles.Tiles[8].GetComponent<Renderer>().material.name
    //        &&
    //        ListTiles.Tiles[2].GetComponent<Renderer>().material.name == ListTiles.Tiles[5].GetComponent<Renderer>().material.name
    //        &&
    //            (ListTiles.Tiles[2].GetComponent<Renderer>().material.name == "X (Instance)"
    //            ||
    //            ListTiles.Tiles[2].GetComponent<Renderer>().material.name == "O (Instance)"))
    //    {
    //        ListTiles.Tiles[0].gameObject.SetActive(false);
    //        checkWhoWon(2);
    //    }
    //    else if (ListTiles.Tiles[3].GetComponent<Renderer>().material.name == ListTiles.Tiles[6].GetComponent<Renderer>().material.name
    //        &&
    //        ListTiles.Tiles[3].GetComponent<Renderer>().material.name == ListTiles.Tiles[9].GetComponent<Renderer>().material.name
    //        &&
    //            (ListTiles.Tiles[3].GetComponent<Renderer>().material.name == "X (Instance)"
    //            ||
    //            ListTiles.Tiles[3].GetComponent<Renderer>().material.name == "O (Instance)"))
    //    {
    //        ListTiles.Tiles[0].gameObject.SetActive(false);
    //        checkWhoWon(3);
    //    }
    //    else if (ListTiles.Tiles[3].GetComponent<Renderer>().material.name == ListTiles.Tiles[5].GetComponent<Renderer>().material.name
    //        &&
    //        ListTiles.Tiles[3].GetComponent<Renderer>().material.name == ListTiles.Tiles[7].GetComponent<Renderer>().material.name
    //        &&
    //            (ListTiles.Tiles[3].GetComponent<Renderer>().material.name == "X (Instance)"
    //            ||
    //            ListTiles.Tiles[3].GetComponent<Renderer>().material.name == "O (Instance)"))
    //    {
    //        ListTiles.Tiles[0].gameObject.SetActive(false);
    //        checkWhoWon(3);
    //    }
    //    ///Mai e cazul de draw
    //    else
    //    {
    //        int ok = 0;
    //        for(int i = 1; i <= 9;i++)
    //        {
    //            if ((ListTiles.Tiles[i].GetComponent<Renderer>().material.name == "X (Instance)"
    //            ||
    //            ListTiles.Tiles[i].GetComponent<Renderer>().material.name == "O (Instance)"))
    //                ok++;
    //        }
    //        if(ok >= 8)
    //        {
    //            ListTiles.Tiles[0].gameObject.SetActive(false);
    //            print(false);
    //            StartCoroutine(Return.WaitAndChangeScene(3, "X0-Online"));
    //        }
    //    }


    //}
    [ObserversRpc]
    public void checks()
    {
        int[][] winningCombinations = new int[][]
        {
        new int[] {1, 2, 3},
        new int[] {4, 5, 6},
        new int[] {7, 8, 9},
        new int[] {1, 4, 7},
        new int[] {2, 5, 8},
        new int[] {3, 6, 9},
        new int[] {1, 5, 9},
        new int[] {3, 5, 7}
        };

        foreach (int[] combo in winningCombinations)
        {
            
            String mat1 = ListTiles.Tiles[combo[0]].GetComponent<Renderer>().material.name;
            String mat2 = ListTiles.Tiles[combo[1]].GetComponent<Renderer>().material.name;
            String mat3 = ListTiles.Tiles[combo[2]].GetComponent<Renderer>().material.name;
            
            if (mat1 == mat2 && mat2 == mat3 && (mat1 == (X.name + " (Instance)") || mat1 == (O.name + " (Instance)")))
            {
                print("E combo: " + combo[0] + combo[1] + combo[2]);
                ListTiles.Tiles[0].gameObject.SetActive(false);
                checkWhoWon(combo[0]);
                return;
            }
        }

        // Check for draw
        int filled = 0;
        for (int i = 1; i <= 9; i++)
        {
            Material m = ListTiles.Tiles[i].GetComponent<Renderer>().material;
            if (m == X || m == O)
                filled++;
        }

        if (filled == 9)
        {
            ListTiles.Tiles[0].gameObject.SetActive(false);
            print("Draw");
            StartCoroutine(Return.WaitAndChangeScene(3, "X0-Online"));
        }
    }


    [ServerRpc]
    public void SendToServerMove(int index)
    {
        changeMaterial(index, isCircle);
        checks();
    }


    [ObserversRpc]
    public void changeMaterial(int index, bool isCircle)
    {
        
        ListTiles.Tiles[index].GetComponent<Renderer>().material = isCircle ? O : X;
        
        if (isCircleTurn)
            isCircleTurn = false;
        else
            isCircleTurn = true;

        print("Now it s time for :" + isCircleTurn);
        
        
    }


}
