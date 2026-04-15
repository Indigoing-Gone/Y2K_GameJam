using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    [SerializeField] private List<WardrobeSlot> wardrobeSlots;

    protected override void Awake()
    {
        base.Awake();

        foreach (WardrobeSlot _slot in wardrobeSlots) _slot.Init(this);
    }
}