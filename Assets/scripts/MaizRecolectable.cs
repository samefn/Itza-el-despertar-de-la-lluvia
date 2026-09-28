using UnityEngine;

public class MaizRecolectable : MonoBehaviour
{
    [Header("Sonido")]
    public AudioClip sonidoRecogerMaiz; // NUEVO
    public int valor = 1; // Cuánto suma este maíz
    private ScoreManager scoreManager;

    void Start()
    {
        // Busca automáticamente el ScoreManager en la escena al iniciar
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el que entró al trigger es el jugador
        if (other.CompareTag("Player"))
        {
            // Sumamos el maíz
            if (scoreManager != null)
            {
                scoreManager.SumarMaiz(valor);
            }

            if (sonidoRecogerMaiz != null)
            {
                AudioSource.PlayClipAtPoint(sonidoRecogerMaiz, transform.position);
            }
            
            Destroy(gameObject);
        }
    }
}