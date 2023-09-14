using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDetectionFunhouse : MonoBehaviour
{
    [SerializeField]
    float detectionDistance = 5f;

    [SerializeField]
    private Material highlightMaterial;
    [SerializeField]
    private Material originalMaterial;
    [SerializeField]
    private Material selectedMaterial;

    [SerializeField]
    private Camera cam;

    GameObject lastHighlightedObject = null;

    private Shader standard;
    private Shader highlight;

    private string interactionMessage = ""; // Message to display when an object is selected
    private ObjectPickupAndRotate_Funhouse pickup;
    private Animator animDoor1;
    private Animator animDoor2;
    private BoxCollider funhouseFloor;
    private Animator playerAnimator;
    public string targetSceneName = "DungeonLevel"; 

    private void Start()
    {
        standard = Shader.Find("Standard"); //Not working
        highlight = Shader.Find("Unlit/Texture");
        pickup = GameObject.Find ("Camera").GetComponent<ObjectPickupAndRotate_Funhouse>();
        animDoor1 = GameObject.Find ("StartDoor1").GetComponent<Animator>();
        animDoor2 = GameObject.Find ("StartDoor2").GetComponent<Animator>();
        funhouseFloor = GameObject.Find ("FunhouseFloor").GetComponent<BoxCollider>();
        playerAnimator = GameObject.Find ("Player").GetComponent<Animator>();
    }

    void HighlightObject(GameObject gameObject)
    {
        if (lastHighlightedObject != gameObject)
        {
            if (gameObject.CompareTag("doorBtn") && gameObject.GetComponent<MeshRenderer>().sharedMaterial != selectedMaterial) 
            {
                ClearHighlighted();
                originalMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
                gameObject.GetComponent<MeshRenderer>().sharedMaterial = highlightMaterial;
                gameObject.GetComponent<MeshRenderer>().sharedMaterial.shader = highlight;
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
                    lastHighlightedObject.GetComponent<MeshRenderer>().sharedMaterial.shader = standard;
                }
            }
            lastHighlightedObject = null;
            interactionMessage = "";
        }
    }

    void HighlightObjectInCenterOfCam()
    {
        // Ray from the center of the viewport.
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;

        Vector3 forward = cam.transform.TransformDirection(Vector3.forward) * 5f;
        Debug.DrawRay(cam.transform.position, forward, Color.green);
        
        // Check if we hit something.
        if (Physics.Raycast(ray, out rayHit, detectionDistance))
        {
            // Get the object that was hit.
            GameObject hitObject = rayHit.collider.gameObject;
            HighlightObject(hitObject);

            if (rayHit.collider.tag == "ticket") 
            {
                interactionMessage = "Press E to interact";

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    interactionMessage = "";
                    Destroy(rayHit.transform.gameObject); 
                    pickup.ticket = true;
                    animDoor1.enabled = true;
                    animDoor2.enabled = true;
                }
            }

            if (rayHit.collider.tag == "missingposter") 
            {
                interactionMessage = "Press E to interact";

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    interactionMessage = "";
                    Destroy(rayHit.transform.gameObject); 
                    pickup.missingperson = true;
                }
            }

            if (rayHit.collider.tag == "arrow") 
            {
                interactionMessage = "Press E to interact";

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    interactionMessage = "";
                    funhouseFloor.enabled = false;
                    playerAnimator.enabled = true;
                    StartCoroutine(NextLevel());
                }
            }
        }
        else
        {
            ClearHighlighted();
        }
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(targetSceneName);
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
