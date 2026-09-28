using UnityEngine;
using System.Collections;

public class TriggerEfectoCompleto : MonoBehaviour
{
    public Transform camara;
    public float duracion = 1f;
    public float intensidad = 0.2f;

    public AudioSource audioSource; // Fuente de audio
    public AudioClip sonido;        // Sonido a reproducir

    private Vector3 posicionOriginal;
    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!activado && other.CompareTag("Player"))
        {
            activado = true;

            // Reproducir sonido
            if (audioSource != null && sonido != null)
            {
                audioSource.PlayOneShot(sonido);
            }

            // Iniciar vibración
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        posicionOriginal = camara.localPosition;

        float tiempo = 0f;

        while (tiempo < duracion)
        {
            float x = Random.Range(-1f, 1f) * intensidad;
            float y = Random.Range(-1f, 1f) * intensidad;

            camara.localPosition = posicionOriginal + new Vector3(x, y, 0);

            tiempo += Time.deltaTime;
            yield return null;
        }

        camara.localPosition = posicionOriginal;
    }
}