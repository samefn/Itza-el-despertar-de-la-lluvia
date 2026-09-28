using UnityEngine;

public class TriggerJaguares : MonoBehaviour
{
    [Header("Enemigos a Despertar")]
    [Tooltip("Arrastra aquí a todos los jaguares que quieres que ataquen a la vez")]
    public EnemigoJaguar[] manadaJaguares;

    private void OnTriggerEnter(Collider other)
    {
        // Si el que entra en la zona es el jugador...
        if (other.CompareTag("Player"))
        {
            // Despertamos a cada jaguar en la lista
            foreach (EnemigoJaguar jaguar in manadaJaguares)
            {
                if (jaguar != null)
                {
                    jaguar.Despertar();
                }
            }

            // Desactivamos el cubo para que no vuelva a llamarlos
            gameObject.SetActive(false);
        }
    }
}