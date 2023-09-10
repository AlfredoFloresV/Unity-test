using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection_Fixed : MonoBehaviour
{
    private LayerMask mask;

    [SerializeField]
    private float detectionDistance = 1.5f;

    [SerializeField]
    private Material btnOrignal;

    [SerializeField]
    private Material btnFocus;

    [SerializeField]
    private Material btnSelected; // The selected material

    private Dictionary<Transform, ObjectState> objectStates = new Dictionary<Transform, ObjectState>();

    // Enum to represent the different object states
    private enum ObjectState
    {
        Normal,
        Selected,
        Pressed
    }

    private string interactionMessage = ""; // Message to display when an object is selected

    void Start()
    {
        mask = LayerMask.GetMask("Object Detection");
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, detectionDistance, mask))
        {
            if (hit.collider.tag == "door")
            {
                HandleObjectInteraction(hit.collider.transform);
            }
            if (hit.collider.tag == "face")
            {
                Debug.Log("stunned");
                GameObject enemy = GameObject.FindGameObjectWithTag("enemy");
                AudioSource audioSource = enemy.transform.GetComponent<AudioSource>();
                audioSource.Play();
                enemy.transform.GetComponent<Animator>().Play("Larry_Stun3");
                enemy.transform.GetComponent<LarryAI>().stun = true;
                StartCoroutine(enemy.transform.GetComponent<LarryAI>().recover());
            }
            // Add additional tags here if needed for other objects.
        }
        else
        {
            StartCoroutine(revertMaterials());
        }
    }

    // Handle object interaction logic
    private void HandleObjectInteraction(Transform objectTransform)
    {
        // If the object is not in the dictionary, add it with the state set to Normal
        if (!objectStates.ContainsKey(objectTransform))
        {
            objectStates.Add(objectTransform, ObjectState.Normal);
        }

        ObjectState currentState = objectStates[objectTransform];

        switch (currentState)
        {
            case ObjectState.Normal:
                objectTransform.GetComponent<Renderer>().material = btnFocus;
                interactionMessage = ""; // Clear the interaction message
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Call the appropriate interaction method for the object
                    InteractWithObject(objectTransform);
                }
                else if (!Input.GetKey(KeyCode.E))
                {
                    objectStates[objectTransform] = ObjectState.Selected;
                    interactionMessage = "Press E to interact"; // Display interaction message
                }
                break;

            case ObjectState.Selected:
                objectTransform.GetComponent<Renderer>().material = btnFocus;
                interactionMessage = "Press E to interact"; // Display interaction message
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Call the appropriate interaction method for the object
                    InteractWithObject(objectTransform);
                }
                break;

            case ObjectState.Pressed:
                objectTransform.GetComponent<Renderer>().material = btnSelected;
                break;
        }
    }

    // Custom interaction method for objects (you can customize this for each object)
    private void InteractWithObject(Transform objectTransform)
    {
        objectTransform.GetComponent<OpenDoor>().open();
        objectStates[objectTransform] = ObjectState.Pressed;
        objectTransform.GetComponent<Renderer>().material = btnSelected;
    }

    // Display GUI elements
    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - (Screen.width * 0.1f), Screen.height - (Screen.height * 0.07f), Screen.width * 0.4f, Screen.height * 0.14f), "<size=50>" + interactionMessage + "</size>");
    }


    IEnumerator revertMaterials() 
    {
        yield return new WaitForSeconds(10f);

        // No object hit, revert the material of all objects to Normal
        foreach (var kvp in objectStates)
        {
            Transform obj = kvp.Key;
            ObjectState objState = kvp.Value;

            if (objState != ObjectState.Pressed)
            {
                Renderer objRenderer = obj.GetComponent<Renderer>();
                if (objRenderer != null)
                {
                    objRenderer.material = btnOrignal;
                    objectStates[obj] = ObjectState.Normal;
                }
            }
        }

        interactionMessage = ""; // Clear the interaction message when not hitting any object
    }
}
