using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ModifySteps", menuName = "Scriptable Objects/Clothing Effects/ModifySteps")]
public class ModifyStepsEffect : ClothingEffect
{
    static public event Action<Unit, ClothingItem> OnModifiedClothingItemReady;

    [field: SerializeField] public int StepModification { get; private set; }

    protected override void ApplyEffect(Unit _originUnit, Unit _targetUnit)
    {
        ClothingItem _targetItem = _targetUnit.EquipmentInSlot(TargetSlot);
        if (_targetItem == null) return;

        for (int i = 0; i < Math.Abs(StepModification); i++)
        {
            if (StepModification < 0) _targetItem.ModifySteps(-1);
            else _targetItem.ModifySteps(1);
        }

        if (_targetItem.IsReady) OnModifiedClothingItemReady?.Invoke(_targetUnit, _targetItem);
    }
}