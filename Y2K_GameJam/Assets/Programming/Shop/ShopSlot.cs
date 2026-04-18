using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public event Action<ClothingData> OnShopSlotSelected;

    [Header("Shop Slot Info")]
    [SerializeField] private ClothingData clothingData;
    [SerializeField] private GameObject clothingImageObject;
    private Image clothingImage;
    private AspectRatioFitter clothingRatio;

    void Awake()
    {
        clothingImage = clothingImageObject.GetComponent<Image>();
        clothingRatio = clothingImageObject.GetComponent<AspectRatioFitter>();
    }

    public void UpdateData(ClothingData _data)
    {
        clothingData = _data;

        if(_data == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if(_data.Sprite == null)
        {
            clothingImage.sprite = null;
            clothingRatio.aspectRatio = 1f;
            return;
        }
        
        clothingImage.sprite = _data.Sprite;
        clothingRatio.aspectRatio = _data.Sprite.bounds.size.x / _data.Sprite.bounds.size.y;
    }

    public void SelectShopSlot()
    {
        if (clothingData == null) return;
        OnShopSlotSelected?.Invoke(clothingData);
        UpdateData(null);
    }
}
