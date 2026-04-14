using System;
using System.Collections.Generic;
using UnityEngine;

public enum ClothingSlot
{
    Default,
    Head,
    Torso,
    Legs,
    Feet
}

[CreateAssetMenu(fileName = "ClothingItem", menuName = "Scriptable Objects/Clothing Item")]
public class ClothingData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField] public ClothingSlot Slot { get; private set; }
    [field: SerializeField] public int Steps { get; private set; }

    [SerializeField] private List<ClothingEffect> Effects;

    [field: SerializeField] public Sprite Sprite { get; private set; }

    public void Activate(Character _originCharacter, (List<Character> _allies, List<Character> _enemies) _characterLists)
    {
        foreach (IClothingEffect effect in Effects) effect.ActivateEffect(_originCharacter, _characterLists);
    }
}

public class ClothingItem
{
    public ClothingData Data { get; private set; }
    public int CurrentSteps { get; private set; }

    public ClothingItem(ClothingData _data)
    {
        Data = _data;
        CurrentSteps = Data.Steps;
    }

    public bool HandleStep()
    {
        if (CurrentSteps > 0) CurrentSteps--;
        if (CurrentSteps > 0) return false;
        return true;
    }

    public void ResetSteps()
    {
        CurrentSteps = Data.Steps;
    }
}