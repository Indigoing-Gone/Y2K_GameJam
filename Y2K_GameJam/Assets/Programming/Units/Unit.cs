using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EquipmentVisuals))]
public class Unit : MonoBehaviour
{
    [Header("Components")]
    [field: SerializeField] public UnitData Data { get; private set; }
    private Equipment equipment;
    private EquipmentVisuals equipmentVisuals;

    private void OnEnable()
    {
        equipment.OnEquipmentChanged += UpdateVisuals;
        Data.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        equipment.OnEquipmentChanged -= UpdateVisuals;
        Data.OnDeath -= HandleDeath;
    }

    protected virtual void Awake()
    {
        equipment = new Equipment();
        equipmentVisuals = GetComponent<EquipmentVisuals>();
    }

    protected void UpdateVisuals(ClothingSlot _slot, ClothingItem _item)
    {
        equipmentVisuals.UpdateVisuals(_slot, _item);
    }

    public List<ClothingItem> StepEquipment()
    {
        List<ClothingItem> readyClothingItems = equipment.StepClothing();
        return readyClothingItems;
    }

    protected virtual void HandleDeath(UnitData data)
    {
        Debug.Log($"{Data.Name} has died.");
    }

    public void Equip(ClothingItem _item) => equipment.Equip(_item);
    public void Unequip(ClothingSlot _slot) => equipment.Unequip(_slot);
    public ClothingItem EquipmentInSlot(ClothingSlot _slot) => equipment.EquipmentInSlot(_slot);
    public List<ClothingItem> AllEquipment() => equipment.AllEquipment();
    public void ResetEquipment() => equipment.ResetEquipment();
}

[Serializable]
public class UnitData
{
    public Action<UnitData> OnDeath;

    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int OrderIndex { get; private set; }

    [field: SerializeField] public int MaxHealth { get; private set; }

    [SerializeField] private int health;
    public int Health
    {
        get { return health; }
        private set
        {
            if(IsDead) return;

            health = Mathf.Max(0, value);
            if(health == 0)
            {
                IsDead = true;
                OnDeath?.Invoke(this);
            }
        }
    }
    public bool IsDead { get; private set; }

    [field: SerializeField] public float AttackMultiplier { get; private set; } = 1.0f;
    [field: SerializeField] public float TempAttackMultiplier { get; private set; } = 0f;
    [SerializeField] private float defenseMultiplier = 1.0f;
    [SerializeField] private float tempDefenseMultiplier = 0f;

    public UnitData(string _name = "Unit", int _index = -1, int _maxHealth = 100)
    {
        Name = _name;
        OrderIndex = _index;

        MaxHealth = _maxHealth;
        Health = MaxHealth;
        
        AttackMultiplier = 1.0f;
        defenseMultiplier = 1.0f;
    }

    public void UpdateOrderIndex(int _index) => OrderIndex = _index;
    public void TakeDamage(int _damage, bool _isTrueDamage)
    {
        float damage = _damage;
        if (!_isTrueDamage) damage /= defenseMultiplier + tempDefenseMultiplier;
        Debug.Log(_damage);
        Debug.Log(defenseMultiplier + tempDefenseMultiplier);
        Debug.Log(damage);
        Health -= (int)damage;
    } 

    // return float value representing percent health remaining
    public float PercentHealth() => (float)Health/MaxHealth;

    public void Reset()
    {
        IsDead = false;
        Health = MaxHealth;

        AttackMultiplier = 1.0f;
        defenseMultiplier = 1.0f;

        TempAttackMultiplier = 0f;
        tempDefenseMultiplier = 0f;
    }

    public void StatusUpdate()
    {
        TempAttackMultiplier = 0f;
        tempDefenseMultiplier = 0f;
    }

    // STATUS EFFECTS
    public void AdjustAttack(float adjustment, bool temporary)
    {
        if (temporary) TempAttackMultiplier += adjustment;
        else AttackMultiplier += adjustment;
    }

    public void AdjustDefense(float adjustment, bool temporary)
    {
        if (temporary) tempDefenseMultiplier += adjustment;
        else defenseMultiplier += adjustment;
    }
}