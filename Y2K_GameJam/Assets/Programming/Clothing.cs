using UnityEngine;
using UnityEngine.UI;

public enum ClothingType
{
    Default,
    Head,
    Torso,
    Legs,
    Feet
}

[CreateAssetMenu(fileName = "ClothingItem", menuName = "Scriptable Objects/Clothing Item")]
public class ClothingItem : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string description;

    [SerializeField] private ClothingType type;
    [SerializeField] private int steps;
    [SerializeField] private bool isDebuffed;

    [SerializeField] private Image image;



    public string Name => itemName;
    public string Description => description;

    public ClothingType Type => type;
    public int Steps => steps;
    public bool IsDebuffed => isDebuffed;

    public Image Icon => image;
}
