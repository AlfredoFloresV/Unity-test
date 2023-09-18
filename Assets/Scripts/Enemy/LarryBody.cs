using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    private List<string> attacks;
    private bool waiting;

    private void Start()
    {
        attacks = new List<string>() { "Larry_Attack1", "Larry_Attack2", "Larry_Attack3", "Larry_JumpScare3", "Larry_JumpScare2" };
        waiting = false;

        InvokeRepeating("activeCollider", 1f, 1f);
    }

    private void activeCollider() 
    {
        GetComponent<BoxCollider>().enabled = !GetComponent<BoxCollider>().enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player")) 
        {
            if (waiting == false) 
            {
                enemy.GetComponent<LarryActions>().AttackActions(attacks[Random.Range(0, attacks.Count)]);
                waiting = true;
                StartCoroutine(wait());
            }
            
        }
    }


    IEnumerator wait() 
    {
        yield return new WaitForSeconds(10f);
        waiting = false;
    }
}
