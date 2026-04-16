using TMPro;
using UnityEngine;

public class EquipmentSlotVisuals : MonoBehaviour
{
    [Header("Clothing Info")]
    [field: SerializeField] public ClothingSlot ClothingSlot { get; private set; }
    private ClothingItem clothingItem;

    //Visuals
    private SpriteRenderer clothingRenderer;
    private TextMeshPro stepsText;

    void Awake()
    {
        clothingItem = null;
        clothingRenderer = GetComponent<SpriteRenderer>();
        stepsText = GetComponentInChildren<TextMeshPro>();
        UpdateVisual(null);
    }

    public void UpdateVisual(ClothingItem _item)
    {
        if (_item == null)
        {
            if (clothingItem != null) clothingItem.OnStepsUpdated -= UpdateSteps;

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

    private void UpdateSteps()
    {
        if (clothingItem == null) return;
        stepsText.text = clothingItem.CurrentSteps.ToString();
    }
}
