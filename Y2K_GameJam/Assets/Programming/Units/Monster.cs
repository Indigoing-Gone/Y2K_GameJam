using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [SerializeField] private List<ClothingData> clothingData;
    [field: SerializeField] public float Width { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Width = GetComponent<BoxCollider2D>().size.x;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    protected override void Start()
    {
        base.Start();
        foreach (ClothingData _data in clothingData)
        {
            ClothingItem _item = new(_data);
            Equip(_item);
        }
    }

    override protected void HandleDeath(UnitData data)
    {
        Destroy(gameObject);
    }
}