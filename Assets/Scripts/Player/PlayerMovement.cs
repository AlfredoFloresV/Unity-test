using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 12f;

    [SerializeField]
    private GameObject obj;

    [SerializeField]
    private GameObject footsteps;

    [SerializeField]
    private GameObject pauseObj;

    [SerializeField]
    private GameObject freezeObj;

    private float x, z;
    private float currSpeed;
    private bool sprint = false;
    private Rigidbody rb;
    private RigidbodyConstraints cons;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cons = rb.constraints;
        currSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseObj.GetComponent<PauseMenu>().isPaused || freezeObj.GetComponent<ObjectPickupAndRotate>().Freezed)
            return;
        
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift)) 
        {
            sprint = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprint = false;
        }
    }

    private void FixedUpdate()
    {
        if (pauseObj.GetComponent<PauseMenu>().isPaused)
            return;

        Vector3 move = transform.right * x + transform.forward * z;
        rb.MovePosition(transform.position + (move * currSpeed * Time.deltaTime));
        //rb.Move(transform.position + (move * currSpeed * Time.fixedDeltaTime), Quaternion.identity);
        //controller.Move(move * currSpeed * Time.fixedDeltaTime);

        if (move.x == 0 && move.z == 0)
        {
            obj.GetComponent<Animator>().Play("Idle");
            rb.constraints = RigidbodyConstraints.FreezeAll;

            performWalkEffect(false);
        }
        else 
        {
            rb.constraints = cons;
            performWalkEffect(true);
        }
    }

    private void performWalkEffect(bool execute) 
    {
        footsteps.SetActive(execute);
        if (!execute) return;

        if (sprint)
        {
            obj.GetComponent<Animator>().Play("HeadBobbing_fixed2");
            footsteps.GetComponent<AudioSource>().pitch = 1.2f;
            currSpeed = speed * 2.3f;
        }
        else 
        {
            obj.GetComponent<Animator>().Play("HeadBobbing_fixed");
            footsteps.GetComponent<AudioSource>().pitch = 0.9f;
            currSpeed = speed;
        }
    }
}
