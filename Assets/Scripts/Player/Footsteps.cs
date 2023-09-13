using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField]
    private GameObject footsteps;

    [SerializeField]
    private GameObject flashlight;

    private int keyNo = 0;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) ||
           Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            keyNo--;
            if(keyNo == 0)
                DisableFootSteps();
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
           Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            EnableFootsteps();
            keyNo++;
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
