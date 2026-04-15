using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IClothingEffect
{
    void ActivateEffect(Unit _originUnit, BattleContext _battleContext);
}

public abstract class ClothingEffect : ScriptableObject, IClothingEffect
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

    [Header("Effect Info")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [Header("Targeting")]
    [field: SerializeField] public EffectTargetType TargetType { get; private set; }
    [field: SerializeField] public EffectTargetPosition TargetPosition { get; private set; }
    [field: SerializeField] public ClothingSlot TargetSlot { get; private set; }

    public virtual void ActivateEffect(Unit _originUnit, BattleContext _battleContext)
    {
        Debug.Log($"Activating {Name} from {_originUnit.name}");

        List<Unit> targets = GetTargetUnits(_originUnit, _battleContext);

        foreach (Unit target in targets)
        {
            if (target == null) continue;
            ApplyEffect(target);
        }
    }

    protected virtual List<Unit> GetTargetUnits(Unit _originUnit, BattleContext _battleContext)
    {
        List<Unit> targetList = TargetType switch
        {
            EffectTargetType.Self => new List<Unit>() { _originUnit },
            EffectTargetType.Allies => _battleContext.GetAllies(_originUnit),
            EffectTargetType.Enemies => _battleContext.GetEnemies(_originUnit),
            _ => throw new ArgumentOutOfRangeException()
        };

        targetList = TargetPosition switch
        {
            EffectTargetPosition.First => _battleContext.GetFront(targetList),
            EffectTargetPosition.Last => _battleContext.GetBack(targetList),
            EffectTargetPosition.All => targetList,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        if (TargetSlot != ClothingSlot.None)
        {
            if (TargetSlot == ClothingSlot.All) targetList = targetList.Where(target => target.AllEquippedItems().Count > 0).ToList();
            targetList = targetList.Where(target => target.EquippedItemInSlot(TargetSlot) != null).ToList();
        }

        return targetList;
    }

    protected virtual void ApplyEffect(Unit _targetUnit)
    {
        Debug.Log($"Applying {Name} to {_targetUnit.name}");
    }
}