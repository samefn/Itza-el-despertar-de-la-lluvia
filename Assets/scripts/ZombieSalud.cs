using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieSalud : MonoBehaviour
{
    [Header("Estadísticas")]
    public int vidaMaxima = 120;
    private int vidaActual;

    [Header("Interfaz")]
    public Slider barraVidaUI;

    [Header("Loot (Objetos a soltar)")]
    public GameObject piedraVerdePrefab;
    public GameObject gemaPrefab;

    void Start()
    {
        vidaActual = vidaMaxima;

        if (barraVidaUI != null)
        {
            barraVidaUI.maxValue = vidaMaxima;
            barraVidaUI.value = vidaActual;
        }
    }

    public void RecibirDano(int cantidadDano)
    {
        vidaActual -= cantidadDano;

        if (barraVidaUI != null)
        {
            barraVidaUI.value = vidaActual;
        }

        Debug.Log("Zombie recibió " + cantidadDano + " de daño. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log("¡El zombie fue derrotado!");

        // 1. Instanciamos (creamos) la piedra verde
        if (piedraVerdePrefab != null)
        {
            // La creamos en la posición del zombie, pero un poco hacia la izquierda y arriba para que no choque con el suelo
            Vector3 posicionPiedra = transform.position + new Vector3(-0.5f, 0.5f, 0f);
            Instantiate(piedraVerdePrefab, posicionPiedra, Quaternion.identity);
        }

        // 2. Instanciamos (creamos) la gema
        if (gemaPrefab != null)
        {
            // La creamos en la posición del zombie, pero un poco hacia la derecha y arriba
            Vector3 posicionGema = transform.position + new Vector3(0.5f, 0.5f, 0f);
            Instantiate(gemaPrefab, posicionGema, Quaternion.identity);
        }

        // Apagamos su IA
        ZombieIA ia = GetComponent<ZombieIA>();
        if (ia != null) ia.enabled = false;

        NavMeshAgent agente = GetComponent<NavMeshAgent>();
        if (agente != null) agente.enabled = false;

        // Destruimos al zombie
        Destroy(gameObject);
    }
}