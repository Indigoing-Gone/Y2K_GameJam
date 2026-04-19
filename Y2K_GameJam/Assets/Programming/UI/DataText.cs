using TMPro;
using UnityEngine;

public class DataText : MonoBehaviour
{
    private TextMeshProUGUI dataText;

    void Awake()
    {
        dataText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateData(float _value, float _tempValue)
    {
        dataText.text = $"{(int)(_value * 100)}%\n{(int)(_tempValue * 100)}%";
    }
}
