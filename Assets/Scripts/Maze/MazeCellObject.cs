using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellObject : MonoBehaviour
{
    [SerializeField] GameObject topWall;
    [SerializeField] GameObject bottomWall;

    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject leftWall;

    [SerializeField] GameObject door;

    public void init(bool top, bool bottom, bool right, bool left)
    {
        topWall.SetActive(top);
        bottomWall.SetActive(bottom);
        rightWall.SetActive(right);
        leftWall.SetActive(left);
    }
    
    /*
    private void Start()
    {
        topWall.SetActive(true);
        bottomWall.SetActive(true);
        leftWall.SetActive(true);
        rightWall.SetActive(true);
        door.SetActive(true);
    }
    */

    public void DisableDoor() 
    {
        if(!door.CompareTag("specialdoor"))
            door.SetActive(false);
    }

    public void placeDoor(string place) 
    {
        switch (place) 
        {
            case "top":
                door.GetComponent<Transform>().localPosition = new Vector3(0f, -0.5f, 0.5f);
                door.GetComponent<Transform>().localEulerAngles = new Vector3(0f, 90f, 0f); 
                break;
            case "bottom":
                door.GetComponent<Transform>().localPosition = new Vector3(0f, -0.5f, -0.5f);
                door.GetComponent<Transform>().localEulerAngles = new Vector3(0f, 90f, 0f);
                break;
            case "left":
                door.GetComponent<Transform>().localPosition = new Vector3(-0.5f, -0.5f, 0f);
                break;
            case "right":
                door.GetComponent<Transform>().localPosition = new Vector3(0.5f, -0.5f, 0f);
                break;
        }

        door.SetActive(true);
    }

    public void DisableRight()
    {
        rightWall.SetActive(false);
    }

    public void DisableLeft()
    {
        leftWall.SetActive(false);
    }

    public void DisableTop()
    {
        topWall.SetActive(false);
    }

    public void DisableBottom() 
    {
        bottomWall.SetActive(false);
    }

    public void EnableRight() 
    {
        rightWall.SetActive(true);
    }

    public void EnableLeft() 
    {
        leftWall.SetActive(true);
    }

    public void EnableTop() 
    {
        topWall.SetActive(true);
    }

    public void EnableBottom() 
    {
        bottomWall.SetActive(true);
    }
}
