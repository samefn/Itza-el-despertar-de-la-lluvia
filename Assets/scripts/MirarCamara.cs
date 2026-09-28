using UnityEngine;

public class MirarCamara : MonoBehaviour
{
    private Camera camaraPrincipal;

    void Start()
    {
        // Busca la cámara principal del jugador automáticamente
        camaraPrincipal = Camera.main;
    }

    // Usamos LateUpdate para asegurarnos de que la imagen rote DESPUÉS de que Soraya caminó
    void LateUpdate()
    {
        if (camaraPrincipal != null)
        {
            // Obliga a la imagen a mirar exactamente en la misma dirección que la cámara
            transform.forward = camaraPrincipal.transform.forward;
        }
    }
}