using System;
using System.Collections;
using System.Collections.Generic;
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
        //Compare by Owner Index in battle order
        int result = x.Owner.Data.OrderIndex.CompareTo(y.Owner.Data.OrderIndex);
        if(result != 0) return result;

        //If same Index, compare by Clothing Slot (to ensure consistent ordering)
        return x.Item.Data.Slot.CompareTo(y.Item.Data.Slot);
    }
}

[Serializable]
public class BattleContext
{
    [field: SerializeField] public List<Unit> Heroes { get; private set; }
    [field: SerializeField] public List<Unit> Monsters { get; private set; }

    public List<Unit> GetAllies(Unit _unit) => Heroes.Contains(_unit) ? Heroes : Monsters;
    public List<Unit> GetEnemies(Unit _unit) => Heroes.Contains(_unit) ? Monsters : Heroes;

    public List<Unit> GetFront(List<Unit> _units) => new() { _units[0] };
    public List<Unit> GetBack(List<Unit> _units) => new() { _units[^1] };
}

public class BattleManager : MonoBehaviour
{
    [SerializeField] private float stepInterval;

    [SerializeField] private BattleContext battleContext;
    private List<Unit> units;

    private PriorityQueue<ClothingEvent, ClothingEvent> clothingEventQueue;

    void OnEnable()
    {
        ModifyStepsEffect.OnModifiedClothingItemReady += EnqueueClothingEvent;
    }

    void OnDisable()
    {
        ModifyStepsEffect.OnModifiedClothingItemReady -= EnqueueClothingEvent;
    }

    private void Start()
    {
        StartBattle();
    }

    private void StartBattle()
    {
        units = new List<Unit>();
        List<Unit> reversedHeroes = new(battleContext.Heroes);
        reversedHeroes.Reverse();
        units.AddRange(reversedHeroes);
        units.AddRange(battleContext.Monsters);
        UpdateUnitOrder();

        clothingEventQueue = new PriorityQueue<ClothingEvent, ClothingEvent>(new ClothingEventPriority());

        StartCoroutine(Step(units));
    }

    private void UpdateUnitOrder()
    {
        for (int i = 0; i < units.Count; i++) units[i].Data.UpdateOrderIndex(i);
    }

    private void EnqueueClothingEvent(Unit _unit, ClothingItem _item)
    {
        ClothingEvent clothingEvent = new(_unit, _item);
        clothingEventQueue.Enqueue(clothingEvent, clothingEvent);
    }

    private IEnumerator Step(List<Unit> _units)
    {
        yield return new WaitForSeconds(2f);

        //Get ready clothing items from each unit and enqueue corresponding clothing events
        foreach (Unit _unit in _units)
        {
            List<ClothingItem> readyClothingItems = _unit.StepEquipment();
            foreach (ClothingItem _item in readyClothingItems) EnqueueClothingEvent(_unit, _item);
        }

        //Process clothing events
        while (clothingEventQueue.Count > 0)
        {
            yield return new WaitForSeconds(1f);
            ClothingEvent _clothingEvent = clothingEventQueue.Dequeue();
            Unit _unit = _clothingEvent.Owner;
            ClothingItem _item = _clothingEvent.Item;

            if (_unit.Data.IsDead || !_item.IsReady) continue;

            _item.Data.Activate(_unit, battleContext);
            _item.ResetSteps();
        }

        StartCoroutine(Step(_units));
    }
}