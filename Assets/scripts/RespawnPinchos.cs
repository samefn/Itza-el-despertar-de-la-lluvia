using UnityEngine;

public class RespawnPinchos : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entró a los pinchos es el jugador
        if (other.CompareTag("Player"))
        {
            // Buscamos el script de salud del jugador
            SaludJugador scriptSalud = other.GetComponent<SaludJugador>();

            if (scriptSalud != null)
            {
                // Le aplicamos un daño masivo para vaciar la barra a 0.
                // Al llegar a 0, el script de SaludJugador se encarga de TODO lo demás
                // (sonido, animación, esperar 4 segundos y mostrar el panel de muerte).
                scriptSalud.RecibirDano(9999);
            }
        }
    }
}