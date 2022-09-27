using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string UserName;
    public GameObject gameOverCanvas;
    public FB_ScoreController ScoreController;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        if (Score.score > ScoreController.maxLocalScore)
        {
            ScoreController.maxLocalScore = Score.score;
            ScoreController.WriteNewScore(Score.score);
        }
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    public void Replay()
    {
        SceneManager.LoadScene(1);
    }
}
