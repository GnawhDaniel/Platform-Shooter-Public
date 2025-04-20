using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject newGamePanel;
    [SerializeField] GameObject playerUI;
    private bool isPaused = false;

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                isPaused = false;
                Resume();
            }
            else
            {
                isPaused = true;
                PauseGame();
            }
        }

    }

    public void NewGame()
    {
        Resume();
        SceneManager.LoadScene("Main");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void PauseGame()
    {
        playerUI.SetActive(false);
        pausePanel.SetActive(true);
        newGamePanel.SetActive(true);
        Time.timeScale = 0;
    }

    void Resume()
    {
        playerUI.SetActive(true);
        pausePanel.SetActive(false);
        newGamePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
