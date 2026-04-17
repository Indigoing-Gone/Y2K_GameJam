using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ModifySteps", menuName = "Scriptable Objects/Clothing Effects/ModifySteps")]
public class ModifyStepsEffect : ClothingEffect
{
    static public Action<Unit, ClothingItem> OnModifiedClothingItemReady;

    [field: SerializeField] public int StepModification { get; private set; }

    protected override void ApplyEffect(Unit _originUnit, Unit _targetUnit)
    {
        List<ClothingItem> _targetItems = new();

        if (TargetSlot == ClothingSlot.All) _targetItems = _targetUnit.AllEquipment();
        else
        {
            ClothingItem _targetItem = _targetUnit.EquipmentInSlot(TargetSlot);
            if (_targetItem == null) return;
            _targetItems.Add(_targetItem);
        }

        foreach (ClothingItem _targetItem in _targetItems)
        {
            _targetItem.ModifySteps(StepModification);
            if (_targetItem.IsReady) OnModifiedClothingItemReady?.Invoke(_targetUnit, _targetItem);
        }
    }
}