using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mainScreen;

    [SerializeField]
    private GameObject fade;

    [SerializeField]
    private GameObject music2;

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
}
