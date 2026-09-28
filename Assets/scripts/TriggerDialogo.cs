using UnityEngine;

public class TriggerDialogo : MonoBehaviour
{
   public Dialogo dialogoUI;

    [TextArea(2,5)]
    public string[] misDialogos;

    private bool yaSeMostro = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (yaSeMostro) return;

        yaSeMostro = true;

        dialogoUI.IniciarDialogo(misDialogos);

        // 🔥 CLAVE: desactiva el trigger para siempre
        GetComponent<Collider>().enabled = false;
    }
}
