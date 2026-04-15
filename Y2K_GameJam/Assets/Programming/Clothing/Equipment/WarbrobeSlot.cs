using System;
using UnityEngine;

public class WardrobeSlot : MonoBehaviour
{
    static public Action<int, ClothingSlot, Unit> OnShiftingWardrobe;

    [SerializeField] private ClothingSlot clothingSlot;
    private Unit unit;    

    public void Init(Unit _unit)
    {
        unit = _unit;
    }

    public void ShiftWardrobe(int _direction)
    {
        OnShiftingWardrobe?.Invoke(_direction, clothingSlot, unit);
    }
}