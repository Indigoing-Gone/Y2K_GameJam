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


public class Tooltip : MonoBehaviour
{


    [field: SerializeField] public TooltipData TooltipData { get; private set; }
    [SerializeField] private Vector3 Offset;

    void Awake() => SetTooltipPosition(Camera.main.WorldToScreenPoint(transform.position + Offset));

    public void SetTooltipText(string _text) => TooltipData.Text = _text;
    public void SetTooltipPosition(Vector3 _position) => TooltipData.Position = _position;
}