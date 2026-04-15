using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Clothing Effects/Attack")]
public class AttackEffect : ClothingEffect
{
    [Header("Attack Info")]
    [field: SerializeField] public int Damage { get; private set; }

    protected override void ApplyEffect(Unit _targetUnit)
    {
        _targetUnit.Data.TakeDamage(Damage);
    }
}