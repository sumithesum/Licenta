using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class PingPongGame : MonoBehaviour
{
    public int P1score = 0;

    public int P2score = 0;

    public float speed = 0.1f;

    public GameObject Player1;

    public GameObject Player2;

    public GameObject Ball;

    public TextMeshProUGUI Score;

    float direction = 1f;

    float angle = 0f;



    // Start is called before the first frame update
    void Start()
    {
        P2score = 0;
        P1score = 0;

        startRound();
    }

    // Update is called once per frame
    void Update()
    {
        Score.text = P1score + " : " + P2score;

        if(Ball.transform.position.y + angle * speed >= 30 || Ball.transform.position.y + angle * speed <= -30)
        {
            angle *= -1;
            speed += 0.03f;
        }

        
        if (Ball.transform.position.x + direction * speed <= -70)  
        {
            P2score++;
            startRound();
            speed += 0.03f;
        }

        if(Ball.transform.position.x + direction * speed >= 70)
        {
            P1score++;
            startRound();
            speed += 0.03f;
        }

        if ( (Ball.transform.position.y <= Player1.transform.position.y+10 && Ball.transform.position.y >= Player1.transform.position.y - 10 ) && (int)Ball.transform.position.x == (int)Player1.transform.position.x)
        {
            direction *= -1;
            angle *= -1;
            speed += 0.03f;
        }
        if ((Ball.transform.position.y <= Player2.transform.position.y + 10 && Ball.transform.position.y >= Player2.transform.position.y - 10) && (int)Ball.transform.position.x == (int)Player2.transform.position.x)
        {
            direction *= -1;
            angle *= -1;
            speed += 0.03f;
        }

        if (Input.GetKey(KeyCode.S) && Player1.transform.position.y >= -30)
        {
            Player1.transform.position = new Vector3(Player1.transform.position.x, (float)(Player1.transform.position.y - speed), Player1.transform.position.z);;
        }

        if (Input.GetKey(KeyCode.W) && Player1.transform.position.y <= 30)
        {
            Player1.transform.position = new Vector3(Player1.transform.position.x, (float)(Player1.transform.position.y + speed), Player1.transform.position.z); ;
        }

        if (Player2.transform.position.y > 30)
            Player2.transform.position = new Vector3(Player2.transform.position.x,30f, Player2.transform.position.z); ;
        if (Player2.transform.position.y < -30)
            Player2.transform.position = new Vector3(Player2.transform.position.x, -30f, Player2.transform.position.z); ;

        if (Input.GetKey(KeyCode.DownArrow) && Player2.transform.position.y >= -30)
        {
            Player2.transform.position = new Vector3(Player2.transform.position.x, (float)(Player2.transform.position.y - speed), Player2.transform.position.z); ;
        }

        if (Input.GetKey(KeyCode.UpArrow) && Player2.transform.position.y <= 30)
        {
            Player2.transform.position = new Vector3(Player2.transform.position.x, (float)(Player2.transform.position.y + speed), Player2.transform.position.z); ;
        }

        if (Player2.transform.position.y > 30)
            Player2.transform.position = new Vector3(Player2.transform.position.x, 30f, Player2.transform.position.z); ;
        if (Player2.transform.position.y < -30)
            Player2.transform.position = new Vector3(Player2.transform.position.x, -30f, Player2.transform.position.z); ;

        Ball.transform.position = new Vector3(Ball.transform.position.x + direction * speed, Ball.transform.position.y + angle * speed, 50);
        
    }

    public void startRound()
    {
        Ball.transform.position = new Vector3(0, 0, 50f);

        speed = 0.1f;

        int side = Random.Range(0, 2) < 0.5 ? 0 : 1;

        angle = Random.Range(-0.5f, 0.5f);
        
        


        //stanga
        if(side == 0)
        {
            direction *= -1;
        }
    }




}
