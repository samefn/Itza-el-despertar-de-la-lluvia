using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Inventario de Maíz")]
    public int maizActual = 0;
    public int maizMaximo = 5;
    public TextMeshProUGUI textoMaiz;

    [Header("Contadores UI")]
    public TextMeshProUGUI textoContadorGemas;

    [Header("Interfaz (UI) Objetos Clave")]
    public GameObject iconoBaldosaHUD;

    // ¡SOLUCIÓN AQUÍ! Cambiamos 'private' por 'public'
    public int cantidadGemas = 0;
    private int cantidadPiedrasVerdes = 0;

    void Start()
    {
        // Si reaparecemos por un checkpoint, recuperamos los objetos
        if (Checkpoint.hayCheckpointActivo)
        {
            maizActual = Checkpoint.maizGuardado;
            cantidadGemas = Checkpoint.gemasGuardadas;

            ActualizarHUD(); // Actualiza el número de maíz
            ActualizarUI();  // Actualiza el número de gemas
        }
    }

    public void SumarMaiz(int cantidad)
    {
        if (maizActual < maizMaximo)
        {
            maizActual += cantidad;

            if (maizActual > maizMaximo) maizActual = maizMaximo;

            ActualizarHUD();
        }
        else
        {
            Debug.Log("Inventario de maíz lleno.");
        }
    }

    public bool UsarMaiz()
    {
        if (maizActual > 0)
        {
            maizActual--;
            ActualizarHUD();
            return true;
        }
        return false;
    }

    public void SumarGemas(int cantidad)
    {
        cantidadGemas += cantidad;
        ActualizarUI();
    }

    public void RecogerPiedra()
    {
        cantidadPiedrasVerdes++;
        Debug.Log("Piedra recolectada. Total en inventario: " + cantidadPiedrasVerdes);
        if (iconoBaldosaHUD != null) iconoBaldosaHUD.SetActive(true);
    }

    private void ActualizarUI()
    {
        if (textoContadorGemas != null) textoContadorGemas.text = cantidadGemas.ToString();
    }

    public bool TienePiedra()
    {
        return cantidadPiedrasVerdes > 0;
    }

    public void UsarPiedra()
    {
        if (cantidadPiedrasVerdes > 0)
        {
            cantidadPiedrasVerdes--;
            Debug.Log("Piedra utilizada para abrir la puerta.");
        }
        if (iconoBaldosaHUD != null) iconoBaldosaHUD.SetActive(false);
    }

    void ActualizarHUD()
    {
        if (textoMaiz != null)
        {
            // Mantenemos tu diseño minimalista (solo el número)
            textoMaiz.text = maizActual.ToString();
        }
    }
}