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

    [SerializeField]
    private AudioClip paperAudio;

    [SerializeField]
    private AudioClip scream;

    [SerializeField]
    private GameObject fade;

    GameObject lastHighlightedObject = null;

    private Shader standard;
    private Shader highlight;

    private string interactionMessage = ""; // Message to display when an object is selected
    private ObjectPickupAndRotate_Funhouse pickup;
    private Animator animDoor1;
    private Animator animDoor2;
    private BoxCollider funhouseFloor;
    private Animator playerAnimator;
    private AudioSource audioSource;
    public string targetSceneName = "DungeonLevel";

    private float WidthPosition;
    private float HeightPosition;

    private float WidthBox;

    public int msgState;
    private Dictionary<int, string> messages;
    private string prevMsg;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        standard = Shader.Find("Standard"); //Not working
        highlight = Shader.Find("Unlit/Texture");
        pickup = GameObject.Find ("Camera").GetComponent<ObjectPickupAndRotate_Funhouse>();
        animDoor1 = GameObject.Find ("StartDoor1").GetComponent<Animator>();
        animDoor2 = GameObject.Find ("StartDoor2").GetComponent<Animator>();
        funhouseFloor = GameObject.Find ("FunhouseFloor").GetComponent<BoxCollider>();
        playerAnimator = GameObject.Find ("Player").GetComponent<Animator>();

        messages = new Dictionary<int, string>();
        messages[0] = "I should find a way inside...";
        messages[1] = "It's locked. I need to find something to open the doors";
        messages[2] = "If only I could buy a <b>ticket</b>";
        messages[3] = "What?";
        messages[4] = "I should take a closer look at that <b>arrow</b>";
        msgState = 0;
        prevMsg = "";
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
            if (interactionMessage.Equals("Press E to interact")) 
            {
                interactionMessage = prevMsg != "" ? prevMsg : "";
                prevMsg = "";
            }
            
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
            if (interactionMessage.Equals("Press E to interact"))
            {
                interactionMessage = prevMsg != "" ? prevMsg : "";
                prevMsg = "";
            }
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
                if (!interactionMessage.Equals("Press E to interact") && !interactionMessage.Equals(""))
                    prevMsg = interactionMessage;

                interactionMessage = "Press E to interact";

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    audioSource.PlayOneShot(paperAudio);
                    audioSource.pitch = 1.3f;
                    interactionMessage = "";
                    Destroy (GameObject.FindWithTag("ticket"));
                    pickup.ticket = true;
                    PlayerPrefsManager.SaveBool("ticket", true);
                    animDoor1.enabled = true;
                    animDoor2.enabled = true;
                    StopAllCoroutines();
                    StartCoroutine(afterTicket());
                }
            }

            if (rayHit.collider.tag == "newspaper") 
            {
                if (!interactionMessage.Equals("Press E to interact") && !interactionMessage.Equals(""))
                    prevMsg = interactionMessage;

                interactionMessage = "Press E to interact";

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    audioSource.pitch = 1f;
                    audioSource.PlayOneShot(paperAudio);
                    interactionMessage = "";
                    Destroy (GameObject.FindWithTag("newspaper"));
                    pickup.newspaper = true;
                    PlayerPrefsManager.SaveBool("newspaper", true);
                }
            }

            if (rayHit.collider.tag == "missingposter") 
            {
                if (!interactionMessage.Equals("Press E to interact") && !interactionMessage.Equals(""))
                    prevMsg = interactionMessage;

                interactionMessage = "Press E to interact";

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    audioSource.pitch = 1f;
                    audioSource.PlayOneShot(paperAudio);
                    interactionMessage = "";
                    Destroy (GameObject.FindWithTag("missingposter"));
                    pickup.missingperson = true;
                    PlayerPrefsManager.SaveBool("missingperson", true);
                }
            }

            if (rayHit.collider.tag == "arrow") 
            {
                if (!interactionMessage.Equals("Press E to interact") && !interactionMessage.Equals(""))
                    prevMsg = interactionMessage;

                interactionMessage = "Press E to interact";

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    interactionMessage = "";
                    StopAllCoroutines();
                    audioSource.pitch = 1f;
                    audioSource.PlayOneShot(scream);
                    funhouseFloor.enabled = false;
                    playerAnimator.enabled = true;
                    fade.GetComponent<Animator>().Play("FadeOut");
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
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(targetSceneName);
    }

    void Update()
    {
        HighlightObjectInCenterOfCam();

        if (msgState != -1) 
        {
            StartCoroutine(instructions(messages[msgState], msgState == 2 ? -1 : msgState + 1));
            msgState = -1;
        }
    }

    // Display GUI elements
    private void OnGUI()
    {
        //GUIStyle style = new GUIStyle();
        //style.alignment = TextAnchor.MiddleCenter;
        //GUI.Label(new Rect(WidthPosition, HeightPosition, WidthBox, Screen.height * 0.15f), "<size=50>" + interactionMessage + "</size>", style);
        //GUI.Label(new Rect(Screen.width / 2 - (Screen.width * 0.1f), Screen.height - (Screen.height * 0.07f), Screen.width * 0.4f, Screen.height * 0.14f), "<size=50>" + interactionMessage + "</size>");
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height * 0.15f), "<color=white><size=50>" + interactionMessage + "</size></color>", style);
    }


    IEnumerator instructions(string message, int nextState)
    {
        interactionMessage = message;
        yield return new WaitForSeconds(5f);
        interactionMessage = "";
        StartCoroutine(wait(nextState));
    }

    
    IEnumerator wait(int nextState) 
    {
        yield return new WaitForSeconds(40f);
        msgState = nextState;
    }

    IEnumerator afterTicket() 
    {
        yield return new WaitForSeconds(2f);
        msgState = 3;
    }
}
