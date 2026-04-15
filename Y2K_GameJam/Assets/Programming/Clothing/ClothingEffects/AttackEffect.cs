using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Clothing Effects/Attack")]
public class AttackEffect : ClothingEffect
{
    [Header("Attack Info")]
    [field: SerializeField] public int Damage { get; private set; }

    protected override void ApplyEffect(Unit _originUnit, Unit _targetUnit)
    {
        int _finalDamage = Mathf.RoundToInt(Damage * _originUnit.Data.AttackModifier);
        _targetUnit.Data.TakeDamage(_finalDamage);
    }
}