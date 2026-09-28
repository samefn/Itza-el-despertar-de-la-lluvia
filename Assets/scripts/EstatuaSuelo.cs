using UnityEngine;

public class EstatuaSuelo : MonoBehaviour
{
    [Header("Configuración de la Estatua")]
    public int idEstatua = 1;

    [Header("Interfaz Visual (Imágenes)")]
    [Tooltip("Arrastra aquí tu objeto con la imagen que dice 'Recoger'")]
    public GameObject cartelRecoger; 

    private bool jugadorCerca = false;
    private InventarioEstatuas inventarioJugador;

    void Start()
    {
        if (cartelRecoger != null) cartelRecoger.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            inventarioJugador = other.GetComponent<InventarioEstatuas>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            inventarioJugador = null;
            if (cartelRecoger != null) cartelRecoger.SetActive(false);
        }
    }

    void Update()
    {
        if (jugadorCerca && inventarioJugador != null)
        {
            if (!inventarioJugador.tieneEstatuaEnMano)
            {
                // Mostramos la imagen de Recoger
                if (cartelRecoger != null) cartelRecoger.SetActive(true);

                if (Input.GetKeyDown(KeyCode.G))
                {
                    inventarioJugador.RecogerEstatua(idEstatua);
                    gameObject.SetActive(false); 
                }
            }
            else
            {
                // Ocultamos la imagen si ya tenemos las manos llenas
                if (cartelRecoger != null) cartelRecoger.SetActive(false);
            }
        }
    }
}