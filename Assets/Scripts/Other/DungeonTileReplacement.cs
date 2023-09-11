using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class DungeonTileReplacement : MonoBehaviour
{
    public List<GameObject> obligatoryPrefabs; // List of prefabs that must spawn
    public List<GameObject> optionalPrefabs; // List of prefabs for random replacement

    public float minDistanceBetweenPrefabs = 5f; // Minimum distance between spawned prefabs

    public float replacementSpawnProbability = 0.5f; // Probability of spawning optional prefabs

    public bool startReplacement = false; // Public boolean to trigger the replacement

    void Update()
    {
        if (startReplacement)
        {
            ReplaceTiles();
            startReplacement = false; // Reset the boolean
        }
    }

    void ReplaceTiles()
    {
        // Find all game objects with the "ReplaceMe" tag in the scene
        GameObject[] replaceMeTiles = GameObject.FindGameObjectsWithTag("ReplaceMe");

        int requiredPrefabCount = 0;
        Dictionary<string, int> replacementPrefabCounts = new Dictionary<string, int>();

        // Create lists to store objects that have been replaced by obligatoryPrefabs and optionalPrefabs
        List<GameObject> objectsReplacedByObligatoryPrefabs = new List<GameObject>();
        List<GameObject> objectsReplacedByOptionalPrefabs = new List<GameObject>();

        foreach (GameObject replaceMeTile in replaceMeTiles)
        {
            Transform tileTransform = replaceMeTile.transform;

            // Check if there's a requiredPrefab to spawn
            if (requiredPrefabCount < obligatoryPrefabs.Count)
            {
                // Check the minimum distance from all spawned optional prefabs
                bool validPosition = true;
                foreach (GameObject prefab in optionalPrefabs)
                {
                    if (Vector3.Distance(tileTransform.position, prefab.transform.position) < minDistanceBetweenPrefabs)
                    {
                        validPosition = false;
                        break;
                    }
                }

                // If valid position, spawn an obligatoryPrefab
                if (validPosition)
                {
                    GameObject obligatoryPrefab = obligatoryPrefabs[requiredPrefabCount];
                    Instantiate(obligatoryPrefab, tileTransform.position, tileTransform.rotation);
                    requiredPrefabCount++;

                    // Add this object to the list of replaced objects by obligatoryPrefabs
                    objectsReplacedByObligatoryPrefabs.Add(replaceMeTile);
                }
            }

            // Check for optional prefabs (if any)
            if (Random.value < replacementSpawnProbability)
            {
                int randomIndex = Random.Range(0, optionalPrefabs.Count);
                GameObject optionalPrefab = optionalPrefabs[randomIndex];

                // Check the minimum distance from all spawned obligatory prefabs
                bool validPosition = true;
                foreach (GameObject prefab in obligatoryPrefabs)
                {
                    if (Vector3.Distance(tileTransform.position, prefab.transform.position) < minDistanceBetweenPrefabs)
                    {
                        validPosition = false;
                        break;
                    }
                }

                // If valid position, spawn an optionalPrefab
                if (validPosition)
                {
                    Instantiate(optionalPrefab, tileTransform.position, tileTransform.rotation);

                    // Add this object to the list of replaced objects by optionalPrefabs
                    objectsReplacedByOptionalPrefabs.Add(replaceMeTile);
                }
            }
        }

        // Remove objects replaced by obligatoryPrefabs and optionalPrefabs from the list
        foreach (var replacedObject in objectsReplacedByObligatoryPrefabs)
        {
            GameObject.Destroy(replacedObject);
        }

        foreach (var replacedObject in objectsReplacedByOptionalPrefabs)
        {
            GameObject.Destroy(replacedObject);
        }

        Debug.Log("Obligatory Prefabs Count: " + requiredPrefabCount);

        // Print replacement prefab counts by name
        foreach (var kvp in replacementPrefabCounts)
        {
            Debug.Log("Optional Prefab: " + kvp.Key + ", Count: " + kvp.Value);
        }
    }
}
