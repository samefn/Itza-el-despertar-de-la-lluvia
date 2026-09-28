using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaludJugador : MonoBehaviour
{
    [Header("Animación")]
    public Animator animatorCuerpo;

    [Header("Estadísticas")]
    public int vidaMaxima = 100;
    private int vidaActual;

    [Header("Curación")]
    public int curacionPorMaiz = 30;
    private ScoreManager scoreManager;

    [Header("Interfaz (UI)")]
    public Image barraSalud;

    [Header("Interfaz de Muerte")]
    public GameObject panelMuerte;
    public GameObject HUDPrincipal;

    [Header("Sonidos")]
    public AudioClip sonidoCuracion;
    public AudioClip sonidoBloqueoEscudo;

    [Header("Sonidos de Voz (Daño)")]
    public AudioSource audioFuenteVoz;
    public AudioClip grunt1;
    public AudioClip grunt2;
    public AudioClip grunt3;
    private int contadorGrunt = 1;

    [Header("Sonidos de Voz (Muerte)")]
    public AudioClip sonidoMuerte;
    private bool yaMurio = false;
    [HideInInspector] public bool estaQuejandose = false;
    private Coroutine rutinaQuejaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarBarra();
        scoreManager = Object.FindFirstObjectByType<ScoreManager>();
    }

    // NUEVO: Añadimos 'Transform atacante = null' para saber de dónde viene el golpe
    public void RecibirDano(int cantidad, Transform atacante = null)
    {
        if (yaMurio) return;

        // --- INICIO SISTEMA DE BLOQUEO FRONTAL ---
        EscudoJugador escudo = GetComponent<EscudoJugador>();

        // Si tenemos el escudo activo y sabemos quién nos ataca...
        if (escudo != null && escudo.estaBloqueando && atacante != null)
        {
            // Calculamos la dirección y el ángulo del atacante
            Vector3 direccionAlAtacante = (atacante.position - transform.position).normalized;
            float angulo = Vector3.Angle(transform.forward, direccionAlAtacante);

            // Si está dentro de nuestro cono frontal de 120° (60° a cada lado)
            if (angulo <= 60f)
            {
                Debug.Log("🛡️ ¡Bloqueo exitoso! El escudo absorbió el daño frontal.");

                // NUEVO: Reproducimos el sonido de choque de frente
                if (sonidoBloqueoEscudo != null)
                {
                    AudioSource.PlayClipAtPoint(sonidoBloqueoEscudo, transform.position);
                }

                return; // Cancelamos el resto de la función para no perder vida
            }
            else
            {
                Debug.Log("⚠️ ¡Ataque por el flanco o la espalda! El escudo no te protegió.");
            }
        }
        // --- FIN SISTEMA DE BLOQUEO FRONTAL ---

        // Si no bloqueamos, recibimos el daño normalmente
        vidaActual -= cantidad;
        ActualizarBarra();

        if (vidaActual > 0 && audioFuenteVoz != null)
        {
            AudioClip gruntElegido = null;
            if (contadorGrunt == 1) gruntElegido = grunt1;
            else if (contadorGrunt == 2) gruntElegido = grunt2;
            else if (contadorGrunt == 3) gruntElegido = grunt3;

            if (gruntElegido != null)
            {
                if (rutinaQuejaActual != null) StopCoroutine(rutinaQuejaActual);
                rutinaQuejaActual = StartCoroutine(ReproducirQueja(gruntElegido));
            }

            contadorGrunt++;
            if (contadorGrunt > 3) contadorGrunt = 1;
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private IEnumerator ReproducirQueja(AudioClip clip)
    {
        estaQuejandose = true;

        audioFuenteVoz.Stop();
        audioFuenteVoz.pitch = 1f;
        audioFuenteVoz.PlayOneShot(clip);

        yield return new WaitForSeconds(clip.length);

        estaQuejandose = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) IntentarCurarse();
    }

    void IntentarCurarse()
    {
        if (vidaActual < vidaMaxima)
        {
            if (scoreManager != null && scoreManager.UsarMaiz() == true)
            {
                vidaActual += curacionPorMaiz;
                if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;

                if (sonidoCuracion != null) AudioSource.PlayClipAtPoint(sonidoCuracion, transform.position);
                ActualizarBarra();
            }
        }
    }

    void ActualizarBarra()
    {
        if (barraSalud != null)
        {
            float porcentajeVida = (float)vidaActual / vidaMaxima;
            barraSalud.fillAmount = porcentajeVida;

            if (porcentajeVida > 0.5f) barraSalud.color = Color.green;
            else if (porcentajeVida > 0.2f) barraSalud.color = Color.yellow;
            else barraSalud.color = Color.red;
        }
    }

    void Morir()
    {
        if (yaMurio) return;
        yaMurio = true;

        Debug.Log("¡El jugador fue derrotado!");

        StartCoroutine(SecuenciaMuerte());
    }

    private IEnumerator SecuenciaMuerte()
    {
        if (audioFuenteVoz != null && sonidoMuerte != null)
        {
            audioFuenteVoz.Stop();
            audioFuenteVoz.pitch = 1f;
            audioFuenteVoz.PlayOneShot(sonidoMuerte);
        }

        if (animatorCuerpo != null)
        {
            animatorCuerpo.SetTrigger("Morir");
        }

        fps scriptMovimiento = GetComponent<fps>();
        if (scriptMovimiento != null) scriptMovimiento.enabled = false;

        AtaqueJugador scriptAtaque = GetComponent<AtaqueJugador>();
        if (scriptAtaque != null) scriptAtaque.enabled = false;

        // Opcional: También podrías desactivar el escudo al morir para que no siga bloqueando
        EscudoJugador scriptEscudo = GetComponent<EscudoJugador>();
        if (scriptEscudo != null) scriptEscudo.enabled = false;

        yield return new WaitForSeconds(4.0f);

        if (panelMuerte != null)
        {
            panelMuerte.SetActive(true);

            if (HUDPrincipal != null) HUDPrincipal.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}