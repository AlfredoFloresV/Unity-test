using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public GameObject pivotPoint; // The GameObject to rotate around
    public float spinSpeed = 60.0f; // Speed of rotation in degrees per second
    public bool reverseSpin = false; // Set to true to reverse the spin direction

    void Update()
    {
        if (pivotPoint != null)
        {
            // Calculate the rotation axis relative to the pivot point
            Vector3 pivotToGameObject = transform.position - pivotPoint.transform.position;
            Vector3 rotationAxis = transform.right; // Use local right axis for X-axis rotation

            // Determine the rotation direction based on the reverseSpin boolean
            float direction = reverseSpin ? -1.0f : 1.0f;

            // Rotate the GameObject around the pivot point with the specified direction
            transform.RotateAround(pivotPoint.transform.position, rotationAxis, direction * spinSpeed * Time.deltaTime);
        }
    }
}
