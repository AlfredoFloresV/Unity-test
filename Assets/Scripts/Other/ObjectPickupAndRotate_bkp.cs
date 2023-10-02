using UnityEngine;

public class ObjectPickupAndRotate_bkp : MonoBehaviour
{
    public GameObject key1Prefab;
    public GameObject key2Prefab;
    public GameObject key3Prefab;
    public GameObject key4Prefab;
    public GameObject drawing1Prefab;
    public GameObject drawing2Prefab;
    public GameObject drawing3Prefab;
    public GameObject drawing4Prefab;
    public GameObject drawing5Prefab;
    public GameObject invite1Prefab;
    public GameObject invite2Prefab;
    public GameObject invite3Prefab;
    public GameObject polaroid1Prefab;
    public GameObject polaroid2Prefab;
    public GameObject polaroid3Prefab;
    public GameObject polaroid4Prefab;
    public GameObject polaroid6Prefab;

    public bool key1 = false;
    public bool key2 = false;
    public bool key3 = false;
    public bool key4 = false;
    public bool drawing1 = false;
    public bool drawing2 = false;
    public bool drawing3 = false;
    public bool drawing4 = false;
    public bool drawing5 = false;
    public bool invite1 = false;
    public bool invite2 = false;
    public bool invite3 = false;
    public bool polaroid1 = false;
    public bool polaroid2 = false;
    public bool polaroid3 = false;
    public bool polaroid4 = false;
    public bool polaroid6 = false;

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
        Cursor.visible = false;
    }

    private void Update()
    {
        if (key1 && !isPaused)
        {
            PauseScene();
            SpawnObject(key1Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "PINK KEY";
            interactionMessage = "Press Q to return";
        }
        if (key2 && !isPaused)
        {
            PauseScene();
            SpawnObject(key2Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "YELLOW KEY";
            interactionMessage = "Press Q to return";
        }
        if (key3 && !isPaused)
        {
            PauseScene();
            SpawnObject(key3Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "GREEN KEY";
            interactionMessage = "Press Q to return";
        }
        if (key4 && !isPaused)
        {
            PauseScene();
            SpawnObject(key4Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "ORANGE KEY";
            interactionMessage = "Press Q to return";
        }
        if (drawing1 && !isPaused)
        {
            PauseScene();
            SpawnObject(drawing1Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "DRAWING";
            interactionMessage = "Press Q to return";
        }
        if (drawing2 && !isPaused)
        {
            PauseScene();
            SpawnObject(drawing2Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "DRAWING";
            interactionMessage = "Press Q to return";
        }
        if (drawing3 && !isPaused)
        {
            PauseScene();
            SpawnObject(drawing3Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "DRAWING";
            interactionMessage = "Press Q to return";
        }
        if (drawing4 && !isPaused)
        {
            PauseScene();
            SpawnObject(drawing4Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "DRAWING";
            interactionMessage = "Press Q to return";
        }
        if (drawing5 && !isPaused)
        {
            PauseScene();
            SpawnObject(drawing5Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "DRAWING";
            interactionMessage = "Press Q to return";
        }
        if (invite1 && !isPaused)
        {
            PauseScene();
            SpawnObject(invite1Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "PARTY INVITATION";
            interactionMessage = "Press Q to return";
        }
        if (invite2 && !isPaused)
        {
            PauseScene();
            SpawnObject(invite2Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "PARTY INVITATION";
            interactionMessage = "Press Q to return";
        }
        if (invite3 && !isPaused)
        {
            PauseScene();
            SpawnObject(invite3Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "PARTY INVITATION";
            interactionMessage = "Press Q to return";
        }
        if (polaroid1 && !isPaused)
        {
            PauseScene();
            SpawnObject(polaroid1Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "OLD PHOTOGRAPH";
            interactionMessage = "Press Q to return";
        }
        if (polaroid2 && !isPaused)
        {
            PauseScene();
            SpawnObject(polaroid2Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "OLD PHOTOGRAPH";
            interactionMessage = "Press Q to return";
        }
        if (polaroid3 && !isPaused)
        {
            PauseScene();
            SpawnObject(polaroid3Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "OLD PHOTOGRAPH";
            interactionMessage = "Press Q to return";
        }
        if (polaroid4 && !isPaused)
        {
            PauseScene();
            SpawnObject(polaroid4Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "OLD PHOTOGRAPH";
            interactionMessage = "Press Q to return";
        }
        if (polaroid6 && !isPaused)
        {
            PauseScene();
            SpawnObject(polaroid6Prefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "OLD PHOTOGRAPH";
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
            drawing1 = false;
            drawing2 = false;
            drawing3 = false;
            drawing4 = false;
            drawing5 = false;
            invite1 = false;
            invite2 = false;
            invite3 = false;
            polaroid1 = false;
            polaroid2 = false;
            polaroid3 = false;
            polaroid4 = false;
            polaroid6 = false;
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
            Vector3 spawnPosition = transform.position + transform.forward * 0.1f; // Adjust the distance as needed
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

            float rotationSpeed = 3f;
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
