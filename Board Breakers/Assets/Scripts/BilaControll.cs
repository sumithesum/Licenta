using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BilaControll : MonoBehaviour
{
    [SerializeField]public float speed = 5f;
    private Rigidbody rb;

    public float interval = 5f;
    public static PingPongSender sender;
    public static BilaControll inst;
    public Vector3 Dir = new Vector3();
    [SerializeField] public TextMeshProUGUI score1, score2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inst = this;
        
        
        if (PlayerHost.isHost)
        {
            
            StartCoroutine(wait());
            StartCoroutine(RepeatEveryNSeconds());
        }
        
        
    }
    private IEnumerator RepeatEveryNSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

           
            DoSomething();
        }
    }

    private void DoSomething()
    {
        if (PlayerHost.isHost)
            PingPongSender.inst.updateBallPosServer(gameObject.transform.position.x, gameObject.transform.position.y , rb.velocity);

    }
    public IEnumerator wait()
    {
       
        yield return new WaitForSeconds(2f);
        ResetBall(); ;

    }

    public void setPos(float x , float y)
    {
        this.gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.L))
        //{
        //    ResetBall();
        //}
        //if (PlayerHost.isHost)
        //    PingPongSender.inst.updateBallPosServer(gameObject.transform.position.x, gameObject.transform.position.y);


    }

    public void setVelocity(Vector3 startDir)
    {
        
        rb.velocity = startDir * speed;
    }

    public void setVelocity2(Vector3 velocity)
    {

        rb.velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {

        //Vector3 normal = collision.contacts[0].normal;
        //Vector3 reflected = Vector3.Reflect(rb.velocity.normalized, normal);
        //rb.velocity = reflected * speed;


        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
        else
        {
            if (collision.gameObject.name == "Left" || collision.gameObject.name == "Right")
            {
                int scoreP1 = int.Parse(score1.text);
                int scoreP2 = int.Parse(score2.text);

                if (collision.gameObject.name == "Left")
                    score2.text = (scoreP2 + 1).ToString();
                else
                    score1.text = (scoreP1 + 1).ToString();
                ResetBall();
            }
        }
            print("Sa lovit");

    }

    public void ResetBall()
    {
        transform.position = new Vector3 (4,4,-1);
        if (PlayerHost.isHost)
        {
            print("Reseting the ball");
            Vector3 startDir = new Vector3(
            Random.Range(0.5f, 1f) * (Random.value > 0.5f ? 1 : -1), // X
            Random.Range(0.2f, 1f) * (Random.value > 0.5f ? 1 : -1),
            Random.Range(0.2f, 1f) * (Random.value > 0.5f ? 1 : -1)  // Z
            ).normalized;
            Dir = startDir;
            PingPongSender.inst.sendStartPositionServer(startDir);

        }
    }

    
}
