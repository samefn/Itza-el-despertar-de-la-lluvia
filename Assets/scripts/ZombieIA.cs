using UnityEngine;
using UnityEngine.AI;
using System.Collections; // Necesario para el retraso del golpe

public class ZombieIA : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador; 

    [Header("Ataque")]
    public float distanciaAtaque = 2f; 
    public float tiempoEntreAtaques = 2f; 
    public int danoZarpazo = 20; // NUEVO: Cuánto daño hace el zombie
    public float retrasoDano = 0.5f; // NUEVO: Tiempo que tarda la mano en impactar desde que inicia la animación

    private NavMeshAgent agente;
    private Animator animator;
    private float proximoAtaque = 0f;
    private bool persiguiendo = false;
    private SaludJugador saludJugadorScript; // NUEVO: Referencia a la salud del jugador

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Buscamos el script de salud en el jugador
        if (jugador != null)
        {
            saludJugadorScript = jugador.GetComponent<SaludJugador>();
        }
    }

    public void ActivarPersecucion()
    {
        persiguiendo = true;
    }

    void Update()
    {
        if (!agente.enabled || jugador == null || !persiguiendo) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaAtaque)
        {
            agente.isStopped = true; 

            Vector3 direccion = (jugador.position - transform.position).normalized;
            direccion.y = 0; 
            transform.rotation = Quaternion.LookRotation(direccion);

            if (Time.time >= proximoAtaque)
            {
                animator.SetTrigger("Atacar");
                proximoAtaque = Time.time + tiempoEntreAtaques;

                // NUEVO: Iniciamos la cuenta regresiva para aplicar el daño
                StartCoroutine(ConectarGolpe());
            }
        }
        else
        {
            agente.isStopped = false; 
            agente.SetDestination(jugador.position);
        }
    }

    // NUEVO: Esta corrutina espera medio segundo antes de bajar la vida
    IEnumerator ConectarGolpe()
    {
        yield return new WaitForSeconds(retrasoDano);

        // Verificamos si el jugador sigue vivo y si el zombie no ha sido destruido
        if (saludJugadorScript != null && agente.enabled)
        {
            // Calculamos la distancia de nuevo por si el jugador esquivó el golpe echándose hacia atrás
            float distancia = Vector3.Distance(transform.position, jugador.position);
            if (distancia <= distanciaAtaque + 0.5f) 
            {
                saludJugadorScript.RecibirDano(danoZarpazo);
            }
        }
    }
}