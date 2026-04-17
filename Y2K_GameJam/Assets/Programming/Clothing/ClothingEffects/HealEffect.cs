using System;
using SerializeReferenceEditor;
using UnityEngine;

[Serializable, SRName("Heal")]
public class HealEffect : ClothingEffect
{
    [Header("Healing Info")]
    [SerializeField] public int healing;

    protected override void ApplyEffect(Unit _originUnit, Unit _targetUnit)
    {
        _targetUnit.Data.Heal(healing);
    }
}
