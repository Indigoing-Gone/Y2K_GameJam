using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EncounterHandler))]
public class GameManager : MonoBehaviour
{
    private EncounterHandler encounterHandler;
    private ShopHandler shopHandler;

    [SerializeField] private List<Hero> heroes;
    [SerializeField] private List<EncounterPool> encountersPools;
    [SerializeField] private List<MonsterEncounter> monsterEncounters;
    private int pool, encountersGrabbed;

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

        pool = 0;
        encountersGrabbed = 0;
        CopyEncounterPool();
    }

    private void Start()
    {
        shopHandler.SetupShop();
    }

    private void ChooseEncounter()
    {
        MonsterEncounter _encounter = monsterEncounters[Random.Range(0, monsterEncounters.Count)];
        monsterEncounters.Remove(_encounter);
        encounterHandler.SetupEncounter(heroes, _encounter);

        encountersGrabbed++;
        if (encountersGrabbed == encountersPools[pool].MaxEncounters)
        {
            encountersGrabbed = 0;
            pool++;
            if (pool != encountersPools.Count)
                CopyEncounterPool();
        }
    }

    private void CopyEncounterPool()
    {
        monsterEncounters = new List<MonsterEncounter>(encountersPools[pool].Encounters);
    }

    public void StartEncounter()
    {
        encounterHandler.StartEncounter(EncounterEnded);
    }

    private void EncounterEnded(bool _success)
    {
        Debug.Log($"Encounter ended with success: {_success}");

        if (!_success)
        {
            Invoke(nameof(GameFail), 2.0f);
            return;
        }

        if (_success && pool == encountersPools.Count)
        {
            Invoke(nameof(GameWin), 2.0f);
            return;
        }

        foreach (Hero hero in heroes) hero.Data.Reset();

        shopHandler.SetupShop();
    }

    private void GameFail() => SceneManager.LoadScene("EndLose");
    
    private void GameWin() => SceneManager.LoadScene("EndWin");
}