using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] float GameOverDelay = 1f;

    private IEnumerator LoadGameOver()
    {
        Cursor.visible = true;
        
        yield return new WaitForSeconds(GameOverDelay);

        SceneManager.LoadScene("EndGame");
    }

    public void SetGameOver()
    {
        StartCoroutine(LoadGameOver());
    }

    public void StartMainGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        if (FindObjectsOfType(this.GetType()).Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
