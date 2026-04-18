using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class TooltipData
{
    public string Text;
    public Vector3 Position;

    public TooltipData(string _text, Vector3 _position)
    {
        Text = _text;
        Position = _position;
    }
}


public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<TooltipData> OnShowTooltip;
    public static event Action OnHideTooltip;

    [field: SerializeField] public TooltipData TooltipData { get; private set; }
    [SerializeField] private Vector3 Offset;

    void Awake() => TooltipData = new TooltipData(string.Empty, transform.position + Offset);

    public void SetTooltipText(string _text) => TooltipData.Text = _text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Showing tooltip with text: {TooltipData.Text} at position: {TooltipData.Position}");
        OnShowTooltip?.Invoke(TooltipData);
    }
    public void OnPointerExit(PointerEventData eventData) => OnHideTooltip?.Invoke();
}