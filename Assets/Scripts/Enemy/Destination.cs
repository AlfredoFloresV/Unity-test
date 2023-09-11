using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField]
    private Material material1;

    [SerializeField]
    private Material material2;

    [SerializeField]
    private Material material3;

    [SerializeField]
    private GameObject renderElem;


    private void Start()
    {
        int matnum = Random.Range(0, 3);
        List<Material> materials = new List<Material>() { material1, material2, material3 };

        renderElem.GetComponent<Renderer>().material = materials[matnum];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy")) 
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(reEnable());
        }
    }


    IEnumerator reEnable() 
    {
        yield return new WaitForSeconds(60f);
        GetComponent<Collider>().enabled = true;
    }
}
