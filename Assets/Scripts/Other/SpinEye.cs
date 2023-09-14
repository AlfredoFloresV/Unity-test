using UnityEngine;

public class SpinEye: MonoBehaviour
{
    public GameObject pivotPoint;
    public float spinSpeed = 60.0f;
    public bool reverseSpin = false;

    void Update()
    {
        if (pivotPoint != null)
        {
            Vector3 pivotToGameObject = transform.position - pivotPoint.transform.position;
            Vector3 rotationAxis = transform.forward;

            float direction = reverseSpin ? -1.0f : 1.0f;

            transform.RotateAround(pivotPoint.transform.position, rotationAxis, direction * spinSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Pivot point not assigned. Please assign a GameObject to the 'Pivot Point' field in the Inspector.");
        }
    }
}
