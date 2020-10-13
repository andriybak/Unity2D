using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] int numberOfBlocksInGame = 0;
   // [SerializeField] List<GameObject> levelPrefabs;
  //  [SerializeField] List<Sprite> levelBackgrounds;
    [SerializeField] List<LevelDesign> LevelContent;

    public int CurrentLevel { get; private set; } = 0;
    public int MaxLevel { get; private set; } = 4;

    private SpriteRenderer backroundController = null;
    private GameObject gamePaddle = null;

    public void AddBlockToGame()
    {
        numberOfBlocksInGame++;
    }

    public void RemoveBlockFromLevel()
    {
        numberOfBlocksInGame--;
        if (numberOfBlocksInGame == 0)
        {
            this.CurrentLevel++;
            if (this.CurrentLevel == this.MaxLevel)
            {
                SceneManager.LoadScene("Win Screen");
            }
            else
            {
                SetNextLevel(this.CurrentLevel, this.LevelContent, backroundController, this.gamePaddle);
            }
        }
    }

    private static PolygonCollider2D RecalculatePaddleCollider(PolygonCollider2D polygonCollider, Sprite sprite)
    {
        var newCollider = polygonCollider;


        polygonCollider.pathCount = sprite.GetPhysicsShapeCount();

        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            path.Clear();
            sprite.GetPhysicsShape(i, path);
            newCollider.SetPath(i, path.ToArray());
        }

        return newCollider;
    }

    private static void SetNextLevel(
        int currentLevel,
        List<LevelDesign> levelDesigns,
        SpriteRenderer background,
        GameObject gamePaddle)
    {
        if (currentLevel < levelDesigns.Count)
        {
            var currentLevelDesign = levelDesigns[currentLevel];
            Instantiate(levelDesigns[currentLevel].levelPrefab, Vector3.zero, Quaternion.identity);
            background.sprite = levelDesigns[currentLevel].levelBackground;

            var paddleSpriteRender = gamePaddle.GetComponent<SpriteRenderer>();
            var paddleCollider = gamePaddle.GetComponent<PolygonCollider2D>();
            if (paddleSpriteRender != null &&
                paddleSpriteRender.name != currentLevelDesign.paddleSprite.name)
            {
                paddleSpriteRender.sprite = currentLevelDesign.paddleSprite;
                paddleCollider = RecalculatePaddleCollider(paddleCollider, currentLevelDesign.paddleSprite);
            }
        }

        var ballsInGame = FindObjectsOfType<BallStart>();
        foreach (var ball in ballsInGame)
        {
            ball.SetBallToInitialPosition();
        }
    }

    private void Start()
    {
        this.backroundController = GetComponent<SpriteRenderer>();
        this.gamePaddle = FindObjectOfType<Paddle>().gameObject;
        this.MaxLevel = this.LevelContent.Count;
        this.CurrentLevel = 0;
        SetNextLevel(this.CurrentLevel, this.LevelContent, this.backroundController, this.gamePaddle);
    }
}
