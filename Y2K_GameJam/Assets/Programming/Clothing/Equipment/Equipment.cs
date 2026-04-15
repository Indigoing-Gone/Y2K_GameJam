using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    public event Action<ClothingItem> OnEquipmentChanged;

    private SortedDictionary<ClothingSlot, ClothingItem> equipment;

    public Equipment()
    {
        equipment = new SortedDictionary<ClothingSlot, ClothingItem>();

        foreach (ClothingSlot _slot in Enum.GetValues(typeof(ClothingSlot)))
        {
            if (_slot == ClothingSlot.None || _slot == ClothingSlot.All) continue;
            equipment[_slot] = null;
        }
    }

    public void Equip(ClothingItem _newItem)
    {
        ClothingSlot _slot = _newItem.Data.Slot;
        ClothingItem _oldItem = null;

        if (equipment.ContainsKey(_slot)) _oldItem = equipment[_slot];

        equipment[_slot] = _newItem;
        OnEquipmentChanged?.Invoke(_newItem);
    }

    public void Unequip(ClothingSlot _slot)
    {
        if (!equipment.ContainsKey(_slot)) return;

        ClothingItem oldItem = equipment[_slot];
        equipment[_slot] = null;

        OnEquipmentChanged?.Invoke(null);
    }

    public ClothingItem EquippedItemInSlot(ClothingSlot slot)
    {
        if (!equipment.ContainsKey(slot)) return null;
        return equipment[slot];
    }

    public List<ClothingItem> AllEquippedItems()
    {
        List<ClothingItem> _equipment = new();
        foreach (KeyValuePair<ClothingSlot, ClothingItem> _entry in equipment)
        {
            if (_entry.Value == null) continue;
            _equipment.Add(_entry.Value);
        }
        return _equipment;
    }

    public List<ClothingItem> StepClothing()
    {
        List<ClothingItem> readyClothingItems = new();
        foreach (KeyValuePair<ClothingSlot, ClothingItem> _entry in equipment)
        {
            if (_entry.Value == null) continue;

            ClothingItem _item = _entry.Value;
            _item.ModifySteps(-1);
            if (_item.IsReady) readyClothingItems.Add(_item);
        }
        return readyClothingItems;
    }
}