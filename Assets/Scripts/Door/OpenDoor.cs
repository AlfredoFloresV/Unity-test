using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject doorFrame;

    [SerializeField]
    private GameObject door;

    [SerializeField]
    private AudioClip doorAudio;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void open() 
    {
        door.GetComponent<Animator>().Play("OpenDoor");
        audioSource.PlayOneShot(doorAudio);

        
    }

    
}
