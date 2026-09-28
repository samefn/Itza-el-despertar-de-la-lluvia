using UnityEngine;

public class AtrioEstatuas : MonoBehaviour
{
    [Header("Estado del Atrio")]
    public bool tieneEstatua = false;
    public int idEstatuaActual = 0;

    [Header("Conexión al Cerebro")]
    public ManagerPuzzleEstatuas managerPuzzle;
    [HideInInspector] public bool puzzleResuelto = false;

    [Header("Modelos Visuales (Hijos del Atrio)")]
    public GameObject[] modelosEstatuas;

    [Header("Interfaz Visual (Imágenes)")]
    public GameObject cartelPoner;
    public GameObject cartelRecoger;
    public GameObject cartelCambiar;

    private bool jugadorCerca = false;
    private InventarioEstatuas inventarioJugador;

    void Start()
    {
        ActualizarVisual();
        ApagarTodosLosCarteles();
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
            ApagarTodosLosCarteles();
        }
    }

    void Update()
    {
        if (jugadorCerca && inventarioJugador != null && !puzzleResuelto)
        {
            bool jugadorTiene = inventarioJugador.tieneEstatuaEnMano;

            // Primero apagamos todos por seguridad
            ApagarTodosLosCarteles();

            // Luego encendemos solo la imagen correcta
            if (jugadorTiene && !tieneEstatua)
            {
                if (cartelPoner != null) cartelPoner.SetActive(true);
            }
            else if (!jugadorTiene && tieneEstatua)
            {
                if (cartelRecoger != null) cartelRecoger.SetActive(true);
            }
            else if (jugadorTiene && tieneEstatua)
            {
                if (cartelCambiar != null) cartelCambiar.SetActive(true);
            }

            // ACCIONES AL PRESIONAR LA TECLA
            if (Input.GetKeyDown(KeyCode.G))
            {
                Interactuar();
            }
        }
        else
        {
            // Si nos alejamos o el puzzle ya se resolvió, apagamos los carteles
            ApagarTodosLosCarteles();
        }
    }

    void Interactuar()
    {
        bool jugadorTiene = inventarioJugador.tieneEstatuaEnMano;

        if (jugadorTiene && !tieneEstatua)
        {
            idEstatuaActual = inventarioJugador.SoltarEstatua();
            tieneEstatua = true;
        }
        else if (!jugadorTiene && tieneEstatua)
        {
            inventarioJugador.RecogerEstatua(idEstatuaActual);
            tieneEstatua = false;
            idEstatuaActual = 0;
        }
        else if (jugadorTiene && tieneEstatua)
        {
            int idQueTeniaElAtrio = idEstatuaActual;
            idEstatuaActual = inventarioJugador.SoltarEstatua();
            inventarioJugador.RecogerEstatua(idQueTeniaElAtrio);
        }

        ActualizarVisual();
        if (managerPuzzle != null) managerPuzzle.ValidarPuzzle();
    }

    public void ActualizarVisual()
    {
        foreach (GameObject modelo in modelosEstatuas)
        {
            if (modelo != null) modelo.SetActive(false);
        }

        if (tieneEstatua && idEstatuaActual > 0 && idEstatuaActual <= modelosEstatuas.Length)
        {
            if (modelosEstatuas[idEstatuaActual - 1] != null)
            {
                modelosEstatuas[idEstatuaActual - 1].SetActive(true);
            }
        }
    }

    // Pequeña función auxiliar para mantener el código limpio
    void ApagarTodosLosCarteles()
    {
        if (cartelPoner != null) cartelPoner.SetActive(false);
        if (cartelRecoger != null) cartelRecoger.SetActive(false);
        if (cartelCambiar != null) cartelCambiar.SetActive(false);
    }
}