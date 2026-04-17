using System;
using SerializeReferenceEditor;
using UnityEngine;

//[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Clothing Effects/Attack")]
[Serializable, SRName("Defense Modifier")]
public class AdjustDefEffect : ClothingEffect
{
    [Header("Attack Multiplier Adjustment Info")]
    [SerializeField] public float adjustment;
    [SerializeField] public bool temporary;

    protected override void ApplyEffect(Unit _originUnit, Unit _targetUnit)
    {
        _targetUnit.Data.AdjustDefense(adjustment, temporary);
    }
}
