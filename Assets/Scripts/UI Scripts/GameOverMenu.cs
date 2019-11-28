using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    bool gameIsPaused = false;
    public GameObject gameOverMenuUI;
    public PlayerController player;
    public void NewGame()
    {
        gameOverMenuUI.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1f;        
        SceneManager.LoadScene(1);        
    }
    
    public void FixedUpdate()
    {
        if(player.CurrentHealth == 0 && !gameIsPaused)
        {
            Pause();
        }
    }

    public void Pause()
    {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
