using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ball : MonoBehaviour {
    public float speed = 30f;
    bool inMovement;
    bool lastGoal;
    int leftPoints = 0;
    int rightPoints = 0;
    public Text Player1Points;
    public Text Player2Points;

	// Use this for initialization
	void Start () {
        StartBall();
    }

    void StartBall()
    {
        GetComponent<Rigidbody2D>().transform.position = new Vector2(0f, 0f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        inMovement = false;
        speed = 30f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Hit the left racket?
        if (col.gameObject.name == "RacketLeft")
        {
            //Calculate hit factor
            float y = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1, y).normalized;

            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }

        if (col.gameObject.name == "RacketRight")
        {
            // Calculate hit Factor
            float y = hitFactor(transform.position,
                                col.transform.position,
                                col.collider.bounds.size.y);

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1, y).normalized;

            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }

        if (col.gameObject.name == "WallLeft")
        {
            StartBall();
            lastGoal = false;
            rightPoints++;
            Player2Points.text = "Player 2: " + rightPoints;
        }

        if (col.gameObject.name == "WallRight")
        {
            StartBall();
            lastGoal = true;
            leftPoints++;
            Player1Points.text = "Player 1: " + leftPoints;
        }
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        // ascii art:
        // ||  1 <- at the top of the racket
        // ||
        // ||  0 <- at the middle of the racket
        // ||
        // || -1 <- at the bottom of the racket
        return (ballPos.y - racketPos.y) / racketHeight;
    }

    void Update()
    {
        if (!inMovement && Input.anyKeyDown)
        {
            if (lastGoal)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
            }
            
            inMovement = true;
        }

        if (inMovement && speed < 45)
        {
            speed += 0.01f;
        }        
    }
}
