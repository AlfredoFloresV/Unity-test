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

    public float maxIntensity = 5f;
    public float intensity;
    public bool focus;

    // Start is called before the first frame update
    void Start()
    {
        l = spotlight.GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
        intensity = maxIntensity;
        focus = false;
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

        if (Input.GetKey(KeyCode.Mouse0)) 
        {
            l.spotAngle = 50;
            l.range = 10;
            l.intensity = intensity * 3;
            focus = true;
        }
        else 
        { 
            l.spotAngle = 100;
            l.range = 80;
            l.intensity = intensity;
            focus = false;
        }
    }

    private void updateFlashLight() 
    {
        if (l.enabled && intensity > 0) 
        {
            intensity = intensity - 0.05f;
            l.intensity = intensity;
        }
    }


    public void chargeBattery() 
    {
        intensity = intensity + 0.5f;
        if (intensity > maxIntensity) 
        {
            intensity = maxIntensity;
        }
    }

    public void spendLight(float value) 
    {
        intensity = intensity - value;
        if (intensity < 0) 
        {
            intensity = 0f;
        }
    }

    public bool lightEnabled() 
    {
        return l.enabled;
    }

    IEnumerator WaitForClickBtn()
    {
        yield return new WaitForSeconds(1);
        l.enabled = !l.enabled;
    }

}
