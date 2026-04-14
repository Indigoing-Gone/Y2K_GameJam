using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    override public void ProcessReadyClothing((List<Character> _heroes, List<Character> _monsters) _characterLists)
    {
        foreach (ClothingItem item in readyClothing)
        {
            item.Data.Activate(this, (_characterLists._monsters, _characterLists._heroes));
            item.ResetSteps();
            equippedClothingVisuals.UpdateSteps();
        }
    }
}
