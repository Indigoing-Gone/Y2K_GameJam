using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EquippedClothingVisuals))]
public class Character : MonoBehaviour
{
    [SerializeField] private List<ClothingData> testClothingItems;

    [SerializeField] private CharacterData characterData;
    private EquippedClothing equippedClothing;
    protected EquippedClothingVisuals equippedClothingVisuals;

    protected List<ClothingItem> readyClothing;


    private void OnEnable()
    {
        equippedClothing.OnEquippedClothingChanged += UpdateVisuals;
    }

    private void OnDisable()
    {
        equippedClothing.OnEquippedClothingChanged -= UpdateVisuals;
    }

    private void Awake()
    {
        equippedClothing = new EquippedClothing();
        equippedClothingVisuals = GetComponent<EquippedClothingVisuals>();
        readyClothing = new List<ClothingItem>();
    }

    private void Start()
    {
        foreach (ClothingData item in testClothingItems)
        {
            ClothingItem clothingItem = new(item);
            equippedClothing.Equip(clothingItem);
        }
    }

    public void TakeStep()
    {
        readyClothing.Clear();

        readyClothing = equippedClothing.TakeStep();
        equippedClothingVisuals.UpdateSteps();
    }

    private void UpdateVisuals(ClothingItem _item)
    {
        equippedClothingVisuals.UpdateVisuals(_item);
        equippedClothingVisuals.UpdateSteps();
    }

    public virtual void ProcessReadyClothing((List<Character> _heroes, List<Character> _monsters) _characterLists)
    {
        foreach (ClothingItem item in readyClothing)
        {
            item.Data.Activate(this, _characterLists);
            item.ResetSteps();
            equippedClothingVisuals.UpdateSteps();
        }
    }

    public void TakeDamage(int _damage) => characterData.UpdateHealth(-_damage);
}

[Serializable]
public class CharacterData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Health { get; private set; }

    public CharacterData(string _name, int _health)
    {
        Name = _name;
        Health = _health;
    }

    public void UpdateHealth(int _amount)
    {
        Health += _amount;
    }
}