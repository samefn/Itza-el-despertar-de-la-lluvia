using UnityEngine;
using UnityEngine.UI;

public class Cambiarhud : MonoBehaviour
{
    public Image hudImage;
    public Sprite nuevaImagen;
    public GameObject avisoUI;
    public float tiempoAviso = 2f;

    [Header("Sonido")]
    public AudioClip sonidoDesbloqueo; 
    [Range(0f, 1f)] public float volumenDesbloqueo = 0.5f;

    // NUEVO: Verificamos el estado apenas carga la escena
    void Start()
    {
        if (Checkpoint.hayCheckpointActivo && Checkpoint.armaGuardada)
        {
            // 1. Ponemos la imagen a color en el HUD
            if (hudImage != null && nuevaImagen != null)
            {
                hudImage.sprite = nuevaImagen;
            }
            
            // 2. Desactivamos el objeto físico en el mundo para no volver a agarrarlo
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        AtaqueJugador ataqueScript = other.GetComponent<AtaqueJugador>();
        if (ataqueScript != null)
        {
            ataqueScript.EquiparArma();
        }

        hudImage.sprite = nuevaImagen;
        avisoUI.SetActive(true);
        Invoke("OcultarAviso", tiempoAviso);

        if (sonidoDesbloqueo != null)
        {
            AudioSource.PlayClipAtPoint(sonidoDesbloqueo, transform.position, volumenDesbloqueo);
        }

        gameObject.SetActive(false);
    }

    void OcultarAviso()
    {
        avisoUI.SetActive(false);
    }
}