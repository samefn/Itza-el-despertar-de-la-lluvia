using UnityEngine;
using UnityEngine.UI; // Necesario para la imagen del HUD
using UnityEngine.SceneManagement; // Necesario para cambiar de escena
using System.Collections;

public class SubirObjeto : MonoBehaviour
{ 
    [Header("Movimiento")]
    public Transform puntoObjetivo;
    public float velocidad = 3f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonido;

    [Header("Diálogo")]
    public Dialogo2 dialogoUI; 
    [TextArea(2,5)]
    public string[] misDialogos;

    [Header("Configuración del Fade (HUD)")]
    public Image imagenNegra; // Arrastra aquí la imagen negra de tu Canvas
    public float velocidadFade = 0.5f; // Más bajo = más lento
    public string nombreEscenaSiguiente;

    private bool yaSono = false;
    private bool yaLlego = false;

    void Update()
    {
        if (puntoObjetivo == null) return;

        // Sonido una sola vez
        if (!yaSono)
        {
            if (audioSource != null && sonido != null)
            {
                audioSource.PlayOneShot(sonido);
                yaSono = true;
            }
        }

        // Movimiento hacia el objetivo
        transform.position = Vector3.MoveTowards(
            transform.position,
            puntoObjetivo.position,
            velocidad * Time.deltaTime
        );

        // Detectar si llegó al destino
        if (!yaLlego && Vector3.Distance(transform.position, puntoObjetivo.position) < 0.01f)
        {
            yaLlego = true;

            // Activar diálogo pasando 'this' como cuarto parámetro
            if (dialogoUI != null)
            {
                // IniciarDialogo(dialogos, guardian, destino, scriptSubir)
                dialogoUI.IniciarDialogo(misDialogos, null, null, this);
            }
        }
    }

    // Esta función la llamará Dialogo2 cuando se presione 'F' en la última frase
    public void IniciarTransicionEscena()
    {
        StartCoroutine(FadeNegroYCambiarEscena());
    }

    IEnumerator FadeNegroYCambiarEscena()
    {
        if (imagenNegra == null)
        {
            Debug.LogWarning("No asignaste la imagen negra en el inspector. Cambiando de escena directo.");
            SceneManager.LoadScene(nombreEscenaSiguiente);
            yield break;
        }

        // Aseguramos que la imagen esté activa
        imagenNegra.gameObject.SetActive(true);
        
        Color colorActual = imagenNegra.color;
        float alpha = 0;

        // Bucle para subir el Alpha lentamente
        while (alpha < 1)
        {
            alpha += Time.deltaTime * velocidadFade;
            colorActual.a = alpha;
            imagenNegra.color = colorActual;
            yield return null; // Espera al siguiente frame
        }

        // Cuando la pantalla está negra, cargamos la escena
        SceneManager.LoadScene(nombreEscenaSiguiente);
    }
}