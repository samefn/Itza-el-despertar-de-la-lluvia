using UnityEngine;
using System.Collections;

public class AtaqueJugador : MonoBehaviour
{
    [Header("Estado del Arma")]
    public bool tieneArma = false;
    public GameObject modeloArmaEnMano;

    [Header("Hitbox (Nuevo Sistema)")]
    public Transform puntoDeAtaque;
    public float radioDeAtaque = 1.5f;
    public LayerMask capaEnemigos;

    [Header("Configuración del Ataque")]
    public int danoAtaque = 40;
    public float tiempoEntreAtaques = 1f;
    public float retrasoDano = 0.3f;

    [Header("Referencias")]
    public Animator animatorCuerpo;

    [Header("Sonidos")]
    public AudioSource audioFuenteJugador;
    public AudioClip sonidoSwing;

    [Header("Sonidos de Voz")]
    public AudioSource audioFuenteVoz;
    public AudioClip vozAtaque1;
    public AudioClip vozAtaque2;
    public AudioClip vozAtaque3;

    private float proximoAtaque = 0f;
    private int contadorCombo = 1;

    public void EquiparArma()
    {
        tieneArma = true;
        if (modeloArmaEnMano != null) modeloArmaEnMano.SetActive(true);
    }

    void Update()
    {
        if (!tieneArma) return;

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= proximoAtaque)
        {
            Atacar();
            proximoAtaque = Time.time + tiempoEntreAtaques;
        }
    }

    void Atacar()
    {
        // 1. Sonido del arma
        if (audioFuenteJugador != null && sonidoSwing != null)
        {
            audioFuenteJugador.pitch = 1f;
            audioFuenteJugador.PlayOneShot(sonidoSwing);
        }

        // 2. Sonidos de voz
        if (audioFuenteVoz != null)
        {
            SaludJugador salud = GetComponent<SaludJugador>();

            if (salud == null || !salud.estaQuejandose)
            {
                audioFuenteVoz.pitch = 1f;
                audioFuenteVoz.Stop();

                if (contadorCombo == 1 && vozAtaque1 != null) audioFuenteVoz.PlayOneShot(vozAtaque1);
                else if (contadorCombo == 2 && vozAtaque2 != null) audioFuenteVoz.PlayOneShot(vozAtaque2);
                else if (contadorCombo == 3 && vozAtaque3 != null) audioFuenteVoz.PlayOneShot(vozAtaque3);
            }
        }

        // 3. Animación
        if (animatorCuerpo != null) animatorCuerpo.SetTrigger("Golpear" + contadorCombo);

        contadorCombo++;
        if (contadorCombo > 3) contadorCombo = 1;

        // 4. Hitbox
        StartCoroutine(ConectarGolpe());
    }

    IEnumerator ConectarGolpe()
    {
        yield return new WaitForSeconds(retrasoDano);

        Collider[] enemigosAlcanzados = Physics.OverlapSphere(puntoDeAtaque.position, radioDeAtaque, capaEnemigos);

        Debug.Log("🟡 El ataque alcanzó a " + enemigosAlcanzados.Length + " objetos en la capaEnemigos.");

        foreach (Collider enemigo in enemigosAlcanzados)
        {
            Debug.Log("   -> El hacha detectó a: " + enemigo.gameObject.name);

            // --- LÓGICA DEL ZOMBIE ---
            ZombieSalud zombie = enemigo.GetComponent<ZombieSalud>();
            if (zombie != null)
            {
                zombie.RecibirDano(danoAtaque);
            }

            // --- LÓGICA DEL JAGUAR ---
            EnemigoJaguar jaguar = enemigo.GetComponent<EnemigoJaguar>();
            if (jaguar == null)
            {
                jaguar = enemigo.GetComponentInParent<EnemigoJaguar>();
            }

            if (jaguar != null)
            {
                Debug.Log("   ✅ ¡Jaguar procesado! Aplicando daño...");
                jaguar.RecibirDano(25);
            }

            // --- LÓGICA DEL JEFE MUTANTE (NUEVO) ---
            JefeMutante jefe = enemigo.GetComponent<JefeMutante>();
            if (jefe == null)
            {
                jefe = enemigo.GetComponentInParent<JefeMutante>();
            }

            if (jefe != null)
            {
                Debug.Log("   ✅ ¡Jefe Mutante procesado! Aplicando daño...");
                jefe.RecibirDano(danoAtaque);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (puntoDeAtaque == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoDeAtaque.position, radioDeAtaque);
    }

    void Start()
    {
        if (Checkpoint.hayCheckpointActivo)
        {
            if (Checkpoint.armaGuardada)
            {
                EquiparArma();
            }
        }
    }
}