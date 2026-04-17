using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class ClothingEffect
{
    public enum EffectTargetType
    {
        Self,
        Allies,
        Enemies
    }

    public enum EffectTargetPosition
    {
        First,
        Last,
        All
    }

    [Header("Targeting")]
    [SerializeField] public EffectTargetType targetType;
    [SerializeField] public EffectTargetPosition targetPosition;
    [SerializeField] public ClothingSlot TargetSlot;

    public virtual IEnumerator ActivateEffect(Unit _originUnit, EncounterContext _context)
    {
        //Debug.Log($"Activating {Name} from {_originUnit.name}");

        List<Unit> targets = GetTargetUnits(_originUnit, _context);

        foreach (Unit target in targets)
        {
            if (target == null) continue;
            ApplyEffect(_originUnit, target);
        }

        yield return null;
    }

    protected virtual List<Unit> GetTargetUnits(Unit _originUnit, EncounterContext _context)
    {
        List<Unit> targetList = targetType switch
        {
            EffectTargetType.Self => new List<Unit>() { _originUnit },
            EffectTargetType.Allies => _context.GetAllies(_originUnit),
            EffectTargetType.Enemies => _context.GetEnemies(_originUnit),
            _ => throw new ArgumentOutOfRangeException()
        };

        targetList = targetPosition switch
        {
            EffectTargetPosition.First => _context.GetFront(targetList),
            EffectTargetPosition.Last => _context.GetBack(targetList),
            EffectTargetPosition.All => targetList,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        if (TargetSlot != ClothingSlot.None)
        {
            if (TargetSlot == ClothingSlot.All) targetList = targetList.Where(target => target.AllEquipment().Count > 0).ToList();
            else targetList = targetList.Where(target => target.EquipmentInSlot(TargetSlot) != null).ToList();
        }

        return targetList;
    }

    protected abstract void ApplyEffect(Unit _originUnit, Unit _targetUnit);
}