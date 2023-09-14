using UnityEngine;

public class ObjectPickupAndRotate_Funhouse : MonoBehaviour
{
    public GameObject ticketPrefab;
    public GameObject missingpersonPrefab;

    public bool ticket = false;
    public bool missingperson = false;

    private bool isPaused = false;
    private float originalTimeScale;
    private GameObject spawnedObject;

    private string objectIntMessage = "";
    private string objectNameMessage = "";
    private string interactionMessage = "";

//Doors assets
    private MeshRenderer policeTape1;
    private MeshRenderer policeTape2;
    private MeshRenderer policeTape3;
    private AudioSource doorsOpen;

    private void Start()
    {
        originalTimeScale = Time.timeScale;
        objectIntMessage = "";
        objectNameMessage = "";
        interactionMessage = "";
        Cursor.visible = false;
        policeTape1 = GameObject.Find ("PoliceTapeD1").GetComponent<MeshRenderer>();
        policeTape2 = GameObject.Find ("PoliceTapeD2").GetComponent<MeshRenderer>();
        policeTape3 = GameObject.Find ("PoliceTapeD3").GetComponent<MeshRenderer>();
        doorsOpen = GameObject.Find ("DoorSound").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (ticket && !isPaused)
        {
            PauseScene();
            SpawnObject(ticketPrefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "TICKET";
            interactionMessage = "Press Q to return";
        }
        if (missingperson && !isPaused)
        {
            PauseScene();
            SpawnObject(missingpersonPrefab, new Vector3(1f, 1f, 1f));
            objectIntMessage = "Found ";
            objectNameMessage = "MISSING PERSON POSTER";
            interactionMessage = "Press Q to return";
        }
        if (Input.GetKeyDown(KeyCode.Q) && isPaused)
        {
            DestroyObject();
            ResumeScene();
            if(ticket==true)
            {
                DoorsOpen();
            }
            ticket = false;
            missingperson = false;
            objectIntMessage = "";
            objectNameMessage = "";
            interactionMessage = "";
        }

        // Allow rotation even when the scene is paused
        RotateObject();
    }

    private void DoorsOpen()
    {
        policeTape1.enabled = false;
        policeTape2.enabled = false;
        policeTape3.enabled = false;
        doorsOpen.Play();
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
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height * 0.15f), "<color=white><size=50>" + interactionMessage + "</size></color>", style);
        //GUI.Label(new Rect(Screen.width / 2 - (Screen.width * 0.2f), Screen.height * 0.8f, Screen.width * 0.4f, Screen.height * 0.14f), "<size=50>" + interactionMessage + "</size>");
        //GUI.Label(new Rect(Screen.width / 2 - (Screen.width * 0.1f), Screen.height - (Screen.height * 0.07f), Screen.width * 0.4f, Screen.height * 0.14f), "<size=50>" + interactionMessage + "</size>");
    }
}
