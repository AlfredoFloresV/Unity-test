using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    [SerializeField]
    private GameObject logo;

    [SerializeField]
    private GameObject warning;

    [SerializeField]
    private GameObject mainScreen;

    [SerializeField]
    private GameObject Fade;

    private void Start()
    {
        StartCoroutine(closeLogo());   
    }

    IEnumerator closeLogo() 
    {
        yield return new WaitForSeconds(2f);
        Fade.GetComponent<Animator>().Play("FadeOut");
        StartCoroutine(loadWarning());
    }

    IEnumerator loadWarning() 
    {
        yield return new WaitForSeconds(2f);
        logo.SetActive(false);
        warning.SetActive(true);
        Fade.GetComponent<Animator>().Play("Fade");
        StartCoroutine(closeWarning());
    }

    IEnumerator closeWarning() 
    {
        yield return new WaitForSeconds(4f);
        Fade.GetComponent<Animator>().Play("FadeOut");
        StartCoroutine(loadMainScreen());
    }

    IEnumerator loadMainScreen() 
    {
        yield return new WaitForSeconds(2f);
        warning.SetActive(false);
        mainScreen.SetActive(true);
        Fade.GetComponent<Animator>().Play("Fade");
        StartCoroutine(inactiveFade());
    }

    IEnumerator inactiveFade() 
    {
        yield return new WaitForSeconds(2);
        Fade.SetActive(false);
    }
}
