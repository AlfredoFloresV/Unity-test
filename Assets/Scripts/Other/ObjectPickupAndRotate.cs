using System.Collections.Generic;
using UnityEngine;

public class ObjectPickupAndRotate : MonoBehaviour
{
    [System.Serializable]
    public struct Collectible
    {
        public string name;
        public string description;
        public GameObject prefab;
    }

    public Collectible[] collectibleList;

    [SerializeField]
    private GameObject pauseObj;

    [SerializeField]
    private GameObject textObj;

    [SerializeField]
    private float distance = 0.1f;

    public bool Freezed;

    private Dictionary<string, Collectible> collectibles;

    private string interactionMessage;
    private string header;
    private float xRotation = 0;
    private float yRotation = 0;
    private GameObject obj;
    private string currname;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Freezed = false;
        collectibles = new Dictionary<string, Collectible>();
        foreach (Collectible c in collectibleList) 
        {
            collectibles[c.name] = c;
        }
    }

    private void Update()
    {
        if (pauseObj.GetComponent<PauseMenu>().isPaused)
            return;

        if (Freezed) 
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                freezeScene(false);
                textObj.GetComponent<TextSupportGUI>().cleanMessages();

                if (currname.Contains("ticket")) 
                {
                    OpenDoors();
                }

                xRotation = 0;
                yRotation = 0;
                if (obj != null) Destroy(obj);
            }
            else 
            {
                freezeScene(true);
                textObj.GetComponent<TextSupportGUI>().setInteractionMessage("Press Q to return", false);
                textObj.GetComponent<TextSupportGUI>().setHeaderMessage("Found <b>" + collectibles[currname].description + "</b>");
            }

            rotateObject();
        }
    }

    private void OpenDoors() 
    {
        MeshRenderer policeTape1 = GameObject.Find("PoliceTapeD1").GetComponent<MeshRenderer>();
        MeshRenderer policeTape2 = GameObject.Find("PoliceTapeD2").GetComponent<MeshRenderer>();
        MeshRenderer policeTape3 = GameObject.Find("PoliceTapeD3").GetComponent<MeshRenderer>();
        AudioSource doorsOpen = GameObject.Find("DoorSound").GetComponent<AudioSource>();

        policeTape1.enabled = false;
        policeTape2.enabled = false;
        policeTape3.enabled = false;
        doorsOpen.Play();
    }

    public void displayObject(string name) 
    {
        freezeScene(true);
        currname = name;
        Vector3 spawnPosition = transform.position + transform.forward * distance; // Adjust the distance as needed
        obj = Instantiate(collectibles[name].prefab, spawnPosition, Quaternion.identity);
        if (name.Contains("key")) 
        {
            obj.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            obj.transform.eulerAngles = obj.transform.eulerAngles + new Vector3(32f,0f,90f);
        }
        textObj.GetComponent<TextSupportGUI>().setInteractionMessage("Press Q to return", false);
        textObj.GetComponent<TextSupportGUI>().setHeaderMessage("Found <b>" + collectibles[name].description + "</b>");
        
    }

    private void rotateObject()
    {
        if (obj != null) 
        {
            float mouseX = Input.GetAxis("Mouse X") * 1.5f;
            float mouseY = Input.GetAxis("Mouse Y") * 1.5f;
            
            xRotation -= mouseY;
            //xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            yRotation -= mouseX;
            //obj.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x + 0f, yRotation, obj.transform.eulerAngles.z + 0f); 
        }
    }

    private void freezeScene(bool freeze) 
    {
        Freezed = freeze;
        Time.timeScale = freeze ? 0f : 1f;
        // Cursor.lockState = freeze ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, Screen.height * 0.08f, Screen.width, Screen.height * 0.15f), "<color=white><size=80>" + header + "</size></color>", style);
        GUI.Label(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height * 0.15f), "<color=white><size=50>" + interactionMessage + "</size></color>", style);
    }
}
