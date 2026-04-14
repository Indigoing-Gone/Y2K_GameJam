using System;
using System.Collections.Generic;
using UnityEngine;

public class EquippedClothing
{
    public event Action<ClothingItem> OnEquippedClothingChanged;

    private Dictionary<ClothingSlot, ClothingItem> equippedClothing;

    public EquippedClothing()
    {
        equippedClothing = new Dictionary<ClothingSlot, ClothingItem>();

        foreach (ClothingSlot _slot in Enum.GetValues(typeof(ClothingSlot)))
        {
            if (_slot == ClothingSlot.Default) continue;
            equippedClothing[_slot] = null;
        }
    }

    public void Equip(ClothingItem _newItem)
    {
        ClothingSlot _slot = _newItem.Data.Slot;
        ClothingItem _oldItem = null;

        if (equippedClothing.ContainsKey(_slot)) _oldItem = equippedClothing[_slot];

        equippedClothing[_slot] = _newItem;
        OnEquippedClothingChanged?.Invoke(_newItem);
    }

    public void Unequip(ClothingSlot _slot)
    {
        if (!equippedClothing.ContainsKey(_slot)) return;

        ClothingItem oldItem = equippedClothing[_slot];
        equippedClothing.Remove(_slot);

        OnEquippedClothingChanged?.Invoke(null);
    }

    public void TakeStep()
    {
        foreach (KeyValuePair<ClothingSlot, ClothingItem> _entry in equippedClothing)
        {
            if (_entry.Value == null) continue;
            ClothingItem _item = _entry.Value;
            _item.TakeStep();
        }
    }
}