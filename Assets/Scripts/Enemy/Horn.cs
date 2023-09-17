using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour
{
    private GameObject[] enemies;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player")) 
        {
            Debug.Log("horn pressed");
            GameObject enemy = Random.Range(0,2) == 0 ? enemies[0] : enemies[1];
            enemy.GetComponent<LarryActions>().currentState = LarryState.Horn;
            enemy.GetComponent<LarryActions>().HornActions(transform.position);
        }
    }
}
