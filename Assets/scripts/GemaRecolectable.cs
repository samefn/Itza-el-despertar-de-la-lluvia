using UnityEngine;

public class GemaRecolectable : MonoBehaviour
{
    private ScoreManager scoreManager;
    
    [Header("Sonido")]
    public AudioClip sonidoGema; // NUEVO

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (scoreManager != null)
            {
                scoreManager.SumarGemas(1);
            }

            // NUEVO: Reproducir el tintineo antes de destruir
            if (sonidoGema != null)
            {
                AudioSource.PlayClipAtPoint(sonidoGema, transform.position);
            }

            Destroy(gameObject);
        }
    }
}