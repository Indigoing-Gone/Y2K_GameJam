using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatusType
{
    Backstab,
    Burn,
    Chillax
}

[RequireComponent(typeof(EquipmentVisuals))]
public class Unit : MonoBehaviour
{
    [Header("Components")]
    [field: SerializeField] public UnitData Data { get; private set; }
    private Bar healthBar;
    [SerializeField] private DataText attackMultiplierDataText;
    [SerializeField] private DataText defenseMultiplierDataText;
    private Equipment equipment;
    private EquipmentVisuals equipmentVisuals;
    [SerializeField] private StatusVisuals statusVisuals;

    private void OnEnable()
    {
        equipment.OnEquipmentChanged += UpdateVisuals;
        Data.OnDeath += HandleDeath;
        Data.OnHealthChanged += healthBar.UpdateValue;
        Data.OnMultiplierChanged += UpdateMultiplierDataTexts;
        Data.OnStatusChanged += statusVisuals.UpdateStatusEffect;
    }

    private void OnDisable()
    {
        equipment.OnEquipmentChanged -= UpdateVisuals;
        Data.OnDeath -= HandleDeath;
        Data.OnHealthChanged -= healthBar.UpdateValue;
        Data.OnMultiplierChanged -= UpdateMultiplierDataTexts;
        Data.OnStatusChanged -= statusVisuals.UpdateStatusEffect;
    }

    protected virtual void Awake()
    {
        equipment = new Equipment();
        equipmentVisuals = GetComponent<EquipmentVisuals>();
        healthBar = GetComponentInChildren<Bar>();
        statusVisuals = GetComponentInChildren<StatusVisuals>();
    }

    protected virtual void Start()
    {
        Data.Init();
        healthBar.SetMaxValue(Data.MaxHealth);
        healthBar.UpdateValue(Data.Health);
    }

    protected void UpdateVisuals(ClothingSlot _slot, ClothingItem _item)
    {
        equipmentVisuals.UpdateVisuals(_slot, _item);
    }

    private void UpdateMultiplierDataTexts((float, float) values, bool isAttack)
    {
        if(attackMultiplierDataText == null || defenseMultiplierDataText == null) return;
        if (isAttack) attackMultiplierDataText.UpdateData(values.Item1, values.Item2);
        else defenseMultiplierDataText.UpdateData(values.Item1, values.Item2);
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
    public event Action<float> OnHealthChanged;
    public event Action<UnitData> OnDeath;
    public event Action<(float, float), bool> OnMultiplierChanged; //bool = isAttackMultiplier
    public event Action<StatusType, int> OnStatusChanged; //int = new stack count

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
            OnHealthChanged?.Invoke(health);
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
            {StatusType.Backstab, 0}, {StatusType.Burn, 0}, {StatusType.Chillax, 0}
        };

        Reset();
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

        OnMultiplierChanged?.Invoke((AttackMultiplier, TempAttackMultiplier), true);
        OnMultiplierChanged?.Invoke((defenseMultiplier, tempDefenseMultiplier), false);

        foreach (StatusType _status in Enum.GetValues(typeof(StatusType))) ClearStatus(_status);
    }

    public void StatusUpdate()
    {
        //remove temps stat changes
        TempAttackMultiplier = 0f;
        tempDefenseMultiplier = 0f;

        OnMultiplierChanged?.Invoke((AttackMultiplier, TempAttackMultiplier), true);
        OnMultiplierChanged?.Invoke((defenseMultiplier, tempDefenseMultiplier), false);

        //remove all Backstab
        ClearStatus(StatusType.Backstab);

        //deal burn damage and reduce stacks
        TakeDamage(statuses[StatusType.Burn], false);
        GainStatus(-5, StatusType.Burn);

        //double Chillax
        GainStatus(statuses[StatusType.Chillax], StatusType.Chillax);
    }

    // STATUS EFFECTS
    public void AdjustAttack(float adjustment, bool temporary)
    {
        if (temporary) TempAttackMultiplier += adjustment;
        else AttackMultiplier += adjustment;

        OnMultiplierChanged?.Invoke((AttackMultiplier, TempAttackMultiplier), true);
    }
    public void AdjustDefense(float adjustment, bool temporary)
    {
        if (temporary) tempDefenseMultiplier += adjustment;
        else defenseMultiplier += adjustment;

        OnMultiplierChanged?.Invoke((defenseMultiplier, tempDefenseMultiplier), false);
    }
    public void GainStatus(int stacks, StatusType status)
    {
        statuses[status] += stacks;
        statuses[status] = Mathf.Max(0, statuses[status]);
        OnStatusChanged?.Invoke(status, statuses[status]);
    }
    public int GetStatusStacks(StatusType status) => statuses[status];
    public void ClearStatus(StatusType status)
    {
        statuses[status] = 0;
        OnStatusChanged?.Invoke(status, 0);
    }
}