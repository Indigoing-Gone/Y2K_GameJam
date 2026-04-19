using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [field: SerializeField] public float MaxValue { get; private set; }
    [field: SerializeField] public float Value { get; private set; }

    [SerializeField] private RectTransform fullRect;
    [SerializeField] private RectTransform emptyRect;
    [SerializeField] private float animationSpeed = 1f;
    private TextMeshProUGUI valueText;

    private float _fullWidth;
    private float TargetWidth => MaxValue == 0 ? 0 : (Value / MaxValue) * _fullWidth;
    private Coroutine adjustBarCoroutine;

    void Awake()
    {
        valueText = GetComponentInChildren<TextMeshProUGUI>();
        _fullWidth = fullRect.rect.width;
    }

    public void SetMaxValue(float _maxValue)
    {
        MaxValue = _maxValue;
        Value = MaxValue;
    }

    public void UpdateValue(float _newValue)
    {
        float _difference = _newValue - Value;
        Value = Mathf.Clamp(_newValue, 0, MaxValue);
        valueText.text = $"{(int)Value}/{(int)MaxValue}";

        if (adjustBarCoroutine != null) StopCoroutine(adjustBarCoroutine);
        adjustBarCoroutine = StartCoroutine(AdjustBarWidth(_difference));
    }

    private IEnumerator AdjustBarWidth(float _change)
    {
        RectTransform suddenChangeRect = _change >= 0 ? emptyRect : fullRect;
        RectTransform gradualChangeRect = _change >= 0 ? fullRect : emptyRect;
        suddenChangeRect.SetWidth(TargetWidth);

        while(Mathf.Abs(suddenChangeRect.rect.width - gradualChangeRect.rect.width) > 1f)
        {
            gradualChangeRect.SetWidth(Mathf.Lerp(gradualChangeRect.rect.width, TargetWidth, Time.deltaTime * animationSpeed));
            yield return null;
        }
        gradualChangeRect.SetWidth(TargetWidth);
    }
}

public static class RectTransformExtensions
{
    public static void SetWidth(this RectTransform _rectTransform, float _width)
    {
        _rectTransform.sizeDelta = new Vector2(_width, _rectTransform.rect.height);
    }
}