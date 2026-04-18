using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TooltipHandler : MonoBehaviour
{
    private RectTransform canvas;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform tooltipBackground;
    private TextMeshProUGUI tooltipText;

    [SerializeField] private Vector2 offset;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        tooltipText = GetComponentInChildren<TextMeshProUGUI>();
        tooltipBackground = tooltipText.transform.parent.GetComponent<RectTransform>();

        SetText(tooltipBackground.name);
    }

    private void SetText(string _text)
    {
        tooltipText.SetText(_text);
        tooltipText.ForceMeshUpdate();
        Vector2 textSize = tooltipText.GetRenderedValues(false);
        tooltipBackground.sizeDelta = textSize + new Vector2(tooltipText.margin.x, tooltipText.margin.y) * 2f;
    }

    void Update()
    {
        Vector2 anchoredPosition = (Mouse.current.position.ReadValue() + offset) / canvas.localScale.x;

        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, tooltipBackground.rect.width / 2, canvas.rect.width - (tooltipBackground.rect.width / 2));
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, tooltipBackground.rect.height / 2, canvas.rect.height - (tooltipBackground.rect.height / 2));

        rectTransform.anchoredPosition = anchoredPosition;
    }
}
