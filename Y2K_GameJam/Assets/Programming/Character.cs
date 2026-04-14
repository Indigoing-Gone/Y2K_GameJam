using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private List<ClothingData> testClothingItems;

    [SerializeField] private string characterName;
    protected EquippedClothing equippedClothing;

    public string Name => characterName;

    protected virtual void OnEnable()
    {
        StepController.OnStepTaken += TakeStep;
    }

    protected virtual void OnDisable()
    {
        StepController.OnStepTaken -= TakeStep;
    }

    protected virtual void Awake()
    {
        equippedClothing = new EquippedClothing();
    }

    protected virtual void Start()
    {
        foreach (ClothingData item in testClothingItems)
        {
            ClothingItem clothingItem = new(item);
            equippedClothing.Equip(clothingItem);
        }
    }

    protected virtual void TakeStep()
    {
        equippedClothing.TakeStep();
    }

}