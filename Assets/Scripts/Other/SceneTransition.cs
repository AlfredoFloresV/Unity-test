using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetSceneName = "MainMenu"; 

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
