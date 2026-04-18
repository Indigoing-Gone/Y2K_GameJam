using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType
{
    Thorns,
    Burn,
    Patience
}

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
        Data.Init();
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

            health = Mathf.Max(0, Mathf.Min(value, MaxHealth));
            if(health == 0)
            {
                IsDead = true;
                OnDeath?.Invoke(this);
            }
        }
    }
    public bool IsDead { get; private set; }


    // STATUS EFFECTS
    [field: SerializeField] public float AttackMultiplier { get; private set; } = 1.0f;
    [field: SerializeField] public float TempAttackMultiplier { get; private set; } = 0f;
    [SerializeField] private float defenseMultiplier = 1.0f;
    [SerializeField] private float tempDefenseMultiplier = 0f;
    private Dictionary<StatusType, int> statuses;

    public void Init()
    {
        statuses = new Dictionary<StatusType, int>() {
            {StatusType.Thorns, 0}, {StatusType.Burn, 0}, {StatusType.Patience, 0}
        };
    }
    
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
        if (!_isTrueDamage) damage /= Mathf.Max(0.5f, defenseMultiplier + tempDefenseMultiplier);
        Health -= (int)damage;
    } 

    public void Heal(int _healing) => Health += _healing;

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

        foreach (KeyValuePair<StatusType, int> _status in statuses) statuses[_status.Key] = 0;

    }

    public void StatusUpdate()
    {
        //remove temps stat changes
        TempAttackMultiplier = 0f;
        tempDefenseMultiplier = 0f;

        //remove all thorns
        statuses[StatusType.Thorns] = 0;
        
        //deal burn damage and reduce stacks
        TakeDamage(statuses[StatusType.Burn], false);
        statuses[StatusType.Burn] = Mathf.Max(0, statuses[StatusType.Burn] - 10);

        //double patience
        statuses[StatusType.Patience] *= 2;
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

    public void GainStatus(int stacks, StatusType status) => statuses[status] += stacks;

    public int GetStatusStacks(StatusType status) => statuses[status];

    public void ClearStatus(StatusType status) => statuses[status] = 0;
}