using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class TooltipHandler : MonoBehaviour
{
    private RectTransform canvas;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform tooltipBackground;
    private TextMeshProUGUI tooltipText;

    private TooltipData currentTooltipData;

    //[SerializeField] private Vector2 offset;

    void OnEnable()
    {
        OnShowTooltip += ShowTooltip;
        OnHideTooltip += HideTooltip;
    }

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        tooltipText = GetComponentInChildren<TextMeshProUGUI>();
        tooltipBackground = tooltipText.transform.parent.GetComponent<RectTransform>();

        HideTooltip();
    }

    void SetPosition(Vector3 _position)
    {
        //(Mouse.current.position.ReadValue()) / canvas.localScale.x;
        Vector2 anchoredPosition = _position;

        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, tooltipBackground.rect.width / 2, canvas.rect.width - (tooltipBackground.rect.width / 2));
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, tooltipBackground.rect.height / 2, canvas.rect.height - (tooltipBackground.rect.height / 2));

        rectTransform.anchoredPosition = anchoredPosition;
    }


    private void SetText(string _text)
    {
        tooltipText.SetText(_text);
        tooltipText.ForceMeshUpdate();
        Vector2 textSize = tooltipText.GetRenderedValues(false);
        tooltipBackground.sizeDelta = textSize + new Vector2(tooltipText.margin.x, tooltipText.margin.y) * 2f;
    }

    private void ShowTooltip(TooltipData _tooltipData)
    {
        currentTooltipData = _tooltipData;
        SetText(currentTooltipData.Text);
        SetPosition(currentTooltipData.Position);

        tooltipBackground.gameObject.SetActive(true);
    }

    private void HideTooltip()
    {
        SetText(string.Empty);
        tooltipBackground.gameObject.SetActive(false);
    }
}
