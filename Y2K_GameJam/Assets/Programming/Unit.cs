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
    }

    private void OnDisable()
    {
        equipment.OnEquipmentChanged -= UpdateVisuals;
    }

    protected virtual void Awake()
    {
        equipment = new Equipment();
        equipmentVisuals = GetComponent<EquipmentVisuals>();
    }

    private void Start()
    {
        UpdateVisuals(null);
    }

    private void UpdateVisuals(ClothingItem _item)
    {
        equipmentVisuals.UpdateVisuals(_item);
    }

    public List<ClothingItem> StepEquipment()
    {
        List<ClothingItem> readyClothingItems = equipment.StepClothing();
        return readyClothingItems;
    }

    public void Equip(ClothingItem _item) => equipment.Equip(_item);
    public void Unequip(ClothingSlot _slot) => equipment.Unequip(_slot);
    public ClothingItem EquipmentInSlot(ClothingSlot _slot) => equipment.EquipmentInSlot(_slot);
    public List<ClothingItem> AllEquipment() => equipment.AllEquipment();
}

[Serializable]
public class UnitData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int OrderIndex { get; private set; }

    [field: SerializeField] public int Health { get; private set; }
    public bool IsDead { get { return Health <= 0; } }

    [field: SerializeField] public float AttackMultiplier { get; private set; }

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