using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;        // Cible à suivre
    public Vector3 offset = new Vector3(0, 2, 1); // Caméra très proche du joueur
    public float mouseSensitivity = 3f;
    public float zoomSpeed = 2f;
    public float minZoom =40f;
    public float maxZoom = 80f; // Zoom maximal fortement augmenté

    public float minPitch = 0f;
    public float maxPitch = 50f;

    private float distance;
    private float yaw = 0f;
    private float pitch = 20f;

    void Start()
    {
        distance = offset.z; // Utiliser l'offset z comme distance initiale
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        HandleZoom();
        HandleRotation();

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * Vector3.forward; // Utiliser forward pour garder la caméra derrière

        // Positionner la caméra à la bonne distance et hauteur
        transform.position = player.position + Vector3.up * offset.y - direction * distance;
        transform.LookAt(player.position + Vector3.up * offset.y);
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
