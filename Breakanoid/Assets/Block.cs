using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] public List<Sprite> blockSprites;
    [SerializeField] GameObject breakEffect;
    [SerializeField] GameObject blockBonus;

    private int numberOfHits = 0;
    private int numberOfHitsToDestroy = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Ball") && this.gameObject.tag == "GameBlock")
        {
            numberOfHits++;
            var blockSpriteRenderer = GetComponent<SpriteRenderer>();
            if (blockSpriteRenderer != null && blockSprites.Count > 0 && numberOfHits < numberOfHitsToDestroy)
            {
                blockSpriteRenderer.sprite = blockSprites[numberOfHits];
            }

            if (numberOfHitsToDestroy == numberOfHits)
            {
                if (breakEffect != null)
                {
                    var breakObject = Instantiate(breakEffect, gameObject.transform.position, gameObject.transform.rotation);
                    Destroy(breakObject, 0.1f);
                }

                if (blockBonus != null)
                {
                    Instantiate(blockBonus, transform.position, transform.rotation);
                }

                FindObjectOfType<GameStatus>().AddScore();
                FindObjectOfType<Level>().RemoveBlockFromLevel();
                Object.Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        FindObjectOfType<Level>().AddBlockToGame();
        this.numberOfHitsToDestroy = Random.Range(1, blockSprites.Count + 1);
    }
}
