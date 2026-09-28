using UnityEngine;

public class EscudoJugador : MonoBehaviour
{
    [Header("Estado del Escudo")]
    public bool tieneEscudo = false;
    public GameObject modeloEscudoEnMano;
    public Animator animatorCuerpo;

    // NUEVO: Esta variable le dirá al sistema de salud si estamos en guardia
    public bool estaBloqueando = false;

    public void EquiparEscudo()
    {
        tieneEscudo = true;
        if (modeloEscudoEnMano != null) modeloEscudoEnMano.SetActive(true);
    }

    void Start()
    {
        if (Checkpoint.hayCheckpointActivo && Checkpoint.escudoGuardado)
        {
            EquiparEscudo();
        }
    }

    void Update()
    {
        if (!tieneEscudo || animatorCuerpo == null) return;

        // Actualizamos nuestra nueva variable dependiendo de si la Q está presionada
        estaBloqueando = Input.GetKey(KeyCode.Q);

        animatorCuerpo.SetBool("EscudoActivo", estaBloqueando);
    }
}