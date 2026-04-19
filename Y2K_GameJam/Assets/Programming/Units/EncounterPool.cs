using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterPool", menuName = "Scriptable Objects/EncounterPool")]
public class EncounterPool : ScriptableObject
{
    [field: SerializeField] public int MaxEncounters { get; private set; }
    [field: SerializeField] public List<MonsterEncounter> Encounters { get; private set; }
}
