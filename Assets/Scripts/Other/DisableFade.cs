using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFade : MonoBehaviour
{
    [SerializeField]
    private GameObject fade;

    private void Start()
    {
        StartCoroutine(disable());
    }

    IEnumerator disable() 
    {
        yield return new WaitForSeconds(2f);
        fade.SetActive(false);
    }
}
