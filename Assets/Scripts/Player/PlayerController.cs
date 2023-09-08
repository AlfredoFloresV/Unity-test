using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    private float orgSpeed;

    [SerializeField]
    private float sensitivityX = 15f;
    
    [SerializeField]
    private float sensitivityY = 5f;

    private PlayerMotor motor;

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        orgSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMove;
        Vector3 movVertical = transform.forward * zMove;

        Vector3 velocity = (movHorizontal + movVertical).normalized * speed;
        motor.Move(velocity);

        // Rotation
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * sensitivityX;
        motor.Rotate(rotation);

        // Camera Rotation
        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 cameraRotation = new Vector3(xRot, 0f, 0f) * sensitivityY;
        motor.RotateCamera(cameraRotation);


        if (Input.GetKey(KeyCode.Space)) 
        {
            speed = orgSpeed * 2f;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            speed = orgSpeed;
        }
    }
}
