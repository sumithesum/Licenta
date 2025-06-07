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


            string s = isCircle ? "Cirlce" : "X";
            Test.StaticSetScore(s);
        }
        else
        {
            gameObject.GetComponent<XOManager>().enabled = false;

        }
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

    [ObserversRpc]
    public void checks()
    {
        // 1 2 3
        // 4 5 6
        // 7 8 9

        if(ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[2].GetComponent<Renderer>().material.name
            &&
            ListTiles.Tiles[3].GetComponent<Renderer>().material.name == ListTiles.Tiles[1].GetComponent<Renderer>().material.name
            &&
                (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "X (Instance)"
                ||
                ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "O (Instance)"))
        {
            ListTiles.Tiles[0].gameObject.SetActive(false);
            
        }

        else if (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[4].GetComponent<Renderer>().material.name
            &&
            ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[7].GetComponent<Renderer>().material.name
            &&
                (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "X (Instance)"
                ||
                ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "O (Instance)"))
        {
            ListTiles.Tiles[0].gameObject.SetActive(false);
            print("Salut");
        }
        else if (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[4].GetComponent<Renderer>().material.name
            &&
            ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[7].GetComponent<Renderer>().material.name
            &&
                (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "X (Instance)"
                ||
                ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "O (Instance)"))
        {
            ListTiles.Tiles[0].gameObject.SetActive(false);
            print("Salut");
        }
        else if (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[5].GetComponent<Renderer>().material.name
            &&
            ListTiles.Tiles[1].GetComponent<Renderer>().material.name == ListTiles.Tiles[9].GetComponent<Renderer>().material.name
            &&
                (ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "X (Instance)"
                ||
                ListTiles.Tiles[1].GetComponent<Renderer>().material.name == "O (Instance)"))
        {
            ListTiles.Tiles[0].gameObject.SetActive(false);
            print("Salut");
        }
        else if (ListTiles.Tiles[4].GetComponent<Renderer>().material.name == ListTiles.Tiles[5].GetComponent<Renderer>().material.name
            &&
            ListTiles.Tiles[4].GetComponent<Renderer>().material.name == ListTiles.Tiles[6].GetComponent<Renderer>().material.name
            &&
                (ListTiles.Tiles[4].GetComponent<Renderer>().material.name == "X (Instance)"
                ||
                ListTiles.Tiles[4].GetComponent<Renderer>().material.name == "O (Instance)"))
        {
            ListTiles.Tiles[0].gameObject.SetActive(false);
            print("Salut");
        }
        else if (ListTiles.Tiles[7].GetComponent<Renderer>().material.name == ListTiles.Tiles[8].GetComponent<Renderer>().material.name
            &&
            ListTiles.Tiles[7].GetComponent<Renderer>().material.name == ListTiles.Tiles[9].GetComponent<Renderer>().material.name
            &&
                (ListTiles.Tiles[7].GetComponent<Renderer>().material.name == "X (Instance)"
                ||
                ListTiles.Tiles[7].GetComponent<Renderer>().material.name == "O (Instance)"))
        {
            ListTiles.Tiles[0].gameObject.SetActive(false);
            print("Salut");
        }
        else if (ListTiles.Tiles[2].GetComponent<Renderer>().material.name == ListTiles.Tiles[8].GetComponent<Renderer>().material.name
            &&
            ListTiles.Tiles[2].GetComponent<Renderer>().material.name == ListTiles.Tiles[5].GetComponent<Renderer>().material.name
            &&
                (ListTiles.Tiles[2].GetComponent<Renderer>().material.name == "X (Instance)"
                ||
                ListTiles.Tiles[2].GetComponent<Renderer>().material.name == "O (Instance)"))
        {
            ListTiles.Tiles[0].gameObject.SetActive(false);
            print("Salut");
        }
        else if (ListTiles.Tiles[3].GetComponent<Renderer>().material.name == ListTiles.Tiles[6].GetComponent<Renderer>().material.name
            &&
            ListTiles.Tiles[3].GetComponent<Renderer>().material.name == ListTiles.Tiles[9].GetComponent<Renderer>().material.name
            &&
                (ListTiles.Tiles[3].GetComponent<Renderer>().material.name == "X (Instance)"
                ||
                ListTiles.Tiles[3].GetComponent<Renderer>().material.name == "O (Instance)"))
        {
            ListTiles.Tiles[0].gameObject.SetActive(false);
            print("Salut");
        }
        else if (ListTiles.Tiles[3].GetComponent<Renderer>().material.name == ListTiles.Tiles[5].GetComponent<Renderer>().material.name
            &&
            ListTiles.Tiles[3].GetComponent<Renderer>().material.name == ListTiles.Tiles[7].GetComponent<Renderer>().material.name
            &&
                (ListTiles.Tiles[3].GetComponent<Renderer>().material.name == "X (Instance)"
                ||
                ListTiles.Tiles[3].GetComponent<Renderer>().material.name == "O (Instance)"))
        {
            ListTiles.Tiles[0].gameObject.SetActive(false);
            print("Salut");
        }
        ///Mai e cazul de draw
        else
        {
            int ok = 0;
            for(int i = 1; i <= 9;i++)
            {
                if ((ListTiles.Tiles[i].GetComponent<Renderer>().material.name == "X (Instance)"
                ||
                ListTiles.Tiles[i].GetComponent<Renderer>().material.name == "O (Instance)"))
                    ok++;
            }
            if(ok >= 8)
            {
                ListTiles.Tiles[0].gameObject.SetActive(false);
                print("Draw");
            }
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
