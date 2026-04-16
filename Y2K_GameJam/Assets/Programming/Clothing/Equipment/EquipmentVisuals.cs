using UnityEngine;

public class EquipmentVisuals : MonoBehaviour
{
    [SerializeField] private EquipmentSlotVisuals[] slotVisuals;

    public void UpdateVisuals(ClothingSlot _slot, ClothingItem _item)
    {
        foreach (EquipmentSlotVisuals _slotVisual in slotVisuals)
        {
            if(_slot != _slotVisual.ClothingSlot && _slot != ClothingSlot.All) continue;
            _slotVisual.UpdateVisual(_item);
        }
    }
}
