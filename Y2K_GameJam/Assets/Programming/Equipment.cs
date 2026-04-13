using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public event Action<ClothingItem, ClothingItem> OnEquipmentChanged;
    private Dictionary<ClothingType, ClothingItem> equippedClothing = new();

    public void Equip(ClothingItem _newItem)
    {
        ClothingType _type = _newItem.Type;
        ClothingItem _oldItem = null;

        if (equippedClothing.ContainsKey(_type)) _oldItem = equippedClothing[_type];


        equippedClothing[_type] = _newItem;
        OnEquipmentChanged?.Invoke(_oldItem, _newItem);
    }

    public void Unequip(ClothingType _type)
    {
        if (!equippedClothing.ContainsKey(_type)) return;

        ClothingItem oldItem = equippedClothing[_type];
        equippedClothing.Remove(_type);

        OnEquipmentChanged?.Invoke(oldItem, null);
    }
}