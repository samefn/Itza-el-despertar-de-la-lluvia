using UnityEngine;

public class tigger2 : MonoBehaviour
{
    public Dialogo2 dialogoUI; // 👈 usa el nuevo script

    [TextArea(2,5)]
    public string[] misDialogos;

    public GuardianMovement guardian;   // 👈 nuevo
    public Transform puntoDestino;      // 👈 nuevo

    private bool yaSeMostro = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (yaSeMostro) return;

        yaSeMostro = true;

        dialogoUI.IniciarDialogo(misDialogos, guardian, puntoDestino);

        GetComponent<Collider>().enabled = false;
    }
}