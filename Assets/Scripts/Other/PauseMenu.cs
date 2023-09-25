using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button returnToGameButton;
    public Button quitGameButton;

    private bool isPaused = false;

    private void Start()
    {
        // Initially, hide the pause menu
        pauseMenuUI.SetActive(false);

        // Add listeners to the buttons
        returnToGameButton.onClick.AddListener(ResumeGame);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0; // Pause the game
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    private void QuitGame()
    {
        // Return to the main menu or another scene
        SceneManager.LoadScene("MainMenu2");
    }
}
