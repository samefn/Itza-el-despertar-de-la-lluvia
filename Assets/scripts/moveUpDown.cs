using UnityEngine;

public class moveUpDown : MonoBehaviour
{
public float speed = 2f;     // Velocidad
    public float distance = 2f;  // Distancia máxima

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        transform.position = new Vector3(startPos.x, startPos.y, startPos.z + offset);
    }
}
