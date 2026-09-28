using UnityEngine;

public class ZonaZombie : MonoBehaviour
{
    [Header("Conexión con el Zombie")]
    [Tooltip("Arrastra aquí a tu zombie desde la Jerarquía")]
    public ZombieIA zombieAsignado;

    private void OnTriggerEnter(Collider other)
    {
        // Si el que entra en la zona es el jugador...
        if (other.CompareTag("Player"))
        {
            // Verificamos que hayamos asignado al zombie en el inspector
            if (zombieAsignado != null)
            {
                zombieAsignado.ActivarPersecucion(); // Despierta al zombie
            }
            
            // Destruimos esta zona invisible para que no siga ejecutando el código innecesariamente
            Destroy(gameObject);
        }
    }
}