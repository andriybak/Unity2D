using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStart : MonoBehaviour
{
    [SerializeField] SpriteRenderer paddleObject;
    [SerializeField] List<AudioClip> bounceSounds;
    [SerializeField] bool AutoPlay = false;

    public bool ballLaunched = false;

    private float distanceFromPaddle = 1f;
    private Rigidbody2D ballRigidBody2D = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ballLaunched && ballRigidBody2D != null)
        {
            if (Mathf.Abs(this.ballRigidBody2D.velocity.x) < 0.1f)
            {
                var newVelocity = new Vector2(Random.Range(0.1f, 0.4f), this.ballRigidBody2D.velocity.y);
                this.ballRigidBody2D.velocity = newVelocity;
            }
        }

        if (collision.gameObject.name.Contains("Block") && collision.gameObject.tag == "GameBlock")
        {
            var audioSource = gameObject.GetComponent<AudioSource>();
            if (bounceSounds.Count > 0)
            {
                var audioClipIndex = Random.Range(0, bounceSounds.Count);
                audioSource.clip = bounceSounds[audioClipIndex];
            }

            audioSource.Play();
        }
    }

    public void ResetScale()
    {
        transform.localScale = new Vector3(1f, 1f, 0f);
    }

    public void SetBallToInitialPosition()
    {
        this.ballLaunched = false;
        if (transform.position != paddleObject.transform.position)
        {
            this.transform.position = new Vector3(paddleObject.transform.position.x, paddleObject.transform.position.y + distanceFromPaddle, paddleObject.transform.position.z);
        }
    }

    public void LaunchBall()
    {
        ballLaunched = true;
        this.ballRigidBody2D.velocity = new Vector2(5.0f, 10.0f);
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LaunchBall();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (this.paddleObject == null)
        {
            this.paddleObject = FindObjectOfType<Paddle>().GetComponent<SpriteRenderer>();
        }

       // distanceFromPaddle = Mathf.Abs(paddleObject.transform.position.y - transform.position.y);
        this.ballRigidBody2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ballLaunched)
        {
            LaunchOnMouseClick();
            if (transform.position != paddleObject.transform.position)
            {
                this.transform.position = new Vector3(paddleObject.transform.position.x, paddleObject.transform.position.y + distanceFromPaddle, paddleObject.transform.position.z);
            }
        }
        else if (ballLaunched && AutoPlay && this.paddleObject != null)
        {
            var paddlePosition = this.paddleObject.transform.position;
            paddlePosition.x = this.gameObject.transform.position.x;
            this.paddleObject.transform.position = paddlePosition;
        }
    }
}
