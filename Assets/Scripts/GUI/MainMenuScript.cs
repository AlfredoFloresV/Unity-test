using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mainScreen;

    [SerializeField]
    private GameObject fade;

    [SerializeField]
    private GameObject music2;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject howTo;

    [SerializeField]
    private GameObject controls;


    private void Start()
    {
        music2.SetActive(true);
        //fade.GetComponent<Animator>().Play("Fade");
        StartCoroutine(inactiveFade());
    }

    IEnumerator inactiveFade() 
    {
        yield return new WaitForSeconds(2);
        mainScreen.SetActive(false);
        fade.SetActive(false);
    }

    public void StartGame() 
    {
        SceneManager.LoadScene("DungeonLevel");
    }

    public void Back() 
    {
        mainMenu.SetActive(true);
        howTo.SetActive(false);
        controls.SetActive(false);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    public void HowToPlay() 
    {
        mainMenu.SetActive(false);
        howTo.SetActive(true);
    }

    public void Controls() 
    {
        mainMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void Credits() 
    {
        SceneManager.LoadScene("Credits");
    }
}
