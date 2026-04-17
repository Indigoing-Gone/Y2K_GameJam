using System;
using SerializeReferenceEditor;
using UnityEngine;

//[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Clothing Effects/Attack")]
[Serializable, SRName("Attack")]
public class AttackEffect : ClothingEffect
{
    [Header("Attack Info")]
    [field: SerializeField] public int Damage { get; private set; }

    protected override void ApplyEffect(Unit _originUnit, Unit _targetUnit)
    {
        int _finalDamage = Mathf.RoundToInt(Damage * _originUnit.Data.AttackMultiplier);
        _targetUnit.Data.TakeDamage(_finalDamage, false);
    }
}