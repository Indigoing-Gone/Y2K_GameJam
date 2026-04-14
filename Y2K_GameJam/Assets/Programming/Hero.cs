using UnityEngine;

[RequireComponent(typeof(EquippedClothingVisuals))]
public class Hero : Character
{
    private EquippedClothingVisuals equippedClothingVisuals;

    protected override void OnEnable()
    {
        base.OnEnable();
        equippedClothing.OnEquippedClothingChanged += UpdateVisuals; 
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        equippedClothing.OnEquippedClothingChanged -= UpdateVisuals;
    }

    protected override void Awake()
    {
        base.Awake();

        equippedClothingVisuals = GetComponent<EquippedClothingVisuals>();
    }

    private void UpdateVisuals(ClothingItem _item)
    {
        equippedClothingVisuals.UpdateVisuals(_item);
    }
}
