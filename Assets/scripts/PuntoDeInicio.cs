using UnityEngine;

public class PuntoDeInicio : MonoBehaviour
{
    void Start()
    {
        // Busca a Soraya usando su etiqueta
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        
        if (jugador != null)
        {
            // Apagamos momentáneamente componentes físicos si los tiene para evitar rebotes
            CharacterController cc = jugador.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // Teletransportamos a Soraya a la posición y rotación de este objeto
            jugador.transform.position = transform.position;
            jugador.transform.rotation = transform.rotation;

            // Volvemos a encender las físicas
            if (cc != null) cc.enabled = true;
            
            Debug.Log("📍 Soraya ha sido teletransportada al Punto de Inicio.");
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró ningún objeto con el tag 'Player'.");
        }
    }
}