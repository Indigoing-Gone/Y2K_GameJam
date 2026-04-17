using System;
using SerializeReferenceEditor;
using UnityEngine;

[Serializable, SRName("Execute")]
public class ExecuteEffect : ClothingEffect
{
    [Header("Execute Info")]
    [field: SerializeField] public float Threshold { get; private set; }

    protected override void ApplyEffect(Unit _originUnit, Unit _targetUnit)
    {
        if (_targetUnit.Data.PercentHealth() > Threshold) return;
        _targetUnit.Data.TakeDamage(_targetUnit.Data.Health, true);
    }
}