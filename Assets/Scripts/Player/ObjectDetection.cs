using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    [SerializeField]
    private Material highlightMaterial;
    [SerializeField]
    private Material originalMaterial;
    [SerializeField]
    private Material selectedMaterial;

    [SerializeField]
    private Camera cam;

    GameObject lastHighlightedObject = null;

    private string interactionMessage = ""; // Message to display when an object is selected
    void HighlightObject(GameObject gameObject)
    {
        if (lastHighlightedObject != gameObject)
        {
            if (gameObject.CompareTag("doorBtn") && gameObject.GetComponent<MeshRenderer>().sharedMaterial != selectedMaterial) 
            {
                ClearHighlighted();
                originalMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
                gameObject.GetComponent<MeshRenderer>().sharedMaterial = highlightMaterial;
            }

            lastHighlightedObject = gameObject;
            interactionMessage = "";
        }

    }

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null)
        {
            if (gameObject.CompareTag("doorBtn"))
            {
                Material m = lastHighlightedObject.GetComponent<MeshRenderer>().sharedMaterial;
                if (m == highlightMaterial)
                {
                    lastHighlightedObject.GetComponent<MeshRenderer>().sharedMaterial = originalMaterial;
                }
                
            }

            lastHighlightedObject = null;
            interactionMessage = "";
        }
    }

    void HighlightObjectInCenterOfCam()
    {
        float rayDistance = 5f;
        // Ray from the center of the viewport.
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;

        Vector3 forward = cam.transform.TransformDirection(Vector3.forward) * 5f;
        Debug.DrawRay(cam.transform.position, forward, Color.green);
        
        // Check if we hit something.
        if (Physics.Raycast(ray, out rayHit, rayDistance))
        {
            // Get the object that was hit.
            GameObject hitObject = rayHit.collider.gameObject;
            HighlightObject(hitObject);


            if (rayHit.collider.tag == "doorBtn")
            {
                interactionMessage = "Press E to interact";

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    interactionMessage = "";
                    rayHit.collider.gameObject.GetComponent<OpenDoor>().open();
                    rayHit.collider.gameObject.GetComponent<Renderer>().material = selectedMaterial;
                }

                //HandleObjectInteraction(rayHit.collider.transform);
            }
            if (rayHit.collider.tag == "face")
            {
                Debug.Log("stunned");
                GameObject enemy = GameObject.FindGameObjectWithTag("enemy");
                AudioSource audioSource = enemy.transform.GetComponent<AudioSource>();
                audioSource.Play();
                enemy.transform.GetComponent<Animator>().Play("Larry_Stun3");
                enemy.transform.GetComponent<LarryAI>().stun = true;
                StartCoroutine(enemy.transform.GetComponent<LarryAI>().recover());
            }

        }
        else
        {
            ClearHighlighted();
        }
    }

    void Update()
    {
        HighlightObjectInCenterOfCam();
    }

    // Display GUI elements
    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - (Screen.width * 0.1f), Screen.height - (Screen.height * 0.07f), Screen.width * 0.4f, Screen.height * 0.14f), "<size=50>" + interactionMessage + "</size>");
    }

}
