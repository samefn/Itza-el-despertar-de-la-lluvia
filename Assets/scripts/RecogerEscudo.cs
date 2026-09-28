using UnityEngine;
using UnityEngine.UI;

public class RecogerEscudo : MonoBehaviour
{
    [Header("Interfaz HUD")]
    public Image hudImageEscudo; // Arrastra aquí la imagen de la letra Q del HUD
    public Sprite nuevaImagenEscudo; // Arrastra aquí la versión a color
    public GameObject avisoUI; // (Opcional) Texto de "Poder desbloqueado"
    public float tiempoAviso = 2f;

    [Header("Sonido")]
    public AudioClip sonidoDesbloqueo; // Arrastra el mismo sonido del hacha
    [Range(0f, 1f)] public float volumenDesbloqueo = 0.5f;

    void Start()
    {
        // Si cargamos el checkpoint y ya teníamos el escudo, actualizamos el HUD y destruimos este objeto
        if (Checkpoint.hayCheckpointActivo && Checkpoint.escudoGuardado)
        {
            if (hudImageEscudo != null && nuevaImagenEscudo != null)
            {
                hudImageEscudo.sprite = nuevaImagenEscudo;
            }
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 1. Equipar el escudo en el jugador
        EscudoJugador escudoScript = other.GetComponent<EscudoJugador>();
        if (escudoScript != null)
        {
            escudoScript.EquiparEscudo();
        }

        // 2. Cambiar la imagen del HUD
        if (hudImageEscudo != null && nuevaImagenEscudo != null)
        {
            hudImageEscudo.sprite = nuevaImagenEscudo;
        }

        // 3. Mostrar el aviso en pantalla
        if (avisoUI != null)
        {
            avisoUI.SetActive(true);
            Invoke("OcultarAviso", tiempoAviso);
        }

        // 4. Reproducir el sonido
        if (sonidoDesbloqueo != null)
        {
            AudioSource.PlayClipAtPoint(sonidoDesbloqueo, transform.position, volumenDesbloqueo);
        }

        // 5. Desactivar el escudo flotante
        gameObject.SetActive(false);
    }

    void OcultarAviso()
    {
        if (avisoUI != null) avisoUI.SetActive(false);
    }
}