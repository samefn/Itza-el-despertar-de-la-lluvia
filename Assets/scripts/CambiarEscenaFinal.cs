using UnityEngine;
using UnityEngine.SceneManagement; // ¡Esta línea es obligatoria para cambiar de escenas!

public class CambiarEscenaFinal : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("El nombre exacto de la escena a la que quieres ir")]
    public string nombreEscena = "FINALFINAL";

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entró al trigger es Soraya (el Player)
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Trigger alcanzado! Cargando la escena: " + nombreEscena);
            SceneManager.LoadScene(nombreEscena);
        }
    }
}