using System;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

public enum ClothingSlot
{
    None,
    Head,
    Torso,
    Legs,
    Feet,
    All
}

public enum ClothingPriority
{
    First,
    Default,
    Last
}

[CreateAssetMenu(fileName = "ClothingData", menuName = "Scriptable Objects/Clothing Data")]
public class ClothingData : ScriptableObject
{
    [Header("Clothing Info")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField, TextArea(3, 10)] public string Description { get; private set; }
    [field: SerializeField] public ClothingSlot Slot { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }

    [Header("Clothing Stats")]
    [field: SerializeField] public ClothingPriority Priority { get; private set; } = ClothingPriority.Default;
    [field: SerializeField] public int Steps { get; private set; }
    [SerializeReference, SR] private List<ClothingEffect> Effects;

    public void Activate(Unit _originUnit, EncounterContext _context)
    {
        foreach (ClothingEffect effect in Effects) effect.ActivateEffect(_originUnit, _context);
    }
}

public class ClothingItem
{
    public event Action OnStepsUpdated;

    private int currentSteps;

    public ClothingData Data { get; private set; }
    public int CurrentSteps
    {
        get => currentSteps; 
        private set
        {
            currentSteps = value;
            OnStepsUpdated?.Invoke();
        }
    }

    public bool IsEquipped { get; private set; }
    public bool IsReady => CurrentSteps <= 0;

    public ClothingItem(ClothingData _data)
    {
        Data = _data;
        CurrentSteps = Data.Steps;
        IsEquipped = false;
    }

    public void ModifySteps(int _amount)
    {
        CurrentSteps += _amount;
        if (CurrentSteps < 0) CurrentSteps = 0;
    }

    public void ResetSteps() => CurrentSteps = Data.Steps;
    public void SetEquipped(bool _equipped) => IsEquipped = _equipped;
}