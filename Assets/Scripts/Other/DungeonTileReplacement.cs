using System.Collections.Generic;
using UnityEngine;

public class DungeonTileReplacement : MonoBehaviour
{
    public List<GameObject> obligatoryPrefabs; // List of prefabs that must spawn
    public List<GameObject> optionalPrefabs; // List of prefabs for random replacement

    public float minDistanceBetweenPrefabs = 5f; // Minimum distance between spawned prefabs
    public float minDistanceToDoor = 2f; // Minimum distance to objects with "door" tag

    public float replacementSpawnProbability = 0.5f; // Probability of spawning replacement prefabs

    public bool startReplacement = false; // Public boolean to trigger the replacement

    private List<Vector3> spawnedPrefabPositions = new List<Vector3>();

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
        GameObject[] doorObjects = GameObject.FindGameObjectsWithTag("door"); // Find all objects with "door" tag

        Dictionary<string, int> replacementPrefabCounts = new Dictionary<string, int>();
        int obligatoryPrefabCount = 0;

        foreach (GameObject replaceMeTile in replaceMeTiles)
        {
            Transform tileTransform = replaceMeTile.transform;

            // Check if the position is valid
            bool validPosition = true;

            // Check the minimum distance to door objects
            foreach (GameObject doorObject in doorObjects)
            {
                if (Vector3.Distance(tileTransform.position, doorObject.transform.position) < minDistanceToDoor)
                {
                    validPosition = false;
                    break;
                }
            }

            // Check the minimum distance between spawned obligatory prefabs
            foreach (Vector3 spawnedPosition in spawnedPrefabPositions)
            {
                if (Vector3.Distance(tileTransform.position, spawnedPosition) < minDistanceBetweenPrefabs)
                {
                    validPosition = false;
                    break;
                }
            }

            // If valid position, spawn an obligatory prefab (only if remaining)
            if (validPosition && obligatoryPrefabCount < obligatoryPrefabs.Count)
            {
                GameObject obligatoryPrefab = obligatoryPrefabs[obligatoryPrefabCount];
                Instantiate(obligatoryPrefab, tileTransform.position, tileTransform.rotation);
                obligatoryPrefabCount++;
                spawnedPrefabPositions.Add(tileTransform.position); // Track the position of spawned obligatory prefab
            }
            // If all obligatory prefabs are spawned, consider spawning optional prefabs
            else if (validPosition)
            {
                GameObject prefabToSpawn = optionalPrefabs[Random.Range(0, optionalPrefabs.Count)];
                Instantiate(prefabToSpawn, tileTransform.position, tileTransform.rotation);

                // Count replacement prefabs by name
                string prefabName = prefabToSpawn.name;
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

        // Print replacement prefab counts by name
        foreach (var kvp in replacementPrefabCounts)
        {
            Debug.Log("Prefab: " + kvp.Key + ", Count: " + kvp.Value);
        }
    }
}
