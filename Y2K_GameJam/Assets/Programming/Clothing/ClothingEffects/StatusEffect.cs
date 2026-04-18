using System;
using SerializeReferenceEditor;
using UnityEngine;

[Serializable, SRName("Status")]
public class StatusEffect : ClothingEffect
{
    [Header("Status Info")]
    [SerializeField] public StatusType status;
    [SerializeField] public int stacks;

    protected override void ApplyEffect(Unit _originUnit, Unit _targetUnit)
    {
        _targetUnit.Data.GainStatus(stacks, status);
    }
}
