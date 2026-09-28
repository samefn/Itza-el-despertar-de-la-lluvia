using UnityEngine;

public class AltarManager : MonoBehaviour
{
    public int totalAltares = 3;
    private int altaresCompletados = 0;

    public GameObject puerta; // o lo que quieras activar

    public void AltarCompletado()
    {
        altaresCompletados++;

        Debug.Log("Altares completados: " + altaresCompletados);

        if (altaresCompletados >= totalAltares)
        {
            ActivarEventoFinal();
        }
    }

    void ActivarEventoFinal()
    {
        Debug.Log("🔥 PUZZLE COMPLETADO");

        if (puerta != null)
        {
            puerta.SetActive(false); // abre la puerta
        }
    }
}