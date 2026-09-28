using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicaEscena;

    void Start()
    {
        audioSource.clip = musicaEscena;
        audioSource.loop = true;
        audioSource.Play();
    }
}