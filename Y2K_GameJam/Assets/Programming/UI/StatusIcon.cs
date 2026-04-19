using TMPro;
using UnityEngine;

public class StatusIcon : MonoBehaviour
{
    [field: SerializeField] public StatusType StatusType { get; private set; }
    [SerializeField] private TextMeshProUGUI stacks;

    void Awake()
    {
        stacks = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateStacks(int stackCount)
    {
        if (stackCount > 1)
        {
            stacks.text = stackCount.ToString();
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
