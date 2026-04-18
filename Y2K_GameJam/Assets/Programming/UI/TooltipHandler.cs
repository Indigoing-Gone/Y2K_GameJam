using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TooltipHandler : MonoBehaviour
{
    private RectTransform canvas;
    private RectTransform rectTransform;
    private TextMeshProUGUI tooltipText;
    [SerializeField] private RectTransform tooltipHolder;

    private TooltipData currentTooltipData;

    //[SerializeField] private Vector2 offset;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        tooltipText = tooltipHolder.GetComponentInChildren<TextMeshProUGUI>();

        HideTooltip();
    }

    void Update()
    {
        RaycastHit2D _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
        
        if(_hit.collider == null || 
            !_hit.collider.TryGetComponent<Tooltip>(out var _tooltip) || 
            _tooltip.TooltipData == null || 
            _tooltip.TooltipData.Text == string.Empty)
        {
            HideTooltip();
            return;
        }

        ShowTooltip(_tooltip.TooltipData);
    }

    private void ShowTooltip(TooltipData _tooltipData)
    {
        currentTooltipData = _tooltipData;
        SetText(currentTooltipData.Text);
        SetPosition(currentTooltipData.Position);

        tooltipHolder.gameObject.SetActive(true);
    }

    private void HideTooltip()
    {
        SetText(string.Empty);
        tooltipHolder.gameObject.SetActive(false);
    }

    void SetPosition(Vector3 _position)
    {
        //(Mouse.current.position.ReadValue()) / canvas.localScale.x;
        Vector3 anchoredPosition = _position / canvas.localScale.x;

        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, tooltipHolder.rect.width / 2, canvas.rect.width - (tooltipHolder.rect.width / 2));
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, tooltipHolder.rect.height / 2, canvas.rect.height - (tooltipHolder.rect.height / 2));

        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string _text)
    {
        tooltipText.SetText(_text);
        tooltipText.ForceMeshUpdate();
        Vector2 textSize = tooltipText.GetRenderedValues(false);
        tooltipHolder.sizeDelta = textSize + new Vector2(tooltipText.margin.x, tooltipText.margin.y) * 2f;
    }
}
