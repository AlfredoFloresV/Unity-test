using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    private AudioClip clickBtn;

    [SerializeField]
    private GameObject pauseObj;

    [SerializeField]
    private GameObject freezeObj;

    private Light l;
    private AudioSource audioSource;

    public float maxIntensity = 2f;
    private float minIntensity; 
    public float intensity;
    private float spotAngle;
    private float range;
    public bool focus;

    // Start is called before the first frame update
    void Start()
    {
        l = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
        intensity = maxIntensity;
        spotAngle = l.spotAngle;
        range = l.range;
        minIntensity = maxIntensity * 0.25f;
        focus = false;
        InvokeRepeating("updateFlashLight", 10.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseObj.GetComponent<PauseMenu>().isPaused || freezeObj.GetComponent<ObjectPickupAndRotate>().Freezed)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            audioSource.PlayOneShot(clickBtn);
            StartCoroutine(WaitForClickBtn());
        }

        if (Input.GetKey(KeyCode.Mouse0)) 
        {
            l.spotAngle = spotAngle / 2;
            l.range = range / 2;
            l.intensity = intensity * 3;
            focus = true;
        }
        else 
        { 
            l.spotAngle = spotAngle;
            l.range = range;
            l.intensity = intensity;
            focus = false;
        }
    }

    private void updateFlashLight() 
    {
        if (l.enabled && intensity > minIntensity) 
        {
            intensity = intensity - 0.01f;
            l.intensity = intensity;
        }
    }


    public void chargeBattery() 
    {
        intensity = intensity + 0.4f;
        if (intensity > maxIntensity) 
        {
            intensity = maxIntensity;
        }
    }

    public void spendLight(float value) 
    {
        intensity = intensity - value;
        if (intensity < minIntensity) 
        {
            intensity = minIntensity;
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
