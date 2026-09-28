using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HoverGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
     public TextMeshProUGUI text;
    public Material normalMat;
    public Material glowMat;

    public RectTransform leftBar;
    public RectTransform rightBar;

    public float scaleMultiplier = 1.05f;
    public float speed = 8f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private float targetAlpha = 0f;

    private Image leftImg;
    private Image rightImg;

    private Vector2 leftStartPos;
    private Vector2 rightStartPos;

    void Start()
    {
        originalScale = text.transform.localScale;
        targetScale = originalScale;

        leftImg = leftBar.GetComponent<Image>();
        rightImg = rightBar.GetComponent<Image>();

        leftStartPos = leftBar.anchoredPosition;
        rightStartPos = rightBar.anchoredPosition;

        SetAlpha(0f);
    }

    void Update()
    {
        // Escala suave
        text.transform.localScale = Vector3.Lerp(
            text.transform.localScale,
            targetScale,
            Time.deltaTime * speed
        );

        // Alpha suave
        float currentAlpha = Mathf.Lerp(leftImg.color.a, targetAlpha, Time.deltaTime * speed);
        SetAlpha(currentAlpha);

        // Movimiento lateral
        float offset = Mathf.Lerp(0, 20, currentAlpha);

        leftBar.anchoredPosition = leftStartPos + new Vector2(-offset, 0);
        rightBar.anchoredPosition = rightStartPos + new Vector2(offset, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontMaterial = glowMat;
        targetScale = originalScale * scaleMultiplier;
        targetAlpha = 1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontMaterial = normalMat;
        targetScale = originalScale;
        targetAlpha = 0f;
    }

    void SetAlpha(float a)
    {
        Color c1 = leftImg.color;
        c1.a = a;
        leftImg.color = c1;

        Color c2 = rightImg.color;
        c2.a = a;
        rightImg.color = c2;
    }
}