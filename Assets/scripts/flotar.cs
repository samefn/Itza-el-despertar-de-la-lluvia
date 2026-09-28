using UnityEngine;

public class flotar : MonoBehaviour
{
 public float floatSpeed = 1f;   // Velocidad del movimiento
    public float floatHeight = 0.3f; // Altura máxima

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
