using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class RecogerCollar : MonoBehaviour
{
    [Header("Interfaz HUD")]
    public Image hudImageCollar; 
    public Sprite nuevaImagenCollar; 
    public GameObject avisoUI; 
    public float tiempoAviso = 2f;

    [Header("Sonido de Recolección")]
    public AudioClip sonidoDesbloqueo;
    [Range(0f, 1f)]
    public float volumenDesbloqueo = 0.5f;

    [Header("Evento de Nivel (Puerta)")]
    [Tooltip("Arrastra aquí el objeto de la puerta que va a desaparecer")]
    public GameObject puertaADestruir;
    [Tooltip("El sonido de rocas derrumbándose o magia rompiéndose")]
    public AudioClip sonidoPuertaDestruida;

    private bool yaRecogido = false;

    void Start()
    {
        if (avisoUI != null) avisoUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaRecogido)
        {
            yaRecogido = true;
            
            // 1. Reproducir el sonido de la recolección
            if (sonidoDesbloqueo != null)
            {
                AudioSource.PlayClipAtPoint(sonidoDesbloqueo, transform.position, volumenDesbloqueo);
            }

            // 2. Cambiar la imagen del HUD
            if (hudImageCollar != null && nuevaImagenCollar != null)
            {
                hudImageCollar.sprite = nuevaImagenCollar;
            }

            // 3. NUEVO: Eliminar la puerta y reproducir su sonido de destrucción
            if (puertaADestruir != null)
            {
                if (sonidoPuertaDestruida != null)
                {
                    // El sonido se reproduce en las coordenadas de la puerta
                    AudioSource.PlayClipAtPoint(sonidoPuertaDestruida, puertaADestruir.transform.position);
                }
                
                Destroy(puertaADestruir); // Elimina la puerta de la escena
                Debug.Log("🚪 ¡Puerta destruida tras recoger el objeto!");
            }

            // 4. Iniciar la secuencia para ocultar este objeto y mostrar el aviso
            StartCoroutine(SecuenciaRecoleccion());
        }
    }

    IEnumerator SecuenciaRecoleccion()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        if (avisoUI != null) avisoUI.SetActive(true);

        yield return new WaitForSeconds(tiempoAviso);

        if (avisoUI != null) avisoUI.SetActive(false);

        Destroy(gameObject);
    }
}