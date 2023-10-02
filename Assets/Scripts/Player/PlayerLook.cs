using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 1f;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private GameObject pauseObj;

    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseObj.GetComponent<PauseMenu>().isPaused || GetComponent<ObjectPickupAndRotate>().Freezed)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, yRotation, player.transform.eulerAngles.z);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
