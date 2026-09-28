using UnityEngine;
using System.Collections;

public class TrampaFuego : MonoBehaviour
{
    [Header("Configuración de Daño")]
    public int danoFuego = 15;
    public float tiempoEntreDano = 1f; 

    [Header("Secuencia (Tiempos en segundos)")]
    public float tiempoEncendido = 2f;
    public float tiempoApagado = 2f;
    public float tiempoDesfase = 0f;

    [Header("Referencias")]
    public ParticleSystem[] sistemasDeParticulas; 
    public Collider hitboxDano; 
    
    [Header("Sonido del Fuego")]
    [Tooltip("El componente AudioSource pegado a esta trampa")]
    public AudioSource audioFuenteFuego; // NUEVO

    private bool estaEncendida = false;
    private float proximoDanoTiempo = 0f; 

    void Start()
    {
        ApagarFuego();
        StartCoroutine(CicloDeFuego());
    }

    IEnumerator CicloDeFuego()
    {
        if (tiempoDesfase > 0)
        {
            yield return new WaitForSeconds(tiempoDesfase);
        }

        while (true)
        {
            EncenderFuego();
            yield return new WaitForSeconds(tiempoEncendido);

            ApagarFuego();
            yield return new WaitForSeconds(tiempoApagado);
        }
    }

    void EncenderFuego()
    {
        estaEncendida = true;
        if (hitboxDano != null) hitboxDano.enabled = true;
        
        foreach (ParticleSystem ps in sistemasDeParticulas)
        {
            if (ps != null) ps.Play();
        }

        // NUEVO: Reproducimos el sonido del fuego
        if (audioFuenteFuego != null)
        {
            audioFuenteFuego.Play();
        }
    }

    void ApagarFuego()
    {
        estaEncendida = false;
        if (hitboxDano != null) hitboxDano.enabled = false;
        
        foreach (ParticleSystem ps in sistemasDeParticulas)
        {
            if (ps != null) ps.Stop();
        }

        // NUEVO: Detenemos el sonido del fuego de golpe
        if (audioFuenteFuego != null)
        {
            audioFuenteFuego.Stop();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (estaEncendida && other.CompareTag("Player") && Time.time >= proximoDanoTiempo)
        {
            SaludJugador salud = other.GetComponent<SaludJugador>();
            if (salud != null)
            {
                salud.RecibirDano(danoFuego, transform);
                proximoDanoTiempo = Time.time + tiempoEntreDano;
            }
        }
    }
}