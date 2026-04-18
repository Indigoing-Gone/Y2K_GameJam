using TMPro;
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

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        tooltipText = GetComponentInChildren<TextMeshProUGUI>();
        tooltipBackground = tooltipText.transform.parent.GetComponent<RectTransform>();

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

        tooltipBackground.gameObject.SetActive(true);
    }

    private void HideTooltip()
    {
        SetText(string.Empty);
        tooltipBackground.gameObject.SetActive(false);
    }

    void SetPosition(Vector3 _position)
    {
        //(Mouse.current.position.ReadValue()) / canvas.localScale.x;
        Vector3 anchoredPosition = _position / canvas.localScale.x;

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
}
