using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraEffects;

    [SerializeField]
    float detectionDistance = 3f;

    [SerializeField]
    private Material highlightMaterial;
    [SerializeField]
    private Material originalMaterial;
    [SerializeField]
    private Material selectedMaterial;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject flashLight;

    GameObject lastHighlightedObject = null;

    private Shader standard;
    private Shader highlight;

    private string interactionMessage = ""; // Message to display when an object is selected

    private PlayerMotor playerMotor;

    private void Start()
    {
        standard = Shader.Find("Standard"); //Not working
        highlight = Shader.Find("Unlit/Texture");
        playerMotor = GetComponent<PlayerMotor>();
        StartCoroutine(message());
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
            interactingWithObject(false);
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
            interactingWithObject(false);
        }
    }

    void HighlightObjectInCenterOfCam()
    {
        // Ray from the center of the viewport.
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;

        Vector3 forward = cam.transform.TransformDirection(Vector3.forward) * 3f;
        Debug.DrawRay(cam.transform.position, forward, Color.green);
        
        // Check if we hit something.
        if (Physics.Raycast(ray, out rayHit, detectionDistance))
        {
            // Get the object that was hit.
            GameObject hitObject = rayHit.collider.gameObject;
            HighlightObject(hitObject);


            if (rayHit.collider.tag == "doorBtn")
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    interactionMessage = "";
                    rayHit.collider.gameObject.GetComponent<OpenDoor>().open();
                    rayHit.collider.gameObject.GetComponent<Renderer>().material = selectedMaterial;
                }

                //HandleObjectInteraction(rayHit.collider.transform);
            }
            if (rayHit.collider.tag == "biscuit") 
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    interactingWithObject(false);
                    rayHit.collider.gameObject.SetActive(false);
                    GetComponent<PlayerMotor>().restoreHealth();
                }
            }
            if (rayHit.collider.tag == "battery") 
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    rayHit.collider.gameObject.SetActive(false);
                    GetComponent<PlayerMotor>().restoreLight();
                }
            }
            if (rayHit.collider.tag == "key1" || rayHit.collider.tag == "key2" || rayHit.collider.tag == "key3" || rayHit.collider.tag == "key4") 
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    rayHit.collider.gameObject.SetActive(false);
                    GetComponent<PlayerMotor>().handleKeys(rayHit.collider.tag);
                }
            }

            if (rayHit.collider.tag == "larry_face")
            {


                if (rayHit.collider.gameObject.GetComponentInParent<LarryActions>().currentState != LarryState.Stun
                    && flashLight.GetComponent<Flashlight>().lightEnabled() == true
                    && flashLight.GetComponent<Flashlight>().intensity >= 0.5 
                    && flashLight.GetComponent<Flashlight>().focus == true
                    && GetComponent<PlayerMotor>().hit == false) 
                {
                    LarryActions ai = rayHit.collider.gameObject.GetComponentInParent<LarryActions>();
                    ai.StunActions();
                    GetComponent<PlayerMotor>().spendLight();
                }
            
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

    private void interactingWithObject(bool interacting) 
    {
        if (interacting == true)
        {
            interactionMessage = "Press E to interact";
            cameraEffects.GetComponent<UISight>().pointing(true);
        }
        else 
        {
            interactionMessage = "";
            cameraEffects.GetComponent<UISight>().pointing(false);
        }
    }

    // Display GUI elements
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height * 0.15f), "<color=white><size=50>" + interactionMessage + "</size></color>", style);
        //GUI.Label(new Rect(Screen.width / 2 - (Screen.width * 0.25f), Screen.height * 0.8f, Screen.width * 0.5f, Screen.height * 0.14f), "<size=50>" + interactionMessage + "</size>", style);
    }

    IEnumerator message() 
    {
        yield return new WaitForSeconds(7f);
        interactionMessage = "I need to find a way out";
    }

    IEnumerator clean() 
    {
        yield return new WaitForSeconds(10f);
        interactionMessage = "";
    }

}
