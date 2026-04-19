using TMPro;
using UnityEngine;

[RequireComponent(typeof(Tooltip))]
public class EquipmentSlotVisuals : MonoBehaviour
{
    [Header("Clothing Info")]
    [field: SerializeField] public ClothingSlot ClothingSlot { get; private set; }
    private ClothingItem clothingItem;

    //Visuals
    private SpriteRenderer clothingRenderer;
    private TextMeshPro stepsText;
    private Tooltip tooltip;

    void Awake()
    {
        clothingItem = null;
        clothingRenderer = GetComponentInChildren<SpriteRenderer>();
        stepsText = GetComponentInChildren<TextMeshPro>();
        tooltip = GetComponent<Tooltip>();

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
            tooltip.SetTooltipText(string.Empty);

            return;
        }

        clothingItem = _item;
        clothingItem.OnStepsUpdated += UpdateSteps;
        UpdateSteps();
        
        clothingRenderer.sprite = _item.Data.Sprite;
        tooltip.SetTooltipText(_item.Data.Description);
    }

    private void UpdateSteps()
    {
        if (clothingItem == null) return;
        stepsText.text = clothingItem.CurrentSteps.ToString();
    }
}
