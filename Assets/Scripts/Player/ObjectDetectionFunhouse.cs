using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDetectionFunhouse : MonoBehaviour
{
    [SerializeField]
    float detectionDistance = 5f;

    //[SerializeField]
    //private Material highlightMaterial;
    //[SerializeField]
    //private Material originalMaterial;
    //[SerializeField]
    //private Material selectedMaterial;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject cameraEffects;

    [SerializeField]
    private GameObject textObj;

    [SerializeField]
    private AudioClip paperAudio;

    [SerializeField]
    private AudioClip scream;

    [SerializeField]
    private GameObject fade;

    private ObjectPickupAndRotate pickup;
    private IntroMessages introMsg;
    private Animator animDoor1;
    private Animator animDoor2;
    private BoxCollider funhouseFloor;
    private Animator playerAnimator;
    private AudioSource audioSource;
    public string targetSceneName = "DungeonLevel";

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        introMsg = GetComponent<IntroMessages>();
        pickup = cam.gameObject.GetComponent<ObjectPickupAndRotate>();
        animDoor1 = GameObject.Find("StartDoor1").GetComponent<Animator>();
        animDoor2 = GameObject.Find("StartDoor2").GetComponent<Animator>();
        funhouseFloor = GameObject.Find("FunhouseFloor").GetComponent<BoxCollider>();
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
    }

    void HighlightObjectInCenterOfCam()
    {
        // Ray from the center of the viewport.
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;

        Vector3 forward = cam.transform.TransformDirection(Vector3.forward) * 3f;
        Debug.DrawRay(cam.transform.position, forward, Color.green);
        interactingWithObject(false);

        // Check if we hit something.
        if (Physics.Raycast(ray, out rayHit, detectionDistance))
        {
            // Get the object that was hit.
            GameObject hitObject = rayHit.collider.gameObject;
            string currTag = rayHit.collider.tag;


            if (currTag == "ticket")
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    audioSource.PlayOneShot(paperAudio);
                    audioSource.pitch = 1.3f;
                    Destroy(GameObject.FindWithTag("ticket"));
                    PlayerPrefsManager.SaveBool("ticket", true);
                    animDoor1.enabled = true;
                    animDoor2.enabled = true;
                    pickup.displayObject(currTag);
                    afterTicket();
                }
            }

            else if (currTag == "newspaper")
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    audioSource.pitch = 1f;
                    audioSource.PlayOneShot(paperAudio);
                    Destroy(GameObject.FindWithTag("newspaper"));
                    PlayerPrefsManager.SaveBool("newspaper", true);
                    pickup.displayObject(currTag);
                }
            }

            else if (currTag == "missingposter")
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    audioSource.pitch = 1f;
                    audioSource.PlayOneShot(paperAudio);
                    Destroy(GameObject.FindWithTag("missingposter"));
                    PlayerPrefsManager.SaveBool("missingperson", true);
                    pickup.displayObject(currTag);
                }
            }

            else if (currTag == "arrow")
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    StopAllCoroutines();
                    audioSource.pitch = 1f;
                    audioSource.PlayOneShot(scream);
                    funhouseFloor.enabled = false;
                    playerAnimator.enabled = true;
                    fade.GetComponent<Animator>().Play("FadeOut");
                    StartCoroutine(NextLevel());
                }
            }
            else 
            {
                interactingWithObject(false);
            }
        }
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(targetSceneName);
    }

    private void interactingWithObject(bool interacting)
    {
        Debug.Log("interacting " + interacting);
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

    private void afterTicket() 
    {
        introMsg.continueIntro();
    }

    private void Update()
    {
        if(!pickup.Freezed) HighlightObjectInCenterOfCam();
    }
}
