using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonTileReplacement : MonoBehaviour
{
    public List<GameObject> obligatoryPrefabs; // List of prefabs that must spawn
    public List<GameObject> optionalPrefabs; // List of prefabs for random replacement

    public float minDistanceBetweenObligatory = 5f; // Minimum distance between spawned obligatory prefabs

    public float replacementSpawnProbability = 0.5f; // Probability of spawning replacement prefabs

    public bool startReplacement = false; // Public boolean to trigger the replacement

    private List<Vector3> obligatorySpawnPositions = new List<Vector3>();

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

        int obligatoryPrefabCount = 0;
        Dictionary<string, int> replacementPrefabCounts = new Dictionary<string, int>();

        foreach (GameObject replaceMeTile in replaceMeTiles)
        {
            Transform tileTransform = replaceMeTile.transform;

            // Check if there's an obligatoryPrefab to spawn
            if (obligatoryPrefabCount < obligatoryPrefabs.Count)
            {
                // Check if the position is valid for an obligatoryPrefab
                bool validPosition = true;
                foreach (Vector3 spawnPosition in obligatorySpawnPositions)
                {
                    if (Vector3.Distance(tileTransform.position, spawnPosition) < minDistanceBetweenObligatory)
                    {
                        validPosition = false;
                        break;
                    }
                }

                // If valid position, spawn an obligatoryPrefab
                if (validPosition)
                {
                    GameObject obligatoryPrefab = obligatoryPrefabs[obligatoryPrefabCount];
                    Instantiate(obligatoryPrefab, tileTransform.position, tileTransform.rotation);
                    obligatorySpawnPositions.Add(tileTransform.position);
                    obligatoryPrefabCount++;
                }
            }

            // Check for replacement prefabs (if any)
            if (Random.value < replacementSpawnProbability)
            {
                int randomIndex = Random.Range(0, optionalPrefabs.Count);
                GameObject replacementPrefab = optionalPrefabs[randomIndex];

                // Instantiate the replacement prefab at the same position and rotation
                Instantiate(replacementPrefab, tileTransform.position, tileTransform.rotation);

                // Count replacement prefabs by name
                string prefabName = replacementPrefab.name;
                if (replacementPrefabCounts.ContainsKey(prefabName))
                {
                    replacementPrefabCounts[prefabName]++;
                }
                else
                {
                    replacementPrefabCounts[prefabName] = 1;
                }
            }

            // Destroy the game object with the "ReplaceMe" tag
            Destroy(replaceMeTile);
        }

        Debug.Log("Obligatory Prefabs Count: " + obligatoryPrefabCount);

        // Print replacement prefab counts by name
        foreach (var kvp in replacementPrefabCounts)
        {
            Debug.Log("Replacement Prefab: " + kvp.Key + ", Count: " + kvp.Value);
        }
    }
}
