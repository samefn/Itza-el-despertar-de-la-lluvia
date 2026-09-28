using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camaraPrincipal;

    void Start()
    {
        camaraPrincipal = Camera.main.transform;
    }

    void LateUpdate()
    {
        // 1. Miramos hacia la cámara
        transform.LookAt(transform.position + camaraPrincipal.forward);

        // 2. Obligamos a que la rotación en X y Z sea siempre 0 (totalmente plana)
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}