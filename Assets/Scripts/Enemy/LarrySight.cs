using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarrySight : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    private LarryAI script;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player")) 
        {
            script = enemy.GetComponent<LarryAI>();
            script.chasing = true;
            script.walking = false;
            script.idle = false;
            StopCoroutine(script.nextDestination());
            StopCoroutine(script.chase());
            StartCoroutine(script.chase());
        }

        if (other.gameObject.CompareTag("destination"))
        {
            script = enemy.GetComponent<LarryAI>();
            if (script.chasing == false)
            {
                script.walking = false;
                script.idle = true;
                enemy.GetComponent<Animator>().Play("Larry_Celebration1");
            }
        }
    }
}
