using FishNet.Connection;

using UnityEngine;
using FishNet.Object;
using TMPro;
using FishNet.Observing;
using FishNet;
using FishNet.Object.Synchronizing;
using FishNet.CodeGenerating;


public class BallManagement : NetworkBehaviour
{
    [SerializeField]
    public TextMeshProUGUI Score;

    public int P1score;

    public int P2score;

    public float direction = 1f;

    public float speed;

    public float angle;

    public bool gameStarted = false;


     public int playerCount;  

    public override void OnStartClient()
    {
        base.OnStartClient();
        StartGame();
    }


    public void StartGame()
    {

        gameObject.transform.position = new Vector3(0, 0, 15f);

        speed = 0.01f;

        int side = Random.Range(0, 2) < 1 ? 0 : 1;

        angle = Random.Range(-0.5f, 0.5f);


        if (side == 0)
        {
            direction *= -1;
        }

        
    }

    public void startRound()
    {
        StartGame();
        print("StartRound");
        print(P1score + "   :     " + P2score);
    }

    void Update()
    {
        //Score.text = P1score + " : " + P2score;

        print("Salut");
        print(gameObject.transform.position);

        if (gameObject.transform.position.y + angle * speed >= 9 || gameObject.transform.position.y + angle * speed <= -9)
        {
            angle *= -1;
            speed += 0.01f;
        }


        if (gameObject.transform.position.x + direction * speed <= -13)
        {
            P2score++;
            startRound();
            speed += 0.01f;
        }

        if (gameObject.transform.position.x + direction * speed >= 13)
        {
            P1score++;
            startRound();
            speed += 0.01f;
        }



        MoveServerRpc( new Vector3(gameObject.transform.position.x + direction * speed,
            gameObject.transform.position.y + angle * speed, 15),direction,speed,angle);

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            direction *= -1;

            
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 paddleCenter = collision.collider.bounds.center;

            angle = (hitPoint.y - paddleCenter.y) * 0.5f;

            speed += 0.01f;

            
        }
    }

    [ServerRpc]
    private void MoveServerRpc(Vector3 move,float direction , float speed , float angle)
    {
        MoveOnClienst(move , direction , speed, angle);
    }

    [ObserversRpc]
    private void MoveOnClienst(Vector3 move, float _direction, float _speed, float _angle)
    {
        print("Salut");
        transform.position = move;
        direction = _direction;
        speed = _speed;
        angle = _angle;

    }
}
