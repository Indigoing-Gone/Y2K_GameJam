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
    [SerializeField] private string itemName;
    [SerializeField] private string description;

    [SerializeField] private ClothingSlot slot;
    [SerializeField] private int steps;

    [SerializeField] private Image image;

    public string Name => itemName;
    public string Description => description;

    public ClothingSlot Slot => slot;
    public int Steps => steps;

    public Image Icon => image;

    public void Activate()
    {
        Debug.Log($"Activating {itemName}");
    }
}

public class ClothingItem
{
    private ClothingData data;
    private int currentSteps;

    public int CurrentSteps => currentSteps;
    public ClothingData Data => data;

    public ClothingItem(ClothingData _data)
    {
        data = _data;
        currentSteps = data.Steps;
    }

    public void TakeStep()
    {
        currentSteps--;
        if(currentSteps > 0) return;
        data.Activate();
        currentSteps = data.Steps;
    }
}