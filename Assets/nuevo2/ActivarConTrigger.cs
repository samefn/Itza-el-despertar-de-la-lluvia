using UnityEngine;

public class ActivarConTrigger : MonoBehaviour
{
    public SubirObjeto scriptMovimiento;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scriptMovimiento.enabled = true;
        }
    }
}