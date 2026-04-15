using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class EquipmentVisuals : MonoBehaviour
{
    [SerializeField] private EquipmentSlotVisuals[] slotVisuals;

    public void UpdateVisuals(ClothingItem _item)
    {
        foreach (EquipmentSlotVisuals _slotVisual in slotVisuals) _slotVisual.UpdateVisual(_item);
    }

    // public void UpdateSteps()
    // {
    //     foreach (EquipmentSlotVisuals _slotVisual in slotVisuals) _slotVisual.UpdateSteps();
    // }
}
