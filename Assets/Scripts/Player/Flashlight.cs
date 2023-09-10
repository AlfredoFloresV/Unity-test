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

        InvokeRepeating("updateFlashLight", 10.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            audioSource.PlayOneShot(clickBtn);
            StartCoroutine(WaitForClickBtn());
        }

        
    }

    private void updateFlashLight() 
    {
        if (l.enabled && intensity > 0) 
        {
            intensity = intensity - 0.01f;
            l.intensity = intensity;
        }
    }


    public void chargeBattery() 
    {
        intensity = intensity + 0.5f;
        if (intensity > 1) 
        {
            intensity = 1f;
        }
    }

    IEnumerator WaitForClickBtn()
    {
        yield return new WaitForSeconds(1);
        l.enabled = !l.enabled;
    }

}
