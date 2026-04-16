using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    [SerializeField] private List<ClothingData> startingClothingItems;
    private Dictionary<ClothingSlot, List<ClothingItem>> availableClothing;

    private bool canShiftWardrobe;

    void OnEnable()
    {
        WardrobeSlot.OnShiftingWardrobe += ShiftWardrobe;
    }

    void OnDisable()
    {
        WardrobeSlot.OnShiftingWardrobe -= ShiftWardrobe;
    }

    private void Awake()
    {
        canShiftWardrobe = true;
        availableClothing = new Dictionary<ClothingSlot, List<ClothingItem>>();

        foreach (ClothingSlot _slot in System.Enum.GetValues(typeof(ClothingSlot)))
        {
            if (_slot == ClothingSlot.None || _slot == ClothingSlot.All) continue;
            availableClothing[_slot] = new List<ClothingItem> { null };
        }

        foreach (ClothingData _data in startingClothingItems) AddClothingItem(new ClothingItem(_data));
    }

    public void AddClothingItem(ClothingItem _item)
    {
        ClothingSlot _slot = _item.Data.Slot;
        if (!availableClothing.ContainsKey(_slot)) return;

        availableClothing[_slot].Add(_item);
    }


    private void ShiftWardrobe(int _direction, ClothingSlot _slot, Unit _unit)
    {
        if (!canShiftWardrobe || !availableClothing.ContainsKey(_slot)) return;

        List<ClothingItem> _clothingItems = availableClothing[_slot];
        if (_clothingItems.Count == 0) return;

        ClothingItem _currentItem = _unit.EquipmentInSlot(_slot);
        int _currentIndex = _clothingItems.IndexOf(_currentItem);

        bool _foundUnequippedItem = false;

        while (!_foundUnequippedItem)
        {
            _currentIndex = (_currentIndex + _direction) % _clothingItems.Count;
            if (_currentIndex < 0) _currentIndex += _clothingItems.Count;

            ClothingItem _potentialItem = _clothingItems[_currentIndex];
            if (_potentialItem == null || !_potentialItem.IsEquipped) _foundUnequippedItem = true;
        }

        ClothingItem _newItem = _clothingItems[_currentIndex];

        if (_newItem == _currentItem) return;
        else if (_newItem == null) _unit.Unequip(_slot);
        else _unit.Equip(_newItem);
    }
}
