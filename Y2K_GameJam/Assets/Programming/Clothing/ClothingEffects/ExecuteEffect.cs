using System;
using SerializeReferenceEditor;
using UnityEngine;

[Serializable, SRName("Execute")]
public class ExecuteEffect : ClothingEffect
{
    [Header("Execute Info")]
    [SerializeField] public float threshold;

    protected override void ApplyEffect(Unit _originUnit, Unit _targetUnit)
    {
        if (_targetUnit.Data.PercentHealth() > threshold) return;
        _targetUnit.Data.TakeDamage(_targetUnit.Data.Health, true);
    }
}