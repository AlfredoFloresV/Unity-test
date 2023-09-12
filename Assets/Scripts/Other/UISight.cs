using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISight : MonoBehaviour
{
    [SerializeField]
    public Image guiImage;

    [SerializeField]
    private Material smallPoint;

    [SerializeField]
    private Material bigPoint;

    public void pointing(bool p) 
    {
        if (p == true)
        {
            guiImage.material = bigPoint;
        }
        else 
        {
            guiImage.material = smallPoint;
        }
    }
}
