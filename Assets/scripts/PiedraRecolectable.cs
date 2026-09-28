using UnityEngine;

public class PiedraRecolectable : MonoBehaviour
{
    private ScoreManager scoreManager;

    [Header("Sonido")]
    public AudioClip sonidoPiedra; // NUEVO: El clip de audio para la baldosa

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
                scoreManager.RecogerPiedra();
            }

            // NUEVO: Reproducir el sonido de roca/deslizamiento antes de destruir
            if (sonidoPiedra != null)
            {
                AudioSource.PlayClipAtPoint(sonidoPiedra, transform.position);
            }

            Destroy(gameObject);
        }
    }
}