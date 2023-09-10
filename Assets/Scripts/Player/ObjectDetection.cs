using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    private LayerMask mask;

    [SerializeField]
    private float detectionDistance = 0.5f;

    [SerializeField]
    private Material btnSelected;

    [SerializeField]
    private Material btnFocus;

    void Start()
    {
        mask = LayerMask.GetMask("Object Detection");
    }

    


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, detectionDistance, mask)) 
        {
            if (hit.collider.tag == "door") 
            {
                hit.collider.transform.GetComponent<Renderer>().material = btnFocus;

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    hit.collider.transform.GetComponent<OpenDoor>().open();
                    hit.collider.transform.GetComponent<Renderer>().material = btnSelected;
                }
            }

            if (hit.collider.tag == "enemy") 
            {
                Debug.Log("larry stun");
                AudioSource audioSource = hit.collider.transform.GetComponent<AudioSource>();
                audioSource.Play();
                hit.collider.transform.GetComponent<Animator>().Play("Larry_Stun3");
            }
        }
    }
}
