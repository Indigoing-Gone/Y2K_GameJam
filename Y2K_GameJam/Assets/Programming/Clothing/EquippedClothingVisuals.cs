using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class EquippedClothingVisuals : MonoBehaviour
{
    [SerializeField] private EquippedClothingSlotVisuals[] slotVisuals;

    public void UpdateVisuals(ClothingItem _item)
    {
        foreach (EquippedClothingSlotVisuals _slotVisual in slotVisuals) _slotVisual.UpdateVisual(_item);
    }

    public void UpdateSteps()
    {
        foreach (EquippedClothingSlotVisuals _slotVisual in slotVisuals) _slotVisual.UpdateSteps();
    }
}
