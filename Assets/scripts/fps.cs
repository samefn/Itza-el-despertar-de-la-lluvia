using UnityEngine;

public class fps : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    [Header("Límites de Cámara")]
    public float mirarArribaMax = -90f; // Límite para mirar hacia arriba
    public float mirarAbajoMax = 90f;

    [Header("Sonidos de Voz")]
    public AudioSource audioFuenteVoz;
    public AudioClip salto1;
    public AudioClip salto2;
    public AudioClip salto3;
    private int contadorSalto = 1;

    [Header("Animaciones")]
    // NUEVO: Aquí arrastraremos tu modelo 3D
    public Animator animatorPersonaje;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        if (Checkpoint.hayCheckpointActivo)
        {
            controller.enabled = false;
            transform.position = Checkpoint.posicionGuardada;
            controller.enabled = true;
        }
    }

    void Update()
    {
        Mover();
        Mirar();
    }

    void Mover()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // NUEVO: Calcular la velocidad para la animación Walk/Idle
        if (animatorPersonaje != null)
        {
            // Medimos si hay movimiento en los ejes X y Z
            float magnitudMovimiento = new Vector2(x, z).magnitude;
            animatorPersonaje.SetFloat("Velocidad", magnitudMovimiento);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (audioFuenteVoz != null)
            {
                SaludJugador salud = GetComponent<SaludJugador>();

                // Solo suena el esfuerzo del salto si NO se está quejando
                if (salud == null || !salud.estaQuejandose)
                {
                    if (contadorSalto == 1 && salto1 != null) audioFuenteVoz.PlayOneShot(salto1);
                    else if (contadorSalto == 2 && salto2 != null) audioFuenteVoz.PlayOneShot(salto2);
                    else if (contadorSalto == 3 && salto3 != null) audioFuenteVoz.PlayOneShot(salto3);

                    contadorSalto++;
                    if (contadorSalto > 3) contadorSalto = 1;
                }
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Mirar()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, mirarArribaMax, mirarAbajoMax);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}