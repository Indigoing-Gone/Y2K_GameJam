using System;
using System.Collections.Generic;
using UnityEngine;

public interface IClothingEffect
{
    void ActivateEffect(Character _originCharacter, (List<Character> _allies, List<Character> _enemies) _characterLists);
}

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Clothing Effects/Attack")]
public class AttackEffect : ScriptableObject, IClothingEffect
{
    public void ActivateEffect(Character _originCharacter, (List<Character> _allies, List<Character> _enemies) _characterLists)
    {
        if(_characterLists._enemies.Count <= 0) return;
        Debug.Log($"{_originCharacter.name} attacking {_characterLists._enemies[0].name}");
    }
}