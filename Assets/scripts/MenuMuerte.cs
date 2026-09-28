using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMuerte : MonoBehaviour
{
    [Header("Configuración de Escenas")]
    [Tooltip("Escribe aquí el nombre EXACTO de tu escena de juego")]
    public string nombreEscenaJuego = "Nivel2";

    [Tooltip("Escribe aquí el nombre EXACTO de tu escena del menú")]
    public string nombreEscenaMenu = "MenuPrincipal";

    public void ReiniciarNivel()
    {
        LimpiarProgreso();
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void IrMenuPrincipal()
    {
        LimpiarProgreso();
        SceneManager.LoadScene(nombreEscenaMenu);
    }

    public void CargarCheckpoint()
    {
        // Al usar el checkpoint, NO limpiamos el progreso.
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    // Esta función resetea toda la memoria RAM para que empieces desde cero
    private void LimpiarProgreso()
    {
        Checkpoint.hayCheckpointActivo = false;
        Checkpoint.maizGuardado = 0;
        Checkpoint.gemasGuardadas = 0;
        Checkpoint.armaGuardada = false;
        Checkpoint.escudoGuardado = false;
    }
}