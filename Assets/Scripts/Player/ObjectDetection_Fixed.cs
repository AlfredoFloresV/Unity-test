using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection_Fixed : MonoBehaviour
{
    private LayerMask mask;

    [SerializeField]
    private float detectionDistance = 1.5f;

    [SerializeField]
    private Material btnOrignal;

    [SerializeField]
    private Material btnFocus;

    [SerializeField]
    private Material btnSelected; 

    private Dictionary<Transform, ObjectState> objectStates = new Dictionary<Transform, ObjectState>();

    private enum ObjectState
    {
        Normal,
        Selected,
        Pressed
    }

    void Start()
    {
        mask = LayerMask.GetMask("Object Detection");
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));

        if (Physics.Raycast(ray, out RaycastHit hit, detectionDistance, mask))
        {
            if (hit.collider.tag == "door")
            {
                Transform hitObject = hit.collider.transform;

                if (!objectStates.ContainsKey(hitObject))
                {
                    objectStates.Add(hitObject, ObjectState.Normal);
                }

                ObjectState currentState = objectStates[hitObject];

                switch (currentState)
                {
                    case ObjectState.Normal:
                        hitObject.GetComponent<Renderer>().material = btnFocus;
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            hitObject.GetComponent<OpenDoor>().open();
                            objectStates[hitObject] = ObjectState.Pressed;
                            hitObject.GetComponent<Renderer>().material = btnSelected;
                        }
                        else if (!Input.GetKey(KeyCode.E))
                        {
                            objectStates[hitObject] = ObjectState.Selected;
                        }
                        break;

                    case ObjectState.Selected:
                        hitObject.GetComponent<Renderer>().material = btnFocus;
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            hitObject.GetComponent<OpenDoor>().open();
                            objectStates[hitObject] = ObjectState.Pressed;
                            hitObject.GetComponent<Renderer>().material = btnSelected;
                        }
                        break;

                    case ObjectState.Pressed:
                        hitObject.GetComponent<Renderer>().material = btnSelected;
                        break;
                }
            }
        }
        else
        {
            foreach (var kvp in objectStates)
            {
                Transform obj = kvp.Key;
                ObjectState objState = kvp.Value;

                if (objState != ObjectState.Pressed)
                {
                    Renderer objRenderer = obj.GetComponent<Renderer>();
                    if (objRenderer != null)
                    {
                        objRenderer.material = btnOrignal;
                        objectStates[obj] = ObjectState.Normal;
                    }
                }
            }
        }
    }
}
