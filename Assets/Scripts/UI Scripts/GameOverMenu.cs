using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    bool gameIsPaused = false;

    [SerializeField]
    GameObject gameOverMenuUI;
    [SerializeField]
    PlayerController player = null;
    [SerializeField]
    LevelCreator levelCreator = null;
    [SerializeField]
    EndLevelPoint endLevelPoint = null;
    public void FixedUpdate()
    {
        if(player.CurrentHealth == 0 || endLevelPoint.LevelComplete && !gameIsPaused)
        {
            Pause();
        }
    }

    public void NewGame()
    {
        Resume();
        endLevelPoint.LevelComplete = false;
        SceneManager.LoadScene(1);
    }

    public void RestartGame()
    {
        Resume();
        endLevelPoint.LevelComplete = false;
        levelCreator.RestartLevel();
    }

    public void Pause()
    {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    private void Resume()
    {
        gameOverMenuUI.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
