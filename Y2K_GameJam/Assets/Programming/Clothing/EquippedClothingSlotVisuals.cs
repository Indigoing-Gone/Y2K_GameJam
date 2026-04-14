using UnityEngine;

public class EquippedClothingSlotVisuals : MonoBehaviour
{
    [SerializeField] private ClothingSlot clothingSlot;
    [SerializeField] private SpriteRenderer clothingRenderer;

    void Awake()
    {
        clothingRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateVisual(ClothingItem _item)
    {
        if (_item.Data.Slot != clothingSlot) return;
        if (_item == null)
        {
            clothingRenderer.sprite = null;
            return;
        }

        clothingRenderer.sprite = _item.Data.Sprite;
    }
}
