using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DestroyDuplicatePrefabs : MonoBehaviour
{
    public List<GameObject> prefabsToCheck;
    public bool startDestruction = false; // Public boolean to trigger the destruction of duplicate prefabs

    void Update()
    {
        if (startDestruction)
        {
            DestroyDuplicates();
            startDestruction = false; // Reset the boolean
        }
    }

    void DestroyDuplicates()
    {
        foreach (GameObject prefab in prefabsToCheck)
        {
            DestroyDuplicatesForPrefab(prefab);
        }
    }

    void DestroyDuplicatesForPrefab(GameObject prefab)
    {
        // Find all instances of objects in the scene
        GameObject[] instances = GameObject.FindObjectsOfType<GameObject>();

        List<GameObject> matchingInstances = new List<GameObject>();

        // Check if the object name matches the name of the current prefab's top-level GameObject
        foreach (GameObject instance in instances)
        {
            if (instance != null && instance.name == prefab.name && !HasParentWithSameName(instance, prefab.name))
            {
                matchingInstances.Add(instance);
            }
        }

        // If there are multiple instances, keep one random instance and destroy the rest
        if (matchingInstances.Count > 1)
        {
            int randomIndexToKeep = Random.Range(0, matchingInstances.Count);

            for (int i = 0; i < matchingInstances.Count; i++)
            {
                if (i != randomIndexToKeep)
                {
                    Destroy(matchingInstances[i]);
                }
            }

            Debug.Log($"Prefab '{prefab.name}': Found {matchingInstances.Count} instances, kept 1, and destroyed {matchingInstances.Count - 1}.");
        }
        else
        {
            Debug.Log($"Prefab '{prefab.name}': Found {matchingInstances.Count} instance, nothing to destroy.");
        }
    }

    // Check if an object has a parent with the same name
    bool HasParentWithSameName(GameObject obj, string name)
    {
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            if (parent.gameObject.name == name)
            {
                return true;
            }
            parent = parent.parent;
        }
        return false;
    }
}
