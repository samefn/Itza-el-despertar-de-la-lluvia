using UnityEngine;
using UnityEngine.UI;

public class SelectorGemas : MonoBehaviour
{
    [Header("Gemas en mano")]
    public GameObject[] gemasMano;

    [System.Serializable]
    public class SlotHUD
    {
        public Image imagen;
        public Sprite normal;
        public Sprite seleccionado;
    }

    [Header("HUD")]
    public SlotHUD[] slots;

    int gemaActual = -1;

    // 🔥 controla si la gema aún existe
    bool[] gemaDisponible;

    void Start()
    {
        gemaDisponible = new bool[gemasMano.Length];

        for (int i = 0; i < gemaDisponible.Length; i++)
            gemaDisponible[i] = true;

        DesactivarTodas();
        ActualizarHUD();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SeleccionarGema(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SeleccionarGema(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SeleccionarGema(2);
    }

    void SeleccionarGema(int index)
    {
        // 🔥 si ya se usó, no deja seleccionarla
        if (!gemaDisponible[index]) return;

        gemaActual = index;

        // activar solo esa gema en la mano
        for (int i = 0; i < gemasMano.Length; i++)
        {
            gemasMano[i].SetActive(i == index);
        }

        ActualizarHUD();
    }

    void ActualizarHUD()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == gemaActual)
                slots[i].imagen.sprite = slots[i].seleccionado;
            else
                slots[i].imagen.sprite = slots[i].normal;
        }
    }

    void DesactivarTodas()
    {
        foreach (var g in gemasMano)
            g.SetActive(false);
    }

    // 🔥 el altar usa esto
    public int ObtenerGemaActual()
    {
        return gemaActual;
    }

    // 🔥 cuando usas una gema
    public void MarcarGemaUsada(int index)
    {
        gemaDisponible[index] = false;

        // quitar de la mano
        gemasMano[index].SetActive(false);

        // quitar selección
        if (gemaActual == index)
        {
            gemaActual = -1;
        }
    }
}