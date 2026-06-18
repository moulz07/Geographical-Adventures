using UnityEngine;

public class PlanetCameraController : MonoBehaviour
{
    public Transform target;

    public float distance = 6f;
    public float height = 2f;

    public float mouseSensitivity = 3f;
    public float smoothSpeed = 8f;
    public float minDistance = 3f;
    public float maxDistance = 10f;
    public float zoomSpeed = 5f;

    float yaw = 0;
    float pitch = 15;

    void LateUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        distance -= scroll * zoomSpeed;

        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        if (target == null)
            return;

        Vector3 gravityUp = target.position.normalized;

        // Mouse input
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -30f, 70f);

        Quaternion rotation =
            Quaternion.AngleAxis(yaw, gravityUp);

        Vector3 right =
            rotation * Vector3.Cross(gravityUp, Vector3.forward);

        Quaternion pitchRotation =
            Quaternion.AngleAxis(pitch, right);

        Vector3 offset =
            pitchRotation *
            rotation *
            (-Vector3.forward * distance);

        offset += gravityUp * height;

        Vector3 desiredPosition =
            target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime);

        transform.LookAt(
            target.position + gravityUp,
            gravityUp);
    }
}