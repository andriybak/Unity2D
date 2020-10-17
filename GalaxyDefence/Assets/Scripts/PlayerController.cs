using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject PlayerLaser;
    [SerializeField] public float LaserSpeed = 5f;
    [SerializeField] public int PlayerMaxHealth = 100;
    [SerializeField] public GameObject DeathAnimation;
    [SerializeField] Text gameOverText;

    private float screenRatioWidth = 9f;
    private float screenRatioHeight = 16f;

    private SpriteRenderer playerSprite = null;
    private AudioSource fireSound = null;

    private float spriteWidthOffset = 0f;
    private float spriteHeightOffset = 0f;

    private float minX = 0f;
    private float minY = 0f;
    private float maxX = 0f;
    private float maxY = 0f;

    private Coroutine fireRoutine = null;

    private int currentPlayerHealth = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyLaser" || collision.gameObject.tag == "Enemy")
        {
            //need collision sound here
            this.currentPlayerHealth -= 20;
            Destroy(collision.gameObject);
            this.CheckHealth();
        }
    }

    private void InitializeBoundaries()
    {
        var minCorner = Camera.main.ViewportToWorldPoint(Vector3.zero);
        this.minX = minCorner.x + this.spriteWidthOffset;
        this.minY = minCorner.y + this.spriteHeightOffset;

        var maxCorner = Camera.main.ViewportToWorldPoint(Vector3.one);
        this.maxX = maxCorner.x - this.spriteWidthOffset;
        this.maxY = maxCorner.y - this.spriteHeightOffset;
    }

    private void Move()
    {
        var mousePos = Input.mousePosition;
        var posX = mousePos.x / Screen.width * screenRatioWidth;
        var posY = mousePos.y / Screen.height * screenRatioHeight;

        posX = Mathf.Clamp(posX, this.minX, this.maxX);
        posY = Mathf.Clamp(posY, this.minY, this.maxY);

        this.gameObject.transform.position = new Vector3(posX, posY, 0f);
    }

    private IEnumerator PlayerFire()
    {
        while (true)
        {
            var laser = Instantiate(this.PlayerLaser, transform.position, transform.rotation);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, LaserSpeed);
            if (this.fireSound != null)
            {
                AudioSource.PlayClipAtPoint(this.fireSound.clip, this.transform.position, 0.4f);
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.fireRoutine = StartCoroutine(PlayerFire());
        }
        else if (Input.GetMouseButtonUp(0) && this.fireRoutine != null)
        {
            StopCoroutine(this.fireRoutine);
            this.fireRoutine = null;
        }
    }

    private void CheckHealth()
    {
        if (this.currentPlayerHealth <= 0)
        {
            Debug.Log("Game Over!");
            this.gameOverText.gameObject.SetActive(true);

            Destroy(this.gameObject);

            if (this.DeathAnimation != null)
            {
                Instantiate(this.DeathAnimation, this.transform.position, this.transform.rotation);
            }

            FindObjectOfType<LevelController>().SetGameOver();
        }
    }

    void Start()
    {
        //Hide mouse cursor
        Cursor.visible = false;

        if (this.gameOverText)
        {
            this.gameOverText.gameObject.SetActive(false);
        }

        this.PlayerMaxHealth = 100;
        this.currentPlayerHealth = this.PlayerMaxHealth;

        this.playerSprite = this.GetComponent<SpriteRenderer>();
        var spriteRect = this.playerSprite.sprite.rect;
        var sizeRatio = this.playerSprite.sprite.pixelsPerUnit / 100f;
        float halfWidth = spriteRect.width * sizeRatio * 0.5f;
        float halfHeight = spriteRect.height * sizeRatio * 0.5f;

        this.spriteWidthOffset = halfWidth / Screen.width * this.screenRatioWidth;
        this.spriteHeightOffset = halfHeight / Screen.height * this.screenRatioHeight;

        this.gameObject.transform.position = new Vector3(4f, 0.5f);

        InitializeBoundaries();

        this.fireSound = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }
}
