using UnityEngine;
using TMPro;

public class Dialogo2 : MonoBehaviour
{
    public TextMeshProUGUI textoUI;
    string[] dialogos;
    int index = 0;

    GuardianMovement guardian;
    Transform destino;
    SubirObjeto scriptSubir; // 👈 NUEVO: Referencia para el fade

    // Añadimos el parámetro 's' para recibir el script SubirObjeto
    public void IniciarDialogo(string[] nuevosDialogos, GuardianMovement g, Transform d, SubirObjeto s = null)
    {
        dialogos = nuevosDialogos;
        index = 0;
        guardian = g;
        destino = d;
        scriptSubir = s; // 👈 Guardamos la referencia

        textoUI.text = dialogos[index];
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            index++;
            if (index < dialogos.Length)
            {
                textoUI.text = dialogos[index];
            }
            else
            {
                gameObject.SetActive(false);

                // 🔥 AQUÍ TERMINA EL DIÁLOGO: Avisamos al script de subir objeto
                if (scriptSubir != null)
                {
                    scriptSubir.IniciarTransicionEscena();
                }

                if (guardian != null && destino != null)
                {
                    guardian.MoveToPosition(destino);
                }
            }
        }
    }
}