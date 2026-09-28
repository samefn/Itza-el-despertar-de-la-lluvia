using UnityEngine;
using UnityEngine.AI;

public class EnemigoJaguar : MonoBehaviour
{
    [Header("Salud del Jaguar")]
    public int vidaMaxima = 50;
    private int vidaActual;
    private bool estaMuerto = false;

    [Header("Recompensas (Loot)")]
    [Tooltip("Arrastra aquí los objetos (Prefabs) que quieres que suelte al morir")]
    public GameObject[] objetosADropear;

    [Header("Configuración del Jaguar")]
    public Transform objetivo;
    public float distanciaParaAtacar = 2.0f;
    public int danoAtaque = 15;
    public float tiempoEntreAtaques = 1.5f;

    [Header("Sonidos del Jaguar")]
    public AudioSource audioFuente; 
    public AudioClip sonidoCaminar; 
    public AudioClip sonidoAtacar;  

    private NavMeshAgent agente;
    private Animator animator;
    private bool estaActivo = false;
    private float temporizadorAtaque;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        vidaActual = vidaMaxima; 

        if (objetivo == null)
        {
            GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
            if (jugadorObj != null) objetivo = jugadorObj.transform;
        }

        // NUEVO: Configuramos el AudioSource para los pasos
        if (audioFuente != null && sonidoCaminar != null)
        {
            audioFuente.clip = sonidoCaminar;
            audioFuente.loop = true; // Para que el sonido de caminar se repita
        }
    }

    public void Despertar()
    {
        if (!estaMuerto) estaActivo = true;
    }

    public void RecibirDano(int cantidadDano)
    {
        if (estaMuerto) return;

        vidaActual -= cantidadDano;
        Debug.Log("🟢 ¡ZARPAZO AL JAGUAR! Recibió " + cantidadDano + " de daño. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        estaMuerto = true;
        estaActivo = false;

        // NUEVO: Silenciamos al jaguar al morir
        if (audioFuente != null) audioFuente.Stop();

        Debug.Log("¡Jaguar derrotado! Eliminándolo de la escena...");

        if (objetosADropear != null && objetosADropear.Length > 0)
        {
            foreach (GameObject objeto in objetosADropear)
            {
                if (objeto != null)
                {
                    Vector3 posicionDrop = transform.position + new Vector3(0, 0.5f, 0);
                    Instantiate(objeto, posicionDrop, Quaternion.identity);
                }
            }
        }
        Destroy(gameObject);
    }

    void Update()
    {
        // Si no está activo, detenemos el audio por seguridad y salimos
        if (!estaActivo || objetivo == null || estaMuerto)
        {
            if (audioFuente != null && audioFuente.isPlaying) audioFuente.Pause();
            return;
        }

        float distanciaActual = Vector3.Distance(transform.position, objetivo.position);

        if (animator != null)
        {
            animator.SetFloat("dist", distanciaActual);
        }

        if (distanciaActual <= distanciaParaAtacar)
        {
            agente.isStopped = true;

            // NUEVO: Como se detuvo para atacar, pausamos el sonido de pasos
            if (audioFuente != null && audioFuente.isPlaying)
            {
                audioFuente.Pause();
            }

            if (Time.time >= temporizadorAtaque)
            {
                Atacar();
                temporizadorAtaque = Time.time + tiempoEntreAtaques;
            }
        }
        else
        {
            agente.isStopped = false;
            agente.SetDestination(objetivo.position);

            // NUEVO: Como se está moviendo, reproducimos el sonido de pasos
            if (audioFuente != null && !audioFuente.isPlaying && sonidoCaminar != null)
            {
                audioFuente.Play();
            }
        }
    }

    void Atacar()
    {
        SaludJugador saludJugador = objetivo.GetComponent<SaludJugador>();
        if (saludJugador != null)
        {
            saludJugador.RecibirDano(danoAtaque, transform);
            
            // Reproducimos el zarpazo o rugido por encima de cualquier otro sonido
            if (audioFuente != null && sonidoAtacar != null)
            {
                audioFuente.PlayOneShot(sonidoAtacar);
            }
        }
    }
}