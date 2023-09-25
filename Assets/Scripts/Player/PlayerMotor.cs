using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject cameraEffects;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

    [SerializeField]
    private GameObject flashLight;

    [SerializeField]
    private AudioClip hurt;

    [SerializeField]
    private AudioClip eatingBiscuit;

    [SerializeField]
    private AudioClip rechargeBattery;

    [SerializeField]
    private AudioClip grabbingKeys;

    [SerializeField]
    private AudioClip boo;

    [SerializeField]
    private AudioClip locked;

    [SerializeField]
    private AudioClip unlock;

    [SerializeField]
    private AudioClip claps;

    [SerializeField]
    private GameObject fade;

    [SerializeField]
    float detectionDistance = 3f;

    [SerializeField]
    private GameObject hearbeat;

    [SerializeField]
    private GameObject bgmusic;

    private Rigidbody rb;
    private AudioSource audioSource;

    private float maxCamera = 40f;

    
    private RigidbodyConstraints cons;
    public int Health = 5;
    public int Damage = 0;
    public Dictionary<string, bool> keysFound;
    public string VictoryKey;
    public int numKeys;
    private string interactionMessage;
    public bool hit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        keysFound = new Dictionary<string, bool>();

        for (int i = 1; i < 5; i++) 
            keysFound["key" + i] = false;
        
        cons = rb.constraints;
        Random random = new Random();

        VictoryKey = "" + (random.Next(1, 5));
        Debug.Log("VictoryKey" + VictoryKey);
        numKeys = 0;
        interactionMessage = "";
        hit = false;

    }

    public void Move(Vector3 v)
    {
        velocity = v;
    }

    public void Rotate(Vector3 r)
    {
        rotation = r;
    }

    public void RotateCamera(Vector3 cr)
    {
        cameraRotation = cr;
    }

    private void Update()
    {
        HighlightObject();
    }

    //Run every physics iteration
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.constraints = cons;
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            if (cam.gameObject.activeSelf == true) 
            {
                cam.GetComponent<Animator>().Play("HeadBobbing");
            }
                
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.angularVelocity = Vector3.zero;
            if (cam.gameObject.activeSelf == true)
            {
                cam.GetComponent<Animator>().Play("Idle");
            }
        }
    }

    private void PerformRotation() 
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam != null) 
        {
            Vector3 angles = cam.transform.eulerAngles - cameraRotation;

            if (angles.x < maxCamera * 2 || angles.x > (360 - maxCamera)) 
            {
                cam.transform.Rotate(-cameraRotation);
            }
        }
    }


    public void playerAttacked() 
    {
        Debug.Log("shake");
        audioSource.PlayOneShot(hurt);
        cam.GetComponent<Animator>().Play("ShakeCamera");
        cameraEffects.GetComponent<UIFadeInAndOut>().damage = true;
        handleLife();
        hit = false;
    }

    private void handleLife() 
    {
        Damage += 1;
        if (Health - Damage >= 0)
        {
            cameraEffects.GetComponent<UIMaterialSwitcher>().damageScreen = Damage;
        }
        else 
        {
            //Actualizar!!
            SceneManager.LoadScene("GameOver1 1");
        }
    }

    
    public void restoreHealth() 
    {
        audioSource.PlayOneShot(eatingBiscuit);
        Health = Health + 1 >= 5 ? 5 : Health + 1;
        Damage = Damage - 1 <= 0 ? 0 : Damage - 1;
        cameraEffects.GetComponent<UIMaterialSwitcher>().damageScreen = Damage;
    }

    public void restoreLight() 
    {
        audioSource.PlayOneShot(rechargeBattery);
        flashLight.GetComponent<Flashlight>().chargeBattery();
    }

    public void spendLight() 
    {
        flashLight.GetComponent<Flashlight>().spendLight(0.3f);
    }

    public float getIntensity() 
    {
        return flashLight.GetComponent<Flashlight>().intensity;
    }

    public bool getFocus() 
    {
        return flashLight.GetComponent<Flashlight>().focus;
    }

    public bool lightEnabled() 
    {
        return flashLight.GetComponent<Flashlight>().lightEnabled();
    }

    public void handleKeys(string key) 
    {
        audioSource.PlayOneShot(grabbingKeys);
        keysFound[key] = true;
        numKeys++;
        interactionMessage = numKeys + "/4 keys";
        StartCoroutine(deleteMessage());
    }

    private void HighlightObject()
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
            if (rayHit.collider.tag == "specialdoor")
            {
                interactingWithObject(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingWithObject(false);
                    string num = rayHit.collider.gameObject.name.Substring(rayHit.collider.gameObject.name.Length - 1);
                    bool isCorrectDoor = num == VictoryKey;

                    if (keysFound["key" + num] == true)
                    {
                        audioSource.PlayOneShot(unlock);
                        rayHit.collider.tag = "specialdoorused";

                        if (isCorrectDoor)
                        {
                            StartCoroutine(win(rayHit.collider.gameObject, claps, true));
                        }
                        else
                        {
                            StartCoroutine(win(rayHit.collider.gameObject, boo, false));
                        }
                    }
                    else
                    {
                        audioSource.PlayOneShot(locked);
                    }

                    //rayHit.collider.gameObject.SetActive(false);
                }
            }
            else 
            {
                interactingWithObject(false);
            }

        }
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

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height * 0.15f), "<color=white><size=50>" + interactionMessage + "</size></color>", style);
    }

    IEnumerator deleteMessage() 
    {
        yield return new WaitForSeconds(6f);
        interactionMessage = "";
    }

    IEnumerator win(GameObject go, AudioClip audio, bool w) 
    {
        yield return new WaitForSeconds(4f);
        go.GetComponent<Animator>().Play("open");
        audioSource.PlayOneShot(audio);
        if (w) 
        {
            hearbeat.SetActive(false);
            bgmusic.SetActive(false);

            fade.SetActive(true);
            fade.GetComponent<Animator>().Play("FadeOut");
            StartCoroutine(winGame());
        }
    }

    IEnumerator winGame() 
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("Win");
    }

}
