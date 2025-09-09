using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;        // Cible à suivre
    public Vector3 offset = new Vector3(0, 2, 5);
    public float mouseSensitivity = 3f;
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    public float minPitch = 0f;
    public float maxPitch = 50f;

    private float distance;
    private float yaw = 0f;
    private float pitch = 20f;

    void Start()
    {
        distance = offset.magnitude;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        HandleZoom();
        HandleRotation();

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * Vector3.back * distance;

        transform.position = player.position + direction;
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance - scroll * zoomSpeed, minZoom, maxZoom);
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * mouseSensitivity, minPitch, maxPitch);
        }
    }
}
