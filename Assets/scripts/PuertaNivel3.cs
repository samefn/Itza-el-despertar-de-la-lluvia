using UnityEngine;
using System.Collections;

public class PuertaNivel3 : MonoBehaviour
{
    [Header("Referencias de la Puerta")]
    public GameObject emblemaVisual; 
    public Transform modeloPuerta;   

    [Header("Efectos Visuales")]
    [Tooltip("Arrastra aquí el Particle System del humo")]
    public ParticleSystem particulasHumo; // NUEVO: Referencia al humo

    [Header("Configuración de Apertura")]
    public Vector3 desplazamientoAbierto = new Vector3(0, -5f, 0); 
    public float velocidadApertura = 2f;
    
    [Header("Requisitos Fijos")]
    public int gemasNecesarias = 3; 

    [Header("Sonidos")]
    public AudioSource audioFuentePuerta;
    public AudioClip sonidoAbriendo;

    private bool yaAbierta = false;
    private Vector3 posicionInicial;
    private ScoreManager scoreManager;

    void Start()
    {
        posicionInicial = modeloPuerta.position;
        scoreManager = FindObjectOfType<ScoreManager>();
        
        // Nos aseguramos de que el humo esté apagado al inicio
        if (particulasHumo != null) particulasHumo.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (yaAbierta || !other.CompareTag("Player")) return;

        if (scoreManager != null)
        {
            bool tieneLaBaldosa = scoreManager.TienePiedra();
            bool tieneLasGemas = scoreManager.cantidadGemas >= gemasNecesarias; 

            if (tieneLaBaldosa && tieneLasGemas)
            {
                scoreManager.UsarPiedra(); 
                AbrirPuerta();
            }
        }
    }

    void AbrirPuerta()
    {
        yaAbierta = true;

        if (emblemaVisual != null) emblemaVisual.SetActive(true);

        // NUEVO: Activar el humo justo cuando se activa el mecanismo
        if (particulasHumo != null) 
        {
            particulasHumo.Play();
        }

        if (audioFuentePuerta != null && sonidoAbriendo != null)
        {
            audioFuentePuerta.PlayOneShot(sonidoAbriendo);
        }

        StartCoroutine(MoverPuerta());
    }

    IEnumerator MoverPuerta()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 posicionFinal = posicionInicial + desplazamientoAbierto;
        float tiempo = 0;

        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidadApertura;
            modeloPuerta.position = Vector3.Lerp(posicionInicial, posicionFinal, tiempo);
            yield return null;
        }
        
        // (Opcional) Detener el humo cuando la puerta termine de moverse
        if (particulasHumo != null) particulasHumo.Stop();
    }
}