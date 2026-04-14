using System;
using System.Collections.Generic;
using UnityEngine;

public interface IClothingEffect
{
    void ActivateEffect(Character _originCharacter, (List<Character> _allies, List<Character> _enemies) _characterLists);
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

    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField] public EffectTargetType TargetType { get; private set; }
    [field: SerializeField] public EffectTargetPosition TargetPosition { get; private set; }

    public void ActivateEffect(Character _originCharacter, (List<Character> _allies, List<Character> _enemies) _characterLists)
    {
        List<Character> targets = IdentifyTarget(_originCharacter, _characterLists);

        foreach (Character target in targets)
        {
            if (target == null) continue;
            ApplyEffect(target);
        }
    }

    protected List<Character> IdentifyTarget(Character _originCharacter, (List<Character> _allies, List<Character> _enemies) _characterLists)
    {
        List<Character> targetList = TargetType switch
        {
            EffectTargetType.Self => new List<Character>() { _originCharacter },
            EffectTargetType.Allies => _characterLists._allies,
            EffectTargetType.Enemies => _characterLists._enemies,
            _ => throw new ArgumentOutOfRangeException()
        };

        return TargetPosition switch
        {
            EffectTargetPosition.First => new List<Character>() { targetList[0] },
            EffectTargetPosition.Last => new List<Character>() { targetList[^1] },
            EffectTargetPosition.All => targetList,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected virtual void ApplyEffect(Character _targetCharacter)
    {
        Debug.Log($"Applying {Name} to {_targetCharacter.name}");
    }
}

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Clothing Effects/Attack")]
public class AttackEffect : ClothingEffect, IClothingEffect
{
    [field: SerializeField] public int Damage { get; private set; }

    protected override void ApplyEffect(Character _targetCharacter)
    {
        _targetCharacter.TakeDamage(Damage);
    }
}