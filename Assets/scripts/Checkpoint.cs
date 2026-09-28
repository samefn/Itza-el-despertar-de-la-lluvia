using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Variables de Posición
    public static bool hayCheckpointActivo = false;
    public static Vector3 posicionGuardada;

    // NUEVO: Variables de Inventario
    public static int maizGuardado = 0;
    public static int gemasGuardadas = 0;
    public static bool armaGuardada = false;
    public static bool escudoGuardado = false;

    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;

            // 1. Guardar posición
            posicionGuardada = transform.position;
            hayCheckpointActivo = true;

            // 2. Guardar estado del HUD y recolección
            ScoreManager score = Object.FindFirstObjectByType<ScoreManager>();
            if (score != null)
            {
                maizGuardado = score.maizActual;
                gemasGuardadas = score.cantidadGemas;
            }

            // 3. Guardar estado del arma
            AtaqueJugador ataque = other.GetComponent<AtaqueJugador>();
            if (ataque != null)
            {
                armaGuardada = ataque.tieneArma;
            }

            EscudoJugador escudo = other.GetComponent<EscudoJugador>();
            if (escudo != null)
            {
                escudoGuardado = escudo.tieneEscudo;
            }

            Debug.Log("¡Checkpoint Guardado: Posición y objetos registrados!");
        }
    }
}