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
    private AudioClip paperAudio;

    [SerializeField]
    private AudioClip keysAudio;

    [SerializeField]
    private AudioClip biscuitAudio;

    [SerializeField]
    private AudioClip batteryAudio;

    [SerializeField]
    private GameObject flashLight;

    [SerializeField]
    private GameObject textObj;

    GameObject lastHighlightedObject = null;

    private ObjectPickupAndRotate pickup;
    private AudioSource audioSource;

    //private string interactionMessage = ""; // Message to display when an object is selected
    //private string introMessage = "I need to find out what happened to those kids";
    private List<string> collectibles;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pickup = cam.GetComponent<ObjectPickupAndRotate>();

        collectibles = new List<string>();

        foreach (ObjectPickupAndRotate.Collectible c in pickup.collectibleList)
        {
            collectibles.Add(c.name);
        }

        StartCoroutine(introMsg());
    }

    IEnumerator introMsg() 
    {
        yield return new WaitForSeconds(7f);
        textObj.GetComponent<TextSupportGUI>().setInteractionMessage("I need to find out what happened to those kids", true);
    }

    void HighlightObject(GameObject gameObject)
    {
        if (gameObject.GetComponent<MeshRenderer>().sharedMaterial != selectedMaterial) 
        {
            ClearHighlighted();
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = highlightMaterial;
            lastHighlightedObject = gameObject;
        }
    }

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null && lastHighlightedObject.GetComponent<MeshRenderer>().sharedMaterial == highlightMaterial)
        {
            lastHighlightedObject.GetComponent<MeshRenderer>().sharedMaterial = originalMaterial;
            lastHighlightedObject = null;
            //interactingWithObject(false);
        }
    }

    void HighlightObjectInCenterOfCam()
    {
        // Ray from the center of the viewport.
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;

        Vector3 forward = cam.transform.TransformDirection(Vector3.forward) * 2f;
        Debug.DrawRay(cam.transform.position, forward, Color.green);
        
        // Check if we hit something.
        if (Physics.Raycast(ray, out rayHit, detectionDistance))
        {
            // Get the object that was hit.
            GameObject hitObject = rayHit.collider.gameObject;
            string colliderTag = rayHit.collider.tag;
            //ClearHighlighted();

            if (colliderTag == "doorBtn" && hitObject.GetComponent<MeshRenderer>().sharedMaterial != selectedMaterial)
            {
                HighlightObject(hitObject);
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    hitObject.GetComponent<OpenDoor>().open();
                    hitObject.GetComponent<MeshRenderer>().sharedMaterial = selectedMaterial;
                }
            }
            else if (colliderTag == "biscuit")
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    audioSource.PlayOneShot(biscuitAudio);
                    rayHit.collider.gameObject.SetActive(false);
                    GetComponent<PlayerMotor>().restoreHealth();
                }
            }
            else if (colliderTag == "battery")
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    audioSource.PlayOneShot(batteryAudio);
                    rayHit.collider.gameObject.SetActive(false);
                    GetComponent<PlayerMotor>().restoreLight();
                }
            }
            else if (colliderTag == "larry_face")
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
            else if (colliderTag == "specialdoor")
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    string num = rayHit.collider.gameObject.name.Substring(rayHit.collider.gameObject.name.Length - 1);
                    GetComponent<PlayerMotor>().verifyVictoryCondition(num, rayHit.collider.gameObject);
                }
            }
            else if (collectibles.Contains(colliderTag))
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    Destroy(rayHit.collider.gameObject);
                    pickup.displayObject(colliderTag);
                    //Keys
                    if (colliderTag.Contains("key"))
                    {
                        audioSource.PlayOneShot(keysAudio);
                        GetComponent<PlayerMotor>().handleKeys(colliderTag);
                        string text = GetComponent<PlayerMotor>().numKeys + "/4 keys found";
                        textObj.GetComponent<TextSupportGUI>().setSubInteractionMessage(text);
                    }
                    else
                    {
                        audioSource.PlayOneShot(paperAudio);
                        PlayerPrefsManager.SaveBool(colliderTag, true);
                    }
                }
            }
            else 
            {
                ClearHighlighted();
                interactingWithObject(false);
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
            textObj.GetComponent<TextSupportGUI>().setInteractionMessage("Press E to interact", false);
            cameraEffects.GetComponent<UISight>().pointing(true);
        }
        else 
        {
            //cleanMessage();
            textObj.GetComponent<TextSupportGUI>().cleanMessages();
            cameraEffects.GetComponent<UISight>().pointing(false);
        }
    }

    // Display GUI elements
    /*
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height * 0.15f), "<color=white><size=50>" + interactionMessage + "</size></color>", style);
    }
    */

    /*
    IEnumerator message() 
    {
        yield return new WaitForSeconds(7f);
        interactionMessage = introMessage;
        StartCoroutine(clean());
    }

    IEnumerator clean() 
    {
        yield return new WaitForSeconds(10f);
        interactionMessage = "";
    }
    */
}
