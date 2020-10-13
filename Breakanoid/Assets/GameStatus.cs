using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameStatus : MonoBehaviour
{
    [Range(1, 10)][SerializeField] float initialGameSpeed = 1.0f;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameTimeText;
    [SerializeField] TextMeshProUGUI playerLivesText;
    [SerializeField] int playerLives = 3;

    float timeFromAcceleration = 0;
    float acceleration = 0.1f;
    float accelerationDelay = 30.0f;

    [SerializeField] int pointsPerBlock = 50;
    [SerializeField] int playerScore = 0;

    public void AddScore()
    {
        playerScore += pointsPerBlock;
        scoreText.text = playerScore.ToString();
    }

    private void ResetGame()
    {
        this.ResetGameSpeed();
        this.playerLives = 3;
        this.playerScore = 0;
        this.timeFromAcceleration = 0f;
        var videoPlayers = FindObjectsOfType<VideoPlayer>();
        foreach (var player in videoPlayers)
        {
            player.Stop();
        }
    }

    public void DecreaseLife()
    {
        this.playerLives--;
        playerLivesText.text = $"Lives: {playerLives}";
        if (this.playerLives == 0)
        {
            this.ResetGame();
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
    }

    public void ResetGameSpeed()
    {
        this.initialGameSpeed = 1.0f;
        Time.timeScale = this.initialGameSpeed;
    }

    private void Awake()
    {
        if (FindObjectsOfType<GameStatus>().Length > 1)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = initialGameSpeed;
        scoreText.text = playerScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        this.gameTimeText.text = $"Game time: {(int)Time.time}";
    }
}
