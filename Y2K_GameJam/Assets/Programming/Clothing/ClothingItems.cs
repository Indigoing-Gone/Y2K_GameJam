using UnityEngine;
using UnityEngine.UI;

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

    [field: SerializeField] public Sprite Sprite { get; private set; }

    public void Activate()
    {
        Debug.Log($"Activating {Name}");
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

    public void TakeStep()
    {
        CurrentSteps--;
        if(CurrentSteps > 0) return;
        Data.Activate();
        CurrentSteps = Data.Steps;
    }
}