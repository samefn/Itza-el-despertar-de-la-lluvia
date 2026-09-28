using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
 public TextMeshProUGUI textoUI;

    string[] dialogos;
    int index = 0;

    public void IniciarDialogo(string[] nuevosDialogos)
    {
        dialogos = nuevosDialogos;
        index = 0;
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
            }
        }
    }
}
