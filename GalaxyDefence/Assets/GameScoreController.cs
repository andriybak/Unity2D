using TMPro;
using UnityEngine;

public class GameScoreController : MonoBehaviour
{
    private TextMeshProUGUI UIScoreText;

    public int currentGameScore = 0;

    public void AddScore(int scoreToAdd)
    {
        this.currentGameScore += scoreToAdd;
        if (this.UIScoreText != null)
        {
            this.UIScoreText.text = $"Score {this.currentGameScore}";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.currentGameScore = 0;
        this.UIScoreText = this.gameObject.GetComponent<TextMeshProUGUI>();
    }
}
