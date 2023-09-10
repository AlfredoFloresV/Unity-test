using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private AudioClip hurt;

    private Rigidbody rb;
    private AudioSource audioSource;

    private bool isHurt;
    public int Health = 5;
    public int Damage = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        isHurt = false;
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
                cam.GetComponent<Animator>().Play("Idle");
            }
        }
    }

    private void PerformRotation() 
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam != null) 
        {
            cam.transform.Rotate(-cameraRotation);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy"))
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
        Damage += 1;
        if (Health - Damage >= 0)
        {
            cameraEffects.GetComponent<UIMaterialSwitcher>().damageScreen = Damage;
        }
        else 
        {
            //Actualizar!!
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator recover() 
    {
        yield return new WaitForSeconds(5f);
        isHurt = false;
    }
}
