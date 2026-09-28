using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogoAutomatico : MonoBehaviour
{
    public AudioSource musica;
    public TextMeshProUGUI textoUI;
    public Image imagenUI;
    public string nombreEscena;

    [TextArea(2,5)]
    public string[] dialogos;

    public float tiempoEntreLineas = 3f;
    public float velocidadTexto = 0.03f; // máquina de escribir
    public float duracionFade = 1f;

    public Sprite imagenSequia;
    public Sprite imagenSacrificio;
    public Sprite imagenXilbaba;

    public Image cuadroDialogo;   // el fondo/cuadro
    public Button botonSaltar;

    string textoActual;
    Coroutine rutinaEscritura;

    int index = 0;
    bool escribiendo = false;

    void Start()
    {
        StartCoroutine(SecuenciaDialogo());
        
    }

    IEnumerator SecuenciaDialogo()
    {
        while (index < dialogos.Length)
        {
            // Cambiar imagen si toca
            CambiarImagen(index);

            // Escribir texto
            yield return StartCoroutine(EscribirTexto(dialogos[index]));

            // Esperar antes de siguiente línea
            yield return new WaitForSeconds(tiempoEntreLineas);

            index++;
        }

        StartCoroutine(FinalConCambioEscena());
    }

    IEnumerator EscribirTexto(string texto)
    {
  escribiendo = true;
    textoActual = texto;
    textoUI.text = "";

    foreach (char letra in texto)
    {
        textoUI.text += letra;
        yield return new WaitForSeconds(velocidadTexto);
    }

    escribiendo = false;
    }

    void CambiarImagen(int i)
    {
        if (i == 0)
            StartCoroutine(FadeImagen(imagenSequia));

        if (i == 3)
            StartCoroutine(FadeImagen(imagenSacrificio));

        if (i == 6)
            StartCoroutine(FadeImagen(imagenXilbaba));
    }

    IEnumerator FadeImagen(Sprite nuevaImagen)
    {
        // Fade out
        float t = 0;
        Color c = imagenUI.color;

        while (t < duracionFade)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / duracionFade);
            imagenUI.color = c;
            yield return null;
        }

        // Cambiar sprite
        imagenUI.sprite = nuevaImagen;

        // Fade in
        t = 0;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / duracionFade);
            imagenUI.color = c;
            yield return null;
        }
    }

    public Image panelNegro;
public float fadeFinal = 1.5f;

IEnumerator FadeUICompleta()
{
 float t = 0;

    Color textoColor = textoUI.color;
    Color cuadroColor = cuadroDialogo.color;

    Image botonImg = botonSaltar.GetComponent<Image>();
    TextMeshProUGUI textoBoton = botonSaltar.GetComponentInChildren<TextMeshProUGUI>();

    Color botonColor = botonImg.color;
    Color textoBotonColor = textoBoton != null ? textoBoton.color : Color.white;

    while (t < duracionFade)
    {
        t += Time.deltaTime;
        float a = Mathf.Lerp(1, 0, t / duracionFade);

        // aplicar alpha
        textoColor.a = a;
        cuadroColor.a = a;
        botonColor.a = a;

        textoUI.color = textoColor;
        cuadroDialogo.color = cuadroColor;
        botonImg.color = botonColor;

        // 🔹 fade del texto del botón
        if (textoBoton != null)
        {
            textoBotonColor.a = a;
            textoBoton.color = textoBotonColor;
        }

        yield return null;
    }

    // desactivar botón al final
    botonSaltar.interactable = false;
}

IEnumerator FadeOutMusica()
{
    float t = 0;
    float volInicial = musica.volume;

    while (t < fadeFinal) // 👈 usa el mismo tiempo
    {
        t += Time.deltaTime;
        musica.volume = Mathf.Lerp(volInicial, 0, t / fadeFinal);
        yield return null;
    }
}

IEnumerator FinalConCambioEscena()
{
    // 🔥 sincroniza música con el fade visual
    StartCoroutine(FadeOutMusica());

    float t = 0;

    // referencias
    Color imgColor = imagenUI.color;
    Color textoColor = textoUI.color;
    Color cuadroColor = cuadroDialogo.color;

    Image botonImg = botonSaltar.GetComponent<Image>();
    TextMeshProUGUI textoBoton = botonSaltar.GetComponentInChildren<TextMeshProUGUI>();

    Color botonColor = botonImg.color;
    Color textoBotonColor = textoBoton != null ? textoBoton.color : Color.white;

    // 🔥 FADE TODO AL MISMO TIEMPO
    while (t < duracionFade)
    {
        t += Time.deltaTime;
        float a = Mathf.Lerp(1, 0, t / duracionFade);

        imgColor.a = a;
        imagenUI.color = imgColor;

        textoColor.a = a;
        textoUI.color = textoColor;

        cuadroColor.a = a;
        cuadroDialogo.color = cuadroColor;

        botonColor.a = a;
        botonImg.color = botonColor;

        if (textoBoton != null)
        {
            textoBotonColor.a = a;
            textoBoton.color = textoBotonColor;
        }

        yield return null;
    }

    botonSaltar.interactable = false;

    // 🔹 Fade a negro (aquí la música sigue bajando)
    t = 0;
    Color panelColor = panelNegro.color;

    while (t < fadeFinal)
    {
        t += Time.deltaTime;
        panelColor.a = Mathf.Lerp(0, 1, t / fadeFinal);
        panelNegro.color = panelColor;
        yield return null;
    }

    SceneManager.LoadScene(nombreEscena);
}

}