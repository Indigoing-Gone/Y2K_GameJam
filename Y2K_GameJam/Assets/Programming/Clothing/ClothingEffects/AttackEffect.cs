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
        float _multiplier = Mathf.Max(0.5f, _originUnit.Data.AttackMultiplier 
                                         + _originUnit.Data.TempAttackMultiplier
                                         + _originUnit.Data.GetStatusStacks(StatusType.Chillax) * .1f);
        int _finalDamage = Mathf.RoundToInt(damage * _multiplier);
        
        _originUnit.Data.ClearStatus(StatusType.Chillax);
        _targetUnit.Data.TakeDamage(_finalDamage, false);

        int _BackstabDamage = _targetUnit.Data.GetStatusStacks(StatusType.Backstab);
        if (_BackstabDamage > 0)
        {
            _multiplier = Mathf.Max(0.5f, _targetUnit.Data.AttackMultiplier 
                                        + _targetUnit.Data.TempAttackMultiplier);
            _originUnit.Data.TakeDamage(Mathf.RoundToInt(_BackstabDamage * _multiplier), false);
        }
    }
}