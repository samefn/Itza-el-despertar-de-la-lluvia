using UnityEngine;

public class GuardianMovement : MonoBehaviour
{
    public GameObject objetoFinal; // lo que quieres activar al final
public Transform ultimoPunto;  // referencia al último punto
    public Transform currentTarget;
    public float speed = 3f;
    bool shouldMove = false;

    public void MoveToPosition(Transform target)
    {
        currentTarget = target;
        shouldMove = true;
    }

    // 🔥 ESTE ES EL QUE TE FALTABA
    public void MoverANuevoPunto(Transform target)
    {
        MoveToPosition(target);
    }

    void Update()
    {
        if (shouldMove && currentTarget != null)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            shouldMove = false;

            // 🔥 si es el último punto → activar objeto
            if (currentTarget == ultimoPunto && objetoFinal != null)
            {
                objetoFinal.SetActive(true);

                Debug.Log("🎉 Evento final activado");
            }
        }
    }
    }
}