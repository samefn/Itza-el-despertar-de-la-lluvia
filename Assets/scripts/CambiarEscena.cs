using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    // 👉 Cargar una escena por nombre
    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // 👉 Salir del juego
    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}