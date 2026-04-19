using System.Collections.Generic;
using UnityEngine;

public class StatusVisuals : MonoBehaviour
{
    [SerializeField] private List<StatusIcon> statusIcons;
    private Dictionary<StatusType, StatusIcon> statusIconMap;

    private void Awake()
    {
        statusIcons = new List<StatusIcon>(GetComponentsInChildren<StatusIcon>());

        statusIconMap = new Dictionary<StatusType, StatusIcon>();
        foreach (var icon in statusIcons)
        {
            statusIconMap[icon.StatusType] = icon;
            icon.gameObject.SetActive(false);
        }
    }

    public void UpdateStatusEffect(StatusType effect, int stackCount)
    {
        if (statusIconMap.TryGetValue(effect, out var icon))
        {
            icon.gameObject.SetActive(stackCount > 0);
            icon.UpdateStacks(stackCount);
        }
    }
}
