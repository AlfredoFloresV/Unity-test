using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    private Light spotlight;


    [SerializeField]
    private Transform flEnabled;
    [SerializeField]
    private Transform flDisabled;
    [SerializeField]
    private AudioClip clickBtn;

    private Light l;
    private AudioSource audioSource;


    public float intensity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        l = spotlight.GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            audioSource.PlayOneShot(clickBtn);
            StartCoroutine(WaitForClickBtn());
        }

        //if (l.enabled) 
        //{
        //    StartCoroutine(updateFlashLight());
        //}
    }

    IEnumerator updateFlashLight() 
    {
        yield return new WaitForSeconds(10f);
        intensity = intensity - 0.1f;
        l.intensity = intensity;
    }


    IEnumerator WaitForClickBtn()
    {
        yield return new WaitForSeconds(1);
        l.enabled = !l.enabled;
    }

}
