using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    private List<WardrobeSlot> wardrobeSlots;

    protected override void Awake()
    {
        base.Awake();
        
        wardrobeSlots = new List<WardrobeSlot>(GetComponentsInChildren<WardrobeSlot>());

        foreach (WardrobeSlot _slot in wardrobeSlots) _slot.Init(this);
    }
}