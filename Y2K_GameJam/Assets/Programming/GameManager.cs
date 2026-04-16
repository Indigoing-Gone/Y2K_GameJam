using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EncounterHandler))]
public class GameManager : MonoBehaviour
{
    private EncounterHandler encounterHandler;

    [SerializeField] private List<Hero> heroes;
    [SerializeField] private MonsterEncounter testEncounter;


    private void Awake()
    {
        encounterHandler = GetComponent<EncounterHandler>();
    }

    private void Start()
    {
        encounterHandler.SetupEncounter(heroes, testEncounter);
    }

    public void StartEncounter()
    {
        encounterHandler.StartEncounter(EncounterEnded);
    }

    private void EncounterEnded(bool _success)
    {
        Debug.Log($"Encounter ended with success: {_success}");
        encounterHandler.SetupEncounter(heroes, testEncounter);
    }
}