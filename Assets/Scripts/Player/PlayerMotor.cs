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
        numKeys = 0;
        interactionMessage = "";


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
            //Debug.Log(-cameraRotation);
            //cam.transform.Rotate(-cameraRotation);
            
            Vector3 angles = cam.transform.eulerAngles - cameraRotation;

            if (angles.x < maxCamera || angles.x > (360 - maxCamera)) 
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
    }

    private void handleLife() 
    {
        Damage += 2;
        if (Health - Damage >= 0)
        {
            cameraEffects.GetComponent<UIMaterialSwitcher>().damageScreen = (Damage - 1) > 0 ? Damage - 1 : 0 ;
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
        Health = Health + 2 >= 5 ? 5 : Health + 2;
        Damage = Damage - 2 <= 0 ? 0 : Damage - 2;
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("specialdoor") && other.gameObject.name.Contains(VictoryKey) && keysFound["key" + VictoryKey] == true)
        {
            audioSource.PlayOneShot(unlock);
            other.gameObject.GetComponent<Animator>().Play("open");
            StartCoroutine(win(claps, true));
            
        }
        else if (other.gameObject.CompareTag("specialdoor") && !other.gameObject.name.Contains(VictoryKey) && keysFound["key" + VictoryKey] == true)
        {
            audioSource.PlayOneShot(unlock);
            other.gameObject.GetComponent<Animator>().Play("open");
            StartCoroutine(win(boo, false));
        }
        else if (other.gameObject.CompareTag("specialdoor") && keysFound["key" + VictoryKey] == false)
        {
            audioSource.PlayOneShot(locked);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - (Screen.width * 0.05f), Screen.height - (Screen.height * 0.14f), Screen.width * 0.5f, Screen.height * 0.14f), "<size=50>" + interactionMessage + "</size>");
    }

    IEnumerator deleteMessage() 
    {
        yield return new WaitForSeconds(4f);
        interactionMessage = "";
    }

    IEnumerator win(AudioClip audio, bool w) 
    {
        yield return new WaitForSeconds(3f);
        audioSource.PlayOneShot(audio);
        if (w) 
        {
            SceneManager.LoadScene("Win");
        }
    }

}
