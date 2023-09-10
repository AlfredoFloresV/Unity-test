using UnityEngine;

public class ObjectPickupAndRotate : MonoBehaviour
{
    public GameObject object1Prefab;
    public GameObject object2Prefab;
    public GameObject object3Prefab;
    public GameObject object4Prefab;

    public bool key1 = false;
    public bool key2 = false;
    public bool key3 = false;
    public bool key4 = false;

    private bool isPaused = false;
    private float originalTimeScale;
    private GameObject spawnedObject;

    
    private string objectIntMessage = "";
    private string objectNameMessage = "";
    private string interactionMessage = "";

    private void Start()
    {
        originalTimeScale = Time.timeScale;
        objectIntMessage = "";
        objectNameMessage = "";
        interactionMessage = "";
    }

    private void Update()
    {
        if (key1 && !isPaused)
        {
            PauseScene();
            SpawnObject(object1Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "YELLOW KEY";
            interactionMessage = "Press Q to return";
        }
        if (key2 && !isPaused)
        {
            PauseScene();
            SpawnObject(object2Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "PINK KEY";
            interactionMessage = "Press Q to return";
        }
        if (key3 && !isPaused)
        {
            PauseScene();
            SpawnObject(object3Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "GREEN KEY";
            interactionMessage = "Press Q to return";
        }
        if (key4 && !isPaused)
        {
            PauseScene();
            SpawnObject(object4Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "ORANGE KEY";
            interactionMessage = "Press Q to return";
        }
        if (Input.GetKeyDown(KeyCode.Q) && isPaused)
        {
            DestroyObject();
            ResumeScene();
            key1 = false;
            key2 = false;
            key3 = false;
            key4 = false;
            objectIntMessage = "";
            objectNameMessage = "";
            interactionMessage = "";
        }

        // Allow rotation even when the scene is paused
        RotateObject();
    }

    private void PauseScene()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the scene
    }

    private void ResumeScene()
    {
        isPaused = false;
        Time.timeScale = originalTimeScale; // Restore the original time scale
    }

    private void SpawnObject(GameObject objPrefab, Vector3 scale)
    {
        if (spawnedObject == null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * 0.5f; // Adjust the distance as needed
            spawnedObject = Instantiate(objPrefab, spawnPosition, Quaternion.identity);
            spawnedObject.transform.localScale = scale; // Set the scale of the spawned object
        }
    }

    private void RotateObject()
    {
        if (spawnedObject != null)
        {
            // Rotate the picked object using the mouse
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor for rotation

            float rotationSpeed = 2f;
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            spawnedObject.transform.Rotate(Vector3.up, mouseX, Space.World);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor when no object is spawned
        }
    }

    private void DestroyObject()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - (Screen.width * 0.2f), Screen.height - (Screen.height * 0.95f), Screen.width * 0.4f, Screen.height * 0.85f), "<size=80>" + objectIntMessage + "<i>" + objectNameMessage + "</i></size>", new GUIStyle { alignment = TextAnchor.UpperCenter, normal = { textColor = Color.white } });
        GUI.Label(new Rect(Screen.width / 2 - (Screen.width * 0.1f), Screen.height - (Screen.height * 0.07f), Screen.width * 0.4f, Screen.height * 0.14f), "<size=50>" + interactionMessage + "</size>");
    }
}
