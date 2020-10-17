using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject EnemyLaser = null;
    [SerializeField] float LaserSpeed = 7f;
    [SerializeField] int NumberOfHitsToKill = 1;
    [SerializeField] float FireDelay = 0.5f;
    [SerializeField] float FireDelayRandomize = 0.3f;
    [SerializeField] float MovementSpeedPerFrame = 4f;
    [SerializeField] GameObject ExplosionAnimation;

    public List<Vector2> EnemyPath;

    private int numberOfCollisions = 0;
    private int nextPathIndex = 0;
    private AudioSource enemyDeathSound = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerLaser")
        {
            this.numberOfCollisions++;
            if (this.numberOfCollisions == this.NumberOfHitsToKill)
            {
                if (this.enemyDeathSound != null)
                {
                    AudioSource.PlayClipAtPoint(this.enemyDeathSound.clip, Vector2.zero);
                }

                if (this.ExplosionAnimation != null)
                {
                    var animation = Instantiate(this.ExplosionAnimation, this.transform.position, this.transform.rotation);
                    Destroy(animation, 0.3f);
                }

                Destroy(this.gameObject);
            }

            Destroy(collision.gameObject);
        }
    }

    private void Move()
    {
        if (this.EnemyPath != null &&
            this.EnemyPath.Count > 0 &&
            this.nextPathIndex < this.EnemyPath.Count)
        {
            var targetPosition = this.EnemyPath[this.nextPathIndex];
            if (Vector2.Distance(this.transform.position, targetPosition) < 0.001f)
            {
                this.nextPathIndex++;
                if (this.nextPathIndex == this.EnemyPath.Count)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    targetPosition = this.EnemyPath[this.nextPathIndex];
                }
            }

            this.transform.position = Vector3.MoveTowards(this.transform.position,
                targetPosition, Time.deltaTime * MovementSpeedPerFrame);
        }
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            if (this.EnemyLaser != null && this.gameObject.activeSelf)
            {
                var laser = Instantiate(this.EnemyLaser, this.transform.position, this.EnemyLaser.transform.rotation);
                if (laser != null)
                {
                    var rigidBody = laser.GetComponent<Rigidbody2D>();
                    if (rigidBody != null)
                    {
                        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, LaserSpeed);
                    }
                }
            }

            var fireDelay = Random.Range(this.FireDelay, this.FireDelay + this.FireDelayRandomize);

            yield return new WaitForSeconds(fireDelay);
        }

    }

    public void SetPath(List<Vector2> path)
    {
        this.EnemyPath = path;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.numberOfCollisions = 0;
        if (this.EnemyPath.Count > 0)
        {
            this.transform.position = this.EnemyPath.First();
        }

        this.enemyDeathSound = this.GetComponent<AudioSource>();

        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
