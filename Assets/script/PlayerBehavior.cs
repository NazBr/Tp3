using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Refs")]
    public CameraController cameraController; // l'empty qui pilote la caméra (facultatif)
    private Camera mainCam;

    [Header("Mvt & Saut")]
    public float speed = 5f;
    public float jumpforce = 5f;

    private Animator animator;
    private Rigidbody rb;
    private bool grounded = true;

    // Buffer d'inputs pour FixedUpdate
    private Vector3 _moveInput = Vector3.zero;
    private bool _jumpQueued = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main; // on tente de récupérer la vraie caméra
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // --- Récupération des inputs clavier ---
        float h = Input.GetAxis("Horizontal"); // A/D ou Q/D
        float v = Input.GetAxis("Vertical");   // W/S ou Z/S

        // Par défaut : déplacement dans le repère monde
        Vector3 moveWorld = new Vector3(h, 0f, v);

        // Si clic droit maintenu -> interpréter ZQSD dans l'orientation caméra
        if (Input.GetMouseButton(1))
        {
            Transform camT = mainCam ? mainCam.transform :
                              (cameraController ? cameraController.transform : null);

            if (camT != null)
            {
                // Avant "plan" = direction à partir du joueur, s'éloignant de la caméra (cam -> player)
                Vector3 camPlanarForward = (transform.position - camT.position);
                camPlanarForward.y = 0f;
                if (camPlanarForward.sqrMagnitude > 0.0001f) camPlanarForward.Normalize();

                // Droite = up x forward (donne bien la droite dans Unity)
                Vector3 camPlanarRight = Vector3.Cross(Vector3.up, camPlanarForward);
                if (camPlanarRight.sqrMagnitude > 0.0001f) camPlanarRight.Normalize();

                _moveInput = camPlanarForward * v + camPlanarRight * h;
            }
            else
            {
                // fallback si pas de caméra trouvée
                _moveInput = moveWorld;
            }
        }
        else
        {
            // Pas de clic droit : repère monde
            _moveInput = moveWorld;
        }

        // Clamp pour éviter de dépasser 1 en diagonale
        if (_moveInput.sqrMagnitude > 1f) _moveInput.Normalize();

        // Saut (on file l'info à FixedUpdate pour un timing physique propre)
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            _jumpQueued = true;
            animator.SetBool("isJumping", true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            animator.SetTrigger("doAttack");
            
        }
        // --- Animation ---
        if (_moveInput != Vector3.zero)
        {
            animator.SetBool("isRunning", true);

            // Si clic droit ET avance (v > 0), orienter le joueur vers la direction caméra
            if (Input.GetMouseButton(1) && Input.GetAxis("Vertical") > 0.01f)
            {
                Transform camT = mainCam ? mainCam.transform :
                                  (cameraController ? cameraController.transform : null);

                if (camT != null)
                {
                    Vector3 camPlanarForward = (transform.position - camT.position);
                    camPlanarForward.y = 0f;
                    if (camPlanarForward.sqrMagnitude > 0.0001f) camPlanarForward.Normalize();

                    Quaternion targetRot = Quaternion.LookRotation(camPlanarForward, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.1f);
                }
            }
            else
            {
                // Sinon, orientation selon le déplacement dans le repère monde
                Quaternion targetRot = Quaternion.LookRotation(_moveInput, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.1f);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void FixedUpdate()
    {
        // Mouvement physique propre avec MovePosition
        if (_moveInput.sqrMagnitude > 0.0001f)
        {
            Vector3 targetPos = rb.position + _moveInput * speed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
        }

        if (_jumpQueued && grounded)
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            grounded = false;
            _jumpQueued = false;
            animator.SetBool("isJumping", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }  
}
