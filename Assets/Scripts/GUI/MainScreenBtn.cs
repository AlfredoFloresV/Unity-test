using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject fade;

    [SerializeField]
    private GameObject mainmenu;

    [SerializeField]
    private RawImage image;

    [SerializeField]
    private AudioClip startBtnSound;

    [SerializeField]
    private GameObject music;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("blinkImage", 3f, 0.5f);
    }

    private void blinkImage() 
    {
        image.gameObject.SetActive( ! image.gameObject.activeSelf );
    }

    public void startGame() 
    {
        CancelInvoke("blinkImage");
        image.gameObject.SetActive(true);
        audioSource.PlayOneShot(startBtnSound);
        fade.SetActive(true);
        fade.GetComponent<Animator>().Play("FadeOut");
        StartCoroutine(activeMainMenu());
    }

    IEnumerator activeMainMenu() 
    {
        yield return new WaitForSeconds(4f);
        fade.GetComponent<Animator>().Play("Fade");
        mainmenu.SetActive(true);
        music.SetActive(false);
    }
}
