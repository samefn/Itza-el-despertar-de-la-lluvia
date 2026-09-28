using UnityEngine;

public class ManagerPuzzleEstatuas : MonoBehaviour
{
    [Header("Conexiones del Puzzle")]
    public AtrioEstatuas[] atrios;
    public int[] combinacionCorrecta;

    [Header("Recompensa Final (Gema Roja)")]
    [Tooltip("Arrastra aquí el Colisionador de la gema")]
    public Collider colisionadorGemaRoja;
    [Tooltip("Opcional: Alguna luz o partícula que se encienda al desbloquearla")]
    public GameObject efectoDesbloqueo;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoError;
    public AudioClip sonidoExito;

    private bool puzzleResuelto = false;

    void Start()
    {
        // 1. Nos aseguramos de que el colisionador empiece SÓLIDO (IsTrigger = false)
        if (colisionadorGemaRoja != null) 
        {
            colisionadorGemaRoja.isTrigger = false;
        }
        
        // 2. Apagamos el efecto de luz/partículas
        if (efectoDesbloqueo != null) efectoDesbloqueo.SetActive(false);
    }

    public void ValidarPuzzle()
    {
        if (puzzleResuelto) return;

        bool todosLlenos = true;
        foreach (AtrioEstatuas atrio in atrios)
        {
            if (!atrio.tieneEstatua)
            {
                todosLlenos = false;
                break;
            }
        }

        if (!todosLlenos) return;

        bool ordenCorrecto = true;
        for (int i = 0; i < atrios.Length; i++)
        {
            if (atrios[i].idEstatuaActual != combinacionCorrecta[i])
            {
                ordenCorrecto = false;
                break;
            }
        }

        if (ordenCorrecto)
        {
            Debug.Log("🎉 ¡PUZZLE RESUELTO! Las estatuas están en el orden correcto.");
            if (audioSource != null && sonidoExito != null) audioSource.PlayOneShot(sonidoExito);
            
            puzzleResuelto = true;
            BloquearAtrios();
            
            DesbloquearGema(); 
        }
        else
        {
            Debug.Log("❌ Combinación incorrecta.");
            if (audioSource != null && sonidoError != null) audioSource.PlayOneShot(sonidoError);
        }
    }

    void BloquearAtrios()
    {
        foreach (AtrioEstatuas atrio in atrios)
        {
            atrio.puzzleResuelto = true; 
            atrio.ActualizarVisual(); 
        }
    }

    void DesbloquearGema()
    {
        // Convertimos el colisionador sólido en un TRIGGER para poder recoger la gema
        if (colisionadorGemaRoja != null) 
        {
            colisionadorGemaRoja.isTrigger = true;
        }

        if (efectoDesbloqueo != null) efectoDesbloqueo.SetActive(true);
        
        Debug.Log("💎 Gema roja ahora es un Trigger y está lista para recoger.");
    }
}