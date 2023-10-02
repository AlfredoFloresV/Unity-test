using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private AudioSource audioSource;
    private PlayerMotor pm;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("updateSound", 10.0f, 3.0f);
    }

    private void updateSound() 
    {
        pm = player.GetComponent<PlayerMotor>();
        audioSource.volume = 1.0f - (float) pm.Health / 5.0f;
    }



}
