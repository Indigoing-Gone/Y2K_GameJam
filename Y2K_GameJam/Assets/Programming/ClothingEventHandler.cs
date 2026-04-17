using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class ClothingEvent
{
    [field: SerializeField] public ClothingItem Item { get; private set; }
    [field: SerializeField] public Unit Owner { get; private set; }

    public ClothingEvent(Unit _owner, ClothingItem _item)
    {
        Owner = _owner;
        Item = _item;
    }
}

public struct ClothingEventPriority : IComparer<ClothingEvent>
{
    public int Compare(ClothingEvent x, ClothingEvent y)
    {
        //Compare by Clothing Priority
        int result = x.Item.Data.Priority.CompareTo(y.Item.Data.Priority);
        if(result != 0) return result;

        //Compare by Owner Index in battle order
        result = x.Owner.Data.OrderIndex.CompareTo(y.Owner.Data.OrderIndex);
        if(result != 0) return result;

        //If same Index, compare by Clothing Slot (to ensure consistent ordering)
        return x.Item.Data.Slot.CompareTo(y.Item.Data.Slot);
    }
}

public class ClothingEventHandler
{
    private PriorityQueue<ClothingEvent, ClothingEvent> clothingEventQueue;
    private float betweenClothingEventInterval;

    public ClothingEventHandler(float _interval)
    {
        clothingEventQueue = new PriorityQueue<ClothingEvent, ClothingEvent>(new ClothingEventPriority());
        betweenClothingEventInterval = _interval;
    }

    public void Setup()
    {
        ModifyStepsEffect.OnModifiedClothingItemReady += EnqueueClothingEvent;
    }

    public void Cleanup()
    {
        ModifyStepsEffect.OnModifiedClothingItemReady -= EnqueueClothingEvent;
    }

    public void EnqueueClothingEvent(Unit _unit, ClothingItem _item)
    {
        ClothingEvent clothingEvent = new(_unit, _item);
        clothingEventQueue.Enqueue(clothingEvent, clothingEvent);
    }

    public IEnumerator ProcessClothingEvents(EncounterContext _context)
    {
        while (clothingEventQueue.Count > 0)
        {
            yield return new WaitForSeconds(betweenClothingEventInterval);
            ClothingEvent _clothingEvent = clothingEventQueue.Dequeue();

            Unit _unit = _clothingEvent.Owner;
            ClothingItem _item = _clothingEvent.Item;

            if (_unit.Data.IsDead || !_item.IsReady) continue;

            yield return _unit.StartCoroutine(_item.Data.Activate(_unit, _context));
            _item.ResetSteps();
        }

        clothingEventQueue.Clear();
    }
}
