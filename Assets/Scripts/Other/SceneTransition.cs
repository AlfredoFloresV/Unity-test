using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetSceneName = "MainMenu";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
