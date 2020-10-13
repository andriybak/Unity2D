using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
    bool gameOverSoundPlayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.StartsWith("Ball"))
        {
            var audio = this.gameObject.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }

            if (FindObjectsOfType<BallStart>().Length == 1)
            {
                var ballController = collision.gameObject.GetComponent<BallStart>();
                if (ballController != null)
                {
                    ballController.SetBallToInitialPosition();
                    FindObjectOfType<GameStatus>().DecreaseLife();
                }
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
