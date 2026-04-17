using System;
using SerializeReferenceEditor;
using UnityEngine;

//[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Clothing Effects/Attack")]
[Serializable, SRName("Attack")]
public class AttackEffect : ClothingEffect
{
    [Header("Attack Info")]
    [SerializeField] public int damage;

    protected override void ApplyEffect(Unit _originUnit, Unit _targetUnit)
    {
        int _finalDamage = Mathf.RoundToInt(damage * (_originUnit.Data.AttackMultiplier + _originUnit.Data.TempAttackMultiplier));
        _targetUnit.Data.TakeDamage(_finalDamage, false);
    }
}