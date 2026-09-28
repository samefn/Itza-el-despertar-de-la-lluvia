using UnityEngine;
using System.Collections;

public class PuertaSecreta : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject piedraVisual; // La piedra que pusimos en el hueco
    public Transform puertaModelo; // El modelo completo de la puerta que se va a mover

    [Header("Configuración de Apertura")]
    public Vector3 desplazamientoAbierto = new Vector3(0, -5f, 0); // Cuánto bajará la puerta
    public float velocidadApertura = 2f;

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
        // Si ya se abrió o no es el jugador, no hacemos nada
        if (yaAbierta || !other.CompareTag("Player")) return;

        // Verificamos si tiene la piedra
        if (scoreManager != null && scoreManager.TienePiedra())
        {
            scoreManager.UsarPiedra(); // Gastamos la piedra
            AbrirPuerta();
        }
        else
        {
            Debug.Log("Necesitas la piedra verde para encajarla aquí.");
        }
    }

    void AbrirPuerta()
    {
        yaAbierta = true;

        if (piedraVisual != null) piedraVisual.SetActive(true);

        // NUEVO: Reproducir el sonido de roca arrastrándose
        if (audioFuentePuerta != null && sonidoDeslizamiento != null)
        {
            audioFuentePuerta.PlayOneShot(sonidoDeslizamiento);
        }

        StartCoroutine(AnimarPuerta());
    }

    IEnumerator AnimarPuerta()
    {
        // Esperamos medio segundo para que el jugador vea que la piedra encajó
        yield return new WaitForSeconds(0.5f);

        Vector3 posicionFinal = posicionInicial + desplazamientoAbierto;
        float t = 0;

        // Movemos la puerta suavemente hacia abajo
        while (t < 1f)
        {
            t += Time.deltaTime * velocidadApertura;
            puertaModelo.position = Vector3.Lerp(posicionInicial, posicionFinal, t);
            yield return null;
        }
    }
}