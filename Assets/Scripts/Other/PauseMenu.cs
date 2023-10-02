using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button returnToGameButton;
    public Button quitGameButton;

    public bool isPaused = false;

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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
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
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
        pauseMenuUI.SetActive(false);
        isPaused = false;
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu2");
    }
}
