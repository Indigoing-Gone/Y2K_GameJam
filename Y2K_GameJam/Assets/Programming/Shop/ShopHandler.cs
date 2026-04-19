using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    public event Action OnShoppingEnded;

    [SerializeField] private Wardrobe wardrobe;

    [Header("Shop Slots")]
    [SerializeField] private RectTransform shopPanel;

    private List<ShopSlot> shopSlots;
    [SerializeField] private ShopSlot shopSlotPrefab;
    [SerializeField] int ShopSlotCount;
    [SerializeField] private int allowedSelections;
    private int currentSelections;
    
    [Header("Shop Settings")]
    [SerializeField] private List<ClothingData> availableClothingItems;

    void OnEnable()
    {
        foreach (ShopSlot _slot in shopSlots) _slot.OnShopSlotSelected += HandleShopSelection;
    }

    void OnDisable()
    {
        foreach (ShopSlot _slot in shopSlots) _slot.OnShopSlotSelected -= HandleShopSelection;
    }

    void Awake()
    {
        shopSlots = new List<ShopSlot>();
        for (int i = 0; i < ShopSlotCount; i++)
        {
            ShopSlot _slotObj = Instantiate(shopSlotPrefab, shopPanel);
            shopSlots.Add(_slotObj);
        }

        shopPanel.gameObject.SetActive(false);
    }

    public void SetupShop()
    {
        shopPanel.gameObject.SetActive(true);
        //foreach (ShopSlot _slot in shopSlots) _slot.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(shopPanel);

        foreach (ShopSlot _slot in shopSlots)
        {
            if (availableClothingItems.Count == 0)
            {
                _slot.UpdateData(null);
                continue;
            }
            
            int _randomIndex = UnityEngine.Random.Range(0, availableClothingItems.Count);
            ClothingData _data = availableClothingItems[_randomIndex];
            availableClothingItems.RemoveAt(_randomIndex);

            _slot.UpdateData(_data);
        }
        
        currentSelections = allowedSelections;
    }

    private void HandleShopSelection(ClothingData data)
    {
        if (currentSelections <= 0) return;

        currentSelections--;
        wardrobe.AddClothingItem(new ClothingItem(data));

        if (currentSelections <= 0)
        {
            shopPanel.gameObject.SetActive(false);
            OnShoppingEnded?.Invoke();
        }
    }
}
