using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -10);

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;

    void Start()
    {
        if (target == null)
        {
            GameObject pivot = new GameObject("Camera Pivot");
            pivot.transform.position = Vector3.zero;
            target = pivot.transform;
        }

        transform.position = target.position + offset;
        transform.LookAt(target);
    }

    void LateUpdate()
    {
        HandleRotation();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseY = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseX = Input.GetAxis("Mouse Y") * rotationSpeed;

            Quaternion camTurnAngle = Quaternion.Euler(mouseX, mouseY, 0);
            offset = camTurnAngle * offset;
        }

        transform.position = target.position + offset;
        transform.LookAt(target);
    }
  
}