using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private float stepInterval;

    [SerializeField] private List<Character> heroes;
    [SerializeField] private List<Character> monsters;

    private List<Character> characterOrder;

    private void Start()
    {
        StartBattle();
    }

    private void StartBattle()
    {
        characterOrder = new List<Character>();
        characterOrder.AddRange(heroes);
        characterOrder.AddRange(monsters);

        StartCoroutine(Step());
    }

    private IEnumerator Step()
    {
        yield return new WaitForSeconds(2f);
        foreach (Character _character in characterOrder) _character.TakeStep();

        List<Character> _heroesReordered = new(heroes);
        _heroesReordered.Reverse();

        foreach (Character _character in characterOrder)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log($"Processing ready clothing for {_character.name}");
            _character.ProcessReadyClothing((_heroesReordered, monsters));
        }

        StartCoroutine(Step());
    }
}