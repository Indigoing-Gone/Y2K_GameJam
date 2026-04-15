using TMPro;
using UnityEngine;

public class EquipmentSlotVisuals : MonoBehaviour
{
    [Header("Clothing Info")]
    [SerializeField] private ClothingSlot clothingSlot;
    [SerializeField] private ClothingItem clothingItem;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer clothingRenderer;
    [SerializeField] private TextMeshPro stepsText;

    void Awake()
    {
        clothingItem = null;
        clothingRenderer = GetComponent<SpriteRenderer>();
        stepsText = GetComponentInChildren<TextMeshPro>();
    }

    public void UpdateVisual(ClothingItem _item)
    {
        if (_item.Data.Slot != clothingSlot) return;
        if (_item == null)
        {
            clothingItem.OnStepsUpdated -= UpdateSteps;
            UpdateSteps();

            clothingItem = null;
            clothingRenderer.sprite = null;
            stepsText.text = "-";

            return;
        }
        

        clothingItem = _item;
        clothingItem.OnStepsUpdated += UpdateSteps;
        UpdateSteps();
        
        clothingRenderer.sprite = _item.Data.Sprite;
    }

    public void UpdateSteps()
    {
        if (clothingItem == null) return;
        stepsText.text = clothingItem.CurrentSteps.ToString();
    }
}
