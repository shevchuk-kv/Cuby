using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    bool gameIsPaused = false;
    [SerializeField]
    GameObject pauseMenuUI = null;
    [SerializeField]
    PlayerController player = null;
    [SerializeField]
    LevelCreator levelCreator = null;
    [SerializeField]
    EndLevelPoint endLevelPoint = null;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && player.CurrentHealth != 0 && !endLevelPoint.LevelComplete)
        {
            if(gameIsPaused)
            {
                Resume();
            } else
            {
                Paused();
            }
        }
    }

    private void Paused()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void ResumeGame()
    {
        Resume();
    }

    public void RestartGame()
    {
        Resume();
        levelCreator.RestartLevel();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
        Resume();
    }
}
