using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EncounterHandler))]
public class GameManager : MonoBehaviour
{
    private EncounterHandler encounterHandler;
    private ShopHandler shopHandler;

    [SerializeField] private List<Hero> heroes;
    [SerializeField] private List<MonsterEncounter> monsterEncounters;

    void OnEnable()
    {
        shopHandler.OnShoppingEnded += ChooseEncounter;
    }

    void OnDisable()
    {
        shopHandler.OnShoppingEnded -= ChooseEncounter;
    }

    private void Awake()
    {
        encounterHandler = GetComponent<EncounterHandler>();
        shopHandler = GetComponent<ShopHandler>();
    }

    private void Start()
    {
        ChooseEncounter();
    }

    private void ChooseEncounter()
    {
        MonsterEncounter _encounter = monsterEncounters[Random.Range(0, monsterEncounters.Count)];
        monsterEncounters.Remove(_encounter);
        encounterHandler.SetupEncounter(heroes, _encounter);
    }

    public void StartEncounter()
    {
        encounterHandler.StartEncounter(EncounterEnded);
    }

    private void EncounterEnded(bool _success)
    {
        Debug.Log($"Encounter ended with success: {_success}");

        foreach (Hero hero in heroes) hero.Data.Reset();

        shopHandler.SetupShop();
    }
}