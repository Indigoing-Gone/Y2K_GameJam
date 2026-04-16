using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [SerializeField] private List<ClothingData> clothingData;

    private void Start()
    {
        foreach (ClothingData _data in clothingData)
        {
            ClothingItem _item = new(_data);
            Equip(_item);
        }
    }
}