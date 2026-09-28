using UnityEngine;

public class InventarioEstatuas : MonoBehaviour
{
    [Header("Estado del Inventario")]
    public bool tieneEstatuaEnMano = false;
    public int idEstatuaActual = 0; 

    [Header("Sonidos de Interacción")]
    public AudioClip sonidoRecoger; // Sonido al agarrar la estatua (Ej: roce de piedra, ropa)
    public AudioClip sonidoPoner;   // Sonido al dejarla en el atrio (Ej: golpe de piedra o 'clack')

    public void RecogerEstatua(int id)
    {
        tieneEstatuaEnMano = true;
        idEstatuaActual = id;
        
        // Reproducir sonido de recoger
        if (sonidoRecoger != null)
        {
            AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
        }

        Debug.Log("🟩 Inventario: Soraya recogió la estatua ID [" + id + "]");
    }

    public int SoltarEstatua()
    {
        int idSoltado = idEstatuaActual; 
        
        tieneEstatuaEnMano = false;
        idEstatuaActual = 0;

        // Reproducir sonido de poner
        if (sonidoPoner != null)
        {
            AudioSource.PlayClipAtPoint(sonidoPoner, transform.position);
        }

        Debug.Log("🟨 Inventario: Soraya soltó la estatua ID [" + idSoltado + "]");
        
        return idSoltado; 
    }

    // Mantenemos tu sistema de texto verde en pantalla por si aún te sirve para hacer pruebas
    void OnGUI()
    {
        if (tieneEstatuaEnMano)
        {
            GUI.color = Color.green;
            GUI.skin.label.fontSize = 20;
            GUI.Label(new Rect(20, 20, 400, 40), "Llevando estatua ID " + idEstatuaActual);
        }
    }
}