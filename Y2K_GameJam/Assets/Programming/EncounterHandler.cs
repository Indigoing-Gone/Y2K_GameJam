using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EncounterState
{
    Setup,
    InProgress,
    Finished
}

[Serializable]
public class EncounterContext
{
    [field: SerializeField] public List<Unit> Heroes { get; private set; }
    [field: SerializeField] public List<Unit> Monsters { get; private set; }

    public EncounterContext(List<Unit> _heroes, List<Unit> _monsters)
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

public class EncounterHandler : MonoBehaviour
{
    static public Action<EncounterState> OnEncounterStateChanged;

    private EncounterContext encounterContext;
    private ClothingEventHandler clothingEventHandler;

    private List<Unit> battleUnits;
    private EncounterState encounterState;
    public EncounterState EncounterState
    {
        get => encounterState;
        private set
        {
            encounterState = value;
            OnEncounterStateChanged?.Invoke(encounterState);
        }
    }

    [Header("Step Timing")]
    [SerializeField] private float betweenStepInterval;
    [SerializeField] private float betweenClothingEventInterval;

    [Header("Monster Spawning")]
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private float monsterSpawnSpacing;

    void OnEnable()
    {
        clothingEventHandler.Setup();
    }

    void OnDisable()
    {
        clothingEventHandler.Cleanup();
    }

    private void Awake()
    {
        battleUnits = new List<Unit>();
        clothingEventHandler = new ClothingEventHandler(betweenClothingEventInterval);
        EncounterState = EncounterState.Finished;
    }

    public void SetupEncounter(List<Hero> _heroes, MonsterEncounter _encounter)
    {
        if(EncounterState != EncounterState.Finished) return;

        foreach (Hero _hero in _heroes) _hero.ResetEquipment();
        List<Unit> _reversedHeroes = new(_heroes);
        _reversedHeroes.Reverse();

        List<Unit> _spawnedMonsters = SpawnMonsters(_encounter.Monsters);

        encounterContext = new EncounterContext(_reversedHeroes, _spawnedMonsters);
    
        battleUnits.Clear();
        battleUnits.AddRange(_heroes);
        battleUnits.AddRange(_spawnedMonsters);
        for (int i = 0; i < battleUnits.Count; i++)
        {
            battleUnits[i].Data.OnDeath += RemoveUnitFromBattle;
            battleUnits[i].Data.UpdateOrderIndex(i);
        }

        EncounterState = EncounterState.Setup;
    }

    private List<Unit> SpawnMonsters(List<Monster> _monsters)
    {
        List<Unit> _spawnedMonsters = new();
        float _totalWidth = monsterSpawnSpacing * (_monsters.Count - 1);

        //Instantiate monsters and calculate total width for centering
        foreach (Monster _monster in _monsters)
        {
            if (_monster == null) continue;
            Monster _spawnedMonster = Instantiate(_monster);
            _spawnedMonsters.Add(_spawnedMonster);
            _totalWidth += _spawnedMonster.Width;
        }

        //Position monsters centered around spawn point
        float _startX = monsterSpawnPoint.position.x - _totalWidth / 2;
        float _currentX = _startX;

        foreach (Monster _monster in _spawnedMonsters.Cast<Monster>())
        {
            if (_monster == null) continue;
            _monster.transform.position = new Vector3(_currentX + _monster.Width / 2, monsterSpawnPoint.position.y, monsterSpawnPoint.position.z);
            _currentX += _monster.Width + monsterSpawnSpacing;
        }

        return _spawnedMonsters;
    }

    private void RemoveUnitFromBattle(UnitData data)
    {
        Unit unitToRemove = battleUnits.Find(u => u.Data == data);
        if (unitToRemove != null)
        {
            battleUnits.Remove(unitToRemove);
            encounterContext.RemoveUnit(unitToRemove);
            unitToRemove.Data.OnDeath -= RemoveUnitFromBattle;
        }
    }

    public void StartEncounter(Action<bool> _onEncounterSuccess)
    {
        if(EncounterState != EncounterState.Setup) return;

        EncounterState = EncounterState.InProgress;
        StartCoroutine(StepEncounter(_onEncounterSuccess));
    }

    private IEnumerator StepEncounter(Action<bool> _onEncounterSuccess)
    {
        yield return new WaitForSeconds(betweenStepInterval);

        StepAllUnits();

        //Process clothing events
        yield return StartCoroutine(clothingEventHandler.ProcessClothingEvents(encounterContext));

        CheckEndConditions(_onEncounterSuccess);
    }

    private void StepAllUnits()
    {
        //Get ready clothing items from each unit and enqueue corresponding clothing events
        foreach (Unit _unit in battleUnits)
        {
            List<ClothingItem> readyClothingItems = _unit.StepEquipment();
            _unit.Data.StatusUpdate();
            foreach (ClothingItem _item in readyClothingItems) clothingEventHandler.EnqueueClothingEvent(_unit, _item);
        }
    }

    private void CheckEndConditions(Action<bool> _onEncounterSuccess)
    {
        if (encounterContext.Heroes.Count == 0 || encounterContext.Monsters.Count == 0)
        {
            EncounterState = EncounterState.Finished;
            _onEncounterSuccess?.Invoke(encounterContext.Monsters.Count == 0);
        }
        else StartCoroutine(StepEncounter(_onEncounterSuccess));
    }
}