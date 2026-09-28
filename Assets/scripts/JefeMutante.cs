using UnityEngine;
using UnityEngine.AI;
using System.Collections; // Necesario para las Corrutinas

public class JefeMutante : MonoBehaviour
{
    [Header("Recompensas (Loot)")]
    [Tooltip("Arrastra aquí los objetos (Prefabs) que quieres que suelte al morir")]
    public GameObject[] objetosADropear;

    [Header("Estadísticas del Jefe")]
    public int vidaMaxima = 150;
    private int vidaActual;
    public int danoAtaque = 25;

    [Header("Combate")]
    public Transform objetivo;
    public float distanciaParaAtacar = 2.5f;
    public float tiempoEntreAtaques = 2.0f;
    [Tooltip("Tiempo exacto en el que el zarpazo conecta con el jugador (en segundos)")]
    public float retrasoDano = 0.8f; // NUEVO: Ajusta esto según tu animación

    [Header("Sonidos")]
    public AudioSource audioFuente;
    public AudioClip sonidoCaminar;
    public AudioClip sonidoAtacar;
    public AudioClip sonidoMuerte;

    private NavMeshAgent agente;
    private Animator animator;
    private bool estaMuerto = false;
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

        if (audioFuente != null && sonidoCaminar != null)
        {
            audioFuente.clip = sonidoCaminar;
            audioFuente.loop = true;
            audioFuente.Play();
        }
    }

    void Update()
    {
        if (estaMuerto || objetivo == null) return;

        float distanciaActual = Vector3.Distance(transform.position, objetivo.position);

        if (distanciaActual <= distanciaParaAtacar)
        {
            agente.isStopped = true;

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
        }
    }

    void Atacar()
    {
        // Mirar a Soraya
        transform.LookAt(new Vector3(objetivo.position.x, transform.position.y, objetivo.position.z));

        // Disparar animación (RECUERDA USAR LA MAYÚSCULA/MINÚSCULA QUE ELEGISTE ANTES)
        if (animator != null) animator.SetTrigger("Atacar");

        if (audioFuente != null && sonidoAtacar != null) audioFuente.PlayOneShot(sonidoAtacar);

        // NUEVO: En lugar de hacer daño inmediato, iniciamos la cuenta regresiva
        StartCoroutine(DarGolpeConRetraso());
    }

    // NUEVA FUNCIÓN: Espera a que termine la animación para aplicar el daño
    IEnumerator DarGolpeConRetraso()
    {
        // 1. Esperamos los segundos que tarda la mano en bajar
        yield return new WaitForSeconds(retrasoDano);

        // 2. Si el jefe murió justo a mitad de su propio ataque, cancelamos todo
        if (estaMuerto || objetivo == null) yield break;

        // 3. Revisamos si Soraya sigue cerca o si logró esquivar hacia atrás
        float distanciaActual = Vector3.Distance(transform.position, objetivo.position);

        // Le damos 0.5f de margen extra para que no sea injusto
        if (distanciaActual <= distanciaParaAtacar + 0.5f)
        {
            SaludJugador saludJugador = objetivo.GetComponent<SaludJugador>();
            if (saludJugador != null)
            {
                saludJugador.RecibirDano(danoAtaque, transform);
            }
        }
        else
        {
            Debug.Log("💨 ¡Soraya esquivó el ataque del Jefe!");
        }
    }

    public void RecibirDano(int cantidadDano)
    {
        if (estaMuerto) return;

        vidaActual -= cantidadDano;
        Debug.Log("💥 Jefe recibió " + cantidadDano + " de daño. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        estaMuerto = true;

        if (agente != null) agente.isStopped = true;
        GetComponent<Collider>().enabled = false;

        if (audioFuente != null)
        {
            audioFuente.Stop();
            if (sonidoMuerte != null) audioFuente.PlayOneShot(sonidoMuerte);
        }

        if (animator != null) animator.SetTrigger("Morir");

        Debug.Log("👑 ¡El Jefe Mutante ha sido derrotado!");

        if (objetosADropear != null && objetosADropear.Length > 0)
        {
            foreach (GameObject objeto in objetosADropear)
            {
                if (objeto != null)
                {
                    // Le sumamos 0.5f en Y para que caigan desde el pecho del jefe
                    // Y le ponemos un pequeño desfase aleatorio para que no caigan uno dentro del otro
                    float desfaseX = Random.Range(-1f, 1f);
                    float desfaseZ = Random.Range(-1f, 1f);
                    Vector3 posicionDrop = transform.position + new Vector3(desfaseX, 1.5f, desfaseZ);

                    Instantiate(objeto, posicionDrop, Quaternion.identity);
                }
            }
        }

        Destroy(gameObject, 5f);
    }
}