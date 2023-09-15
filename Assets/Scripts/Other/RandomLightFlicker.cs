using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLightFlicker : MonoBehaviour
{
    public Material lightMaterial;
    public float minFlickerInterval = 0.5f;
    public float maxFlickerInterval = 3.0f;

    private float nextFlickerTime;
    private bool lightOn = true;
    //public Light spotLight;

    void Start()
    {
        // Start with the light on
        lightMaterial.SetColor("_EmissionColor", Color.yellow);
        lightMaterial.SetColor("_Color", Color.white);

        // Schedule the first flicker
        nextFlickerTime = Time.time + Random.Range(minFlickerInterval, maxFlickerInterval);
    }

    void Update()
    {
        if (Time.time >= nextFlickerTime)
        {
            // Toggle the light state
            lightOn = !lightOn;

            // Apply changes to the material
            if (lightOn)
            {
                lightMaterial.SetColor("_EmissionColor", Color.yellow);
                lightMaterial.SetColor("_Color", Color.white);
            }
            else
            {
                lightMaterial.SetColor("_EmissionColor", Color.gray);
                lightMaterial.SetColor("_Color", Color.gray);
            }

            // Schedule the next flicker
            nextFlickerTime = Time.time + Random.Range(minFlickerInterval, maxFlickerInterval);
        }
    }

    /*void Update()
    {
        // Handle light state change in LateUpdate for smoother rendering
        spotLight.enabled = lightOn;
    }*/
}
