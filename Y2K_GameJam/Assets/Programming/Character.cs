using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private List<ClothingItem> testClothingItems = new();

    [SerializeField] private string characterName;
    private Equipment equipment = new();

    public string Name => characterName;

    void Awake()
    {
        equipment.OnEquipmentChanged += OnEquipmentChanged;
    }

    void Start()
    {
        foreach (ClothingItem item in testClothingItems) equipment.Equip(item);
        equipment.DisplayEquipment();
    }

    private void OnEquipmentChanged(ClothingItem _oldItem, ClothingItem _newItem)
    {
        
    }
}