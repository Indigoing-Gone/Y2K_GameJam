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

    public BattleContext(List<Unit> _heroes, List<Unit> _monsters)
    {
        Heroes = _heroes;
        Monsters = _monsters;
    }

    public List<Unit> GetAllies(Unit _unit) => Heroes.Contains(_unit) ? Heroes : Monsters;
    public List<Unit> GetEnemies(Unit _unit) => Heroes.Contains(_unit) ? Monsters : Heroes;

    public List<Unit> GetFront(List<Unit> _units) => (_units.Count == 0) ? new() : new() { _units[0] };
    public List<Unit> GetBack(List<Unit> _units) => (_units.Count == 0) ? new() : new() { _units[^1] };

    public void RemoveUnit(Unit _unit)
    {
        if (Heroes.Contains(_unit)) Heroes.Remove(_unit);
        else if (Monsters.Contains(_unit)) Monsters.Remove(_unit);
    }
}

public class BattleManager : MonoBehaviour
{
    [SerializeField] private List<Hero> heroes;
    [SerializeField] private MonsterEncounter testEncounter;
    private BattleContext battleContext;
    private List<Unit> battleUnits;
    private PriorityQueue<ClothingEvent, ClothingEvent> clothingEventQueue;

    [SerializeField] private float stepInterval;
    private bool inBattle;

    void OnEnable()
    {
        ModifyStepsEffect.OnModifiedClothingItemReady += EnqueueClothingEvent;
    }

    void OnDisable()
    {
        ModifyStepsEffect.OnModifiedClothingItemReady -= EnqueueClothingEvent;
    }

    void Awake()
    {
        battleUnits = new List<Unit>();
        clothingEventQueue = new PriorityQueue<ClothingEvent, ClothingEvent>(new ClothingEventPriority());
    }

    void Start()
    {
        SetupBattle(testEncounter);
    }

    private void EnqueueClothingEvent(Unit _unit, ClothingItem _item)
    {
        ClothingEvent clothingEvent = new(_unit, _item);
        clothingEventQueue.Enqueue(clothingEvent, clothingEvent);
    }

    public void SetupBattle(MonsterEncounter _encounter)
    {
        List<Unit> _spawnedMonsters = new();
        foreach (Monster _monster in _encounter.Monsters)
        {
            Monster spawnedMonster = Instantiate(_monster);
            _spawnedMonsters.Add(spawnedMonster);
        }

        foreach (Hero _hero in heroes) _hero.ResetEquipment();
        List<Unit> _reversedHeroes = new(heroes);
        _reversedHeroes.Reverse();

        battleContext = new BattleContext(_reversedHeroes, _spawnedMonsters);

        battleUnits.Clear();
        battleUnits.AddRange(heroes);
        battleUnits.AddRange(_spawnedMonsters);
        for (int i = 0; i < battleUnits.Count; i++)
        {
            battleUnits[i].Data.OnDeath += RemoveUnitFromBattle;
            battleUnits[i].Data.UpdateOrderIndex(i);
        }
    }

    private void RemoveUnitFromBattle(UnitData data)
    {
        Unit unitToRemove = battleUnits.Find(u => u.Data == data);
        if (unitToRemove != null)
        {
            battleUnits.Remove(unitToRemove);
            battleContext.RemoveUnit(unitToRemove);
            unitToRemove.Data.OnDeath -= RemoveUnitFromBattle;
        }
    }

    public void StartBattle()
    {
        if(inBattle) return;

        clothingEventQueue.Clear();

        inBattle = true;
        StartCoroutine(Step(battleUnits));
    }

    private void EndBattle(bool _battleWon)
    {
        inBattle = false;
        StopAllCoroutines();
        SetupBattle(testEncounter);
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

        clothingEventQueue.Clear();

        if (battleContext.Heroes.Count == 0) EndBattle(false);
        else if (battleContext.Monsters.Count == 0) EndBattle(true);
        else StartCoroutine(Step(_units));
    }
}