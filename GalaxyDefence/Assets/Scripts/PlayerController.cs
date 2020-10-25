using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerFireLevel
{
    One = 0,
    Two,
    Four,
    Six
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] public int PlayerMaxHealth = 100;
    [SerializeField] public GameObject DeathAnimation;
    [SerializeField] Text gameOverText;

    [SerializeField] public GameObject PlayerLaser;
    [SerializeField] public float LaserSpeed = 5f;
    [SerializeField] public float PlayerFireDelay = 0.3f;

    [SerializeField] GameObject ShieldPrefab;
    [SerializeField] float ShieldDuration = 5f;
    private GameObject activeShield = null;

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

    private PlayerFireLevel gunFireLevel = PlayerFireLevel.One;

    private PlayerHealthController playerHealthText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyLaser" || collision.gameObject.tag == "Enemy")
        {
            if (this.playerHealthText != null)
            {
                this.playerHealthText.DecreaseHealth(20);
                this.currentPlayerHealth = this.playerHealthText.currentHealth;
            }

            //need collision sound here

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

        if (this.activeShield != null)
        {
            this.activeShield.transform.position = this.gameObject.transform.position;
        }
    }

    private void CreateLevelTwoLasers(Vector3 playerPos)
    {
        Vector3 shiftfVector = new Vector3(0.3f, 0f, 0f);
        var laser = Instantiate(this.PlayerLaser, playerPos - shiftfVector, this.transform.rotation);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, LaserSpeed);

        laser = Instantiate(this.PlayerLaser, playerPos + shiftfVector, transform.rotation);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, LaserSpeed);
    }

    private void CreateLevelFourLaser(Vector3 playerPos)
    {
        CreateLevelTwoLasers(playerPos);

        var shiftfVector = new Vector3(0.5f, 0f, 0f);
        var laser = Instantiate(this.PlayerLaser, playerPos - shiftfVector, this.transform.rotation);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, LaserSpeed);

        laser = Instantiate(this.PlayerLaser, playerPos + shiftfVector, this.transform.rotation);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, LaserSpeed);
    }

    private void CreateLevelSixLaser(Vector3 playerPos)
    {
        CreateLevelFourLaser(playerPos);

        var shiftfVector = new Vector3(0.55f, 0f, 0f);
        var laser = Instantiate(this.PlayerLaser, playerPos - shiftfVector, this.transform.rotation);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(-LaserSpeed, LaserSpeed);

        laser = Instantiate(this.PlayerLaser, playerPos + shiftfVector, this.transform.rotation);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(LaserSpeed, LaserSpeed);
    }

    private IEnumerator PlayerFire()
    {
        while (true)
        {
            switch (this.gunFireLevel)
            {
                case PlayerFireLevel.One:
                    {
                        var laser = Instantiate(this.PlayerLaser, transform.position, transform.rotation);
                        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, LaserSpeed);
                        break;
                    }
                case PlayerFireLevel.Two:
                    {
                        this.CreateLevelTwoLasers(this.transform.position);
                        break;
                    }
                case PlayerFireLevel.Four:
                    {
                        this.CreateLevelFourLaser(this.transform.position);
                        break;
                    }
                case PlayerFireLevel.Six:
                    {
                        this.CreateLevelSixLaser(this.transform.position);
                        break;
                    }
                default:
                    break;
            }

            if (this.fireSound != null)
            {
                AudioSource.PlayClipAtPoint(this.fireSound.clip, this.transform.position, 0.4f);
            }

            yield return new WaitForSeconds(PlayerFireDelay);
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

        this.playerHealthText = FindObjectOfType<PlayerHealthController>();
        this.currentPlayerHealth = this.PlayerMaxHealth;
        if (this.playerHealthText != null)
        {
            this.playerHealthText.SetHealth(this.PlayerMaxHealth);
        }

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

        SetUpShield();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    public void SetFullHp()
    {
        this.currentPlayerHealth = this.PlayerMaxHealth;
        if (this.playerHealthText != null)
        {
            this.playerHealthText.SetHealth(this.PlayerMaxHealth);
        }
    }

    public void LvlUpGuns()
    {
        if (this.gunFireLevel != PlayerFireLevel.Six)
        {
            this.gunFireLevel++;
        }
    }

    private IEnumerator ActivateShieldCoroutine()
    {
        this.activeShield = Instantiate(this.ShieldPrefab, this.transform.position, this.transform.rotation);

        yield return new WaitForSeconds(this.ShieldDuration);

        Destroy(this.activeShield);
        this.activeShield = null;
    }

    public void SetUpShield()
    {
        if (this.ShieldPrefab != null)
        {
            StartCoroutine(this.ActivateShieldCoroutine());
        }
    }

    public void SpeedUpFire()
    {
        this.LaserSpeed += this.LaserSpeed * 0.5f;
        this.LaserSpeed = Mathf.Clamp(this.LaserSpeed, 1f, 15f);

        this.PlayerFireDelay -= 0.05f;
        this.PlayerFireDelay = Mathf.Clamp(this.PlayerFireDelay, 0.05f, 0.3f);
    }
}
