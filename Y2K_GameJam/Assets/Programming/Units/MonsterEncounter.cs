using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterEncounter", menuName = "Scriptable Objects/Monster Encounter")]
public class MonsterEncounter : ScriptableObject
{
    [field: SerializeField] public List<Monster> Monsters { get; private set; }
}
