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

    public UnitData(string _name = "Unit", int _index = -1, int _health = 100)
    {
        Name = _name;
        OrderIndex = _index;

        Health = _health;
        
        AttackMultiplier = 1.0f;
    }

    public void UpdateOrderIndex(int _index) => OrderIndex = _index;
    public void TakeDamage(int _damage) => Health -= _damage;
}