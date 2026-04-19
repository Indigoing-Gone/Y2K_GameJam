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
        dataText.text = $"{_value * 100}%\n{_tempValue * 100}%";
    }
}
