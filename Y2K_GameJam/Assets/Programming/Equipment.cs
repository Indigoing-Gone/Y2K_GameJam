using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    public event Action<ClothingItem, ClothingItem> OnEquipmentChanged;
    private Dictionary<ClothingSlot, ClothingItem> equippedClothing = new();

    public Equipment()
    {
        foreach (ClothingSlot _slot in Enum.GetValues(typeof(ClothingSlot)))
        {
            if (_slot == ClothingSlot.Default) continue;
            equippedClothing[_slot] = null;
        }
    }

    public void Equip(ClothingItem _newItem)
    {
        ClothingSlot _slot = _newItem.Slot;
        ClothingItem _oldItem = null;

        if (equippedClothing.ContainsKey(_slot)) _oldItem = equippedClothing[_slot];


        equippedClothing[_slot] = _newItem;
        OnEquipmentChanged?.Invoke(_oldItem, _newItem);
    }

    public void Unequip(ClothingSlot _slot)
    {
        if (!equippedClothing.ContainsKey(_slot)) return;

        ClothingItem oldItem = equippedClothing[_slot];
        equippedClothing.Remove(_slot);

        OnEquipmentChanged?.Invoke(oldItem, null);
    }

    public void DisplayEquipment()
    {
        foreach (KeyValuePair<ClothingSlot, ClothingItem> _entry in equippedClothing)
        {
            if (_entry.Value != null) Debug.Log($"Equipped {_entry.Value.Name} in slot {_entry.Key}");
            else Debug.Log($"No item equipped in slot {_entry.Key}");
        }
    }
}