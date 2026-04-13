using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private List<ClothingData> testClothingItems = new();

    [SerializeField] private string characterName;
    private Equipment equipment = new();

    public string Name => characterName;

    void Awake()
    {
        StepController.OnStepTaken += () => equipment.TakeStep();
    }

    void Start()
    {
        foreach (ClothingData item in testClothingItems)
        {
            ClothingItem clothingItem = new(item);
            equipment.Equip(clothingItem);
        }
        
        equipment.DisplayEquipment();
    }

}