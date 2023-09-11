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

    private Rigidbody rb;
    private AudioSource audioSource;

    private float maxCamera = 40f;

    private bool isHurt;
    public int Health = 5;
    public int Damage = 0;
    public Dictionary<string, bool> keysFound;
    public string VictoryKey;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        isHurt = false;
        keysFound = new Dictionary<string, bool>();
        Random random = new Random();

        VictoryKey = "" + (random.Next(1, 5));
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
        if (!isHurt) 
        {
            if (velocity != Vector3.zero)
            {
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
                cam.GetComponent<Animator>().Play("HeadBobbing");
            }
            else
            {
                rb.angularVelocity = Vector3.zero;
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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            if (isHurt == false)
            {
                Debug.Log("shake");
                isHurt = true;
                audioSource.PlayOneShot(hurt);
                cam.GetComponent<Animator>().Play("ShakeCamera");
                StartCoroutine(recover());
                cameraEffects.GetComponent<UIFadeInAndOut>().damage = true;
                handleLife();
            }
        }
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

    IEnumerator recover() 
    {
        yield return new WaitForSeconds(3f);
        isHurt = false;
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

    public void handleKeys(string key) 
    {
        audioSource.PlayOneShot(grabbingKeys);
        keysFound[key] = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("specialdoor") && other.gameObject.name.Contains(VictoryKey) && keysFound["key" + VictoryKey] == true)
        {
            SceneManager.LoadScene("Win");
        }
        else if (other.gameObject.CompareTag("specialdoor") && !other.gameObject.name.Contains(VictoryKey) && keysFound["key" + VictoryKey] == true)
        {
            audioSource.PlayOneShot(boo);
            other.gameObject.GetComponent<Animator>().Play("open");
        }
        else if (other.gameObject.CompareTag("specialdoor") && keysFound["key" + VictoryKey] == false)
        {
            audioSource.PlayOneShot(locked);
        }
    }
}
