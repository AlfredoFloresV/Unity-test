using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;


    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            cam.GetComponent<Animator>().Play("HeadBobbing");
        }
        else 
        {
            cam.GetComponent<Animator>().Play("Idle");
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

}
