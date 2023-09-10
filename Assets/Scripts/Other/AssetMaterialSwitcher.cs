using UnityEngine;

public class AssetMaterialSwitcher : MonoBehaviour
{
    public Material[] materials; // List of materials to switch between

    private Renderer rend; // Reference to the GameObject's renderer component

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("Renderer component not found on the GameObject.");
            enabled = false; // Disable the script if there's no renderer
            return;
        }

        // Ensure there are materials assigned in the list
        if (materials == null || materials.Length == 0)
        {
            Debug.LogError("No materials assigned in the inspector.");
            enabled = false; // Disable the script if there are no materials
            return;
        }

        // Set a random initial material
        int randomIndex = Random.Range(0, materials.Length);
        ApplyMaterial(randomIndex);
    }

    public void ChangeMaterial(int index)
    {
        // Ensure the index is within bounds
        if (index >= 0 && index < materials.Length)
        {
            ApplyMaterial(index);
        }
    }

    private void ApplyMaterial(int index)
    {
        // Apply the material at the specified index
        rend.material = materials[index];
    }
}
