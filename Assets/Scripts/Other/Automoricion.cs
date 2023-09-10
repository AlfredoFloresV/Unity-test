using UnityEngine;

public class Automoricion : MonoBehaviour
{
    public float destroyPercentage = 50.0f; // Percentage chance of destroying the GameObject (0-100)

    private void Start()
    {
        // Generate a random number between 0 and 100
        float randomValue = Random.Range(0.0f, 100.0f);

        // Check if the random value is less than the destroy percentage
        if (randomValue < destroyPercentage)
        {
            // Destroy the GameObject
            Destroy(gameObject);
        }
    }
}
