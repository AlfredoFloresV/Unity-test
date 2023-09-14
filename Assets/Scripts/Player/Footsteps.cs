using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField]
    private GameObject footsteps;

    [SerializeField]
    private GameObject flashlight;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
           Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            EnableFootsteps();
        }
        else
        {
            DisableFootSteps();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            footsteps.GetComponent<AudioSource>().pitch = 1.2f;
            //flashlight.GetComponent<Animator>().Play("LightSprint");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            footsteps.GetComponent<AudioSource>().pitch = 0.9f;
            //flashlight.GetComponent<Animator>().Play("LightWalking");
        }
    }

    private void EnableFootsteps()
    {
        footsteps.SetActive(true);
    }

    private void DisableFootSteps()
    {
        footsteps.SetActive(false);
    }
}
