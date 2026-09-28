using UnityEngine;

public class ActivarMouse : MonoBehaviour
{
    void Start()
    {
        // 1. Desbloqueamos el mouse (por si estaba atrapado en el centro)
        Cursor.lockState = CursorLockMode.None;
        
        // 2. Lo hacemos visible (por si estaba oculto)
        Cursor.visible = true;

        // 3. Opcional: Aseguramos que el tiempo no esté pausado
        Time.timeScale = 1f;
    }
}