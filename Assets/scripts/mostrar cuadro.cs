using UnityEngine;

public class mostrarcuadro : MonoBehaviour
{
 public GameObject imagenUI;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            imagenUI.SetActive(true);
        }
    }
}
