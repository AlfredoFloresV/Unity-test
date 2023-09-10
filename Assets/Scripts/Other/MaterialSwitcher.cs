using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSwitcher : MonoBehaviour
{
    public Image guiImage; // Reference to the GUI Image component
    public Material material1; // Reference to the first material
    public Material material2; // Reference to the second material
    public float switchInterval = 2.0f; // Interval in seconds between material switches

    private bool isMaterial1 = true; // Flag to track the current material
    private float timer = 0.0f;

    private void Start()
    {
        // Initialize the GUI Image with material1
        guiImage.material = material1;
    }

    private void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to switch materials
        if (timer >= switchInterval)
        {
            // Toggle between material1 and material2
            if (isMaterial1)
            {
                guiImage.material = material2;
            }
            else
            {
                guiImage.material = material1;
            }

            // Reset the timer
            timer = 0.0f;

            // Toggle the flag
            isMaterial1 = !isMaterial1;
        }
    }
}
