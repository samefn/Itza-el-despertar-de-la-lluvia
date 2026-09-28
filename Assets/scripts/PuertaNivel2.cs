using UnityEngine;
using System.Collections;

public class PuertaNivel2 : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject piedraVisual; 
    public Transform puertaModelo; 

    [Header("Configuración de Apertura")]
    public Vector3 desplazamientoAbierto = new Vector3(0, -5f, 0); 
    public float velocidadApertura = 2f;
    
    [Header("Requisitos para Abrir")]
    public int gemasNecesarias = 2; // NUEVO: Cantidad de gemas que exige la puerta

    [Header("Sonido")]
    public AudioSource audioFuentePuerta;
    public AudioClip sonidoDeslizamiento;

    private bool yaAbierta = false;
    private Vector3 posicionInicial;
    private ScoreManager scoreManager;

    void Start()
    {
        posicionInicial = puertaModelo.position;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (yaAbierta || !other.CompareTag("Player")) return;

        if (scoreManager != null)
        {
            // CONDICIÓN 1: Tener la baldosa
            bool tienePiedra = scoreManager.TienePiedra();
            
            // CONDICIÓN 2: Tener las gemas necesarias
            // IMPORTANTE: Debes cambiar "cantidadGemas" por el nombre exacto 
            // de la variable pública que usas en tu ScoreManager.cs
            bool tieneGemas = scoreManager.cantidadGemas >= gemasNecesarias; 

            if (tienePiedra && tieneGemas)
            {
                scoreManager.UsarPiedra(); // Gastamos la piedra
                AbrirPuerta();
            }
            else
            {
                // Mensajes de diagnóstico en consola
                if (!tienePiedra) Debug.Log("Falta la baldosa de Jade para abrir esta puerta.");
                if (!tieneGemas) Debug.Log("Faltan gemas. Se necesitan " + gemasNecesarias);
            }
        }
    }

    void AbrirPuerta()
    {
        yaAbierta = true;

        if (piedraVisual != null) piedraVisual.SetActive(true);

        if (audioFuentePuerta != null && sonidoDeslizamiento != null)
        {
            audioFuentePuerta.PlayOneShot(sonidoDeslizamiento);
        }

        StartCoroutine(AnimarPuerta());
    }

    IEnumerator AnimarPuerta()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 posicionFinal = posicionInicial + desplazamientoAbierto;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * velocidadApertura;
            puertaModelo.position = Vector3.Lerp(posicionInicial, posicionFinal, t);
            yield return null;
        }
    }
}