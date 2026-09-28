using UnityEngine;

public class AltarGema : MonoBehaviour
{
    public GuardianMovement guardian;
    public Transform siguientePunto;

    [Header("Gema del altar (mesh)")]
    public GameObject gemaColocada;

    [Header("Partículas (opcional)")]
    public ParticleSystem particulasActivacion;

    [Header("Referencias")]
    public SelectorGemas selector;

    [Header("HUD")]
    public GameObject imagenPonerHUD;
    public GameObject slotHUDGema;

    [Header("Configuración")]
    public int gemaCorrecta = 0;

    [Header("Sonidos 🔊")]
    public AudioSource audioSource;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    private bool jugadorCerca = false;
    private bool completado = false;

    void Update()
    {
        if (!jugadorCerca || completado) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            IntentarColocar();
        }
    }

    void IntentarColocar()
    {
        int gema = selector.ObtenerGemaActual();

        if (gema == -1) return;

        if (gema == gemaCorrecta)
        {
            // 🔊 sonido correcto
            if (audioSource != null && sonidoCorrecto != null)
                audioSource.PlayOneShot(sonidoCorrecto);

            // ✔️ activar mesh del altar
            if (gemaColocada != null)
                gemaColocada.SetActive(true);

            // ✨ partículas
            if (particulasActivacion != null)
                particulasActivacion.Play();

            // ✔️ quitar del jugador
            selector.MarcarGemaUsada(gema);

            // ✔️ mover guardián
            if (guardian != null && siguientePunto != null)
                guardian.MoverANuevoPunto(siguientePunto);

            // ✔️ UI
            if (slotHUDGema != null)
                slotHUDGema.SetActive(false);

            if (imagenPonerHUD != null)
                imagenPonerHUD.SetActive(false);

            completado = true;

            Debug.Log("✅ Gema correcta → guardián avanza");
        }
        else
        {
            // 🔊 sonido incorrecto
            if (audioSource != null && sonidoIncorrecto != null)
                audioSource.PlayOneShot(sonidoIncorrecto);

            Debug.Log("❌ Gema incorrecta");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !completado)
        {
            jugadorCerca = true;

            if (imagenPonerHUD != null)
                imagenPonerHUD.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            if (imagenPonerHUD != null)
                imagenPonerHUD.SetActive(false);
        }
    }
}