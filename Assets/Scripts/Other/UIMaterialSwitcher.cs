using UnityEngine;
using UnityEngine.UI;

public class UIMaterialSwitcher : MonoBehaviour
{
    public int damageScreen = 0; // Public value to control the material index (0-5)

    public Material[] materials; // Array of materials to switch between
    public Image guiImage; // Reference to the GUI Image component

    private void Start()
    {
        // Ensure the initial material is set correctly
        SetMaterial(damageScreen);
    }

    private void Update()
    {
        // Check if the material index has changed
        if (guiImage.material != materials[damageScreen])
        {
            SetMaterial(damageScreen);
        }
    }

    // Function to set the material based on the index
    private void SetMaterial(int index)
    {
        if (index >= 0 && index < materials.Length)
        {
            guiImage.material = materials[index];

            // Hide the image if the index is 0 (invisible)
            guiImage.enabled = (index != 0);
        }
    }
}
