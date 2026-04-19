using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WardrobeSlot : MonoBehaviour
{
    static public event Action<int, ClothingSlot, Unit> OnShiftingWardrobe;

    [SerializeField] private ClothingSlot clothingSlot;
    private List<GameObject> children;
    private Button[] buttons;
    private Unit unit;

    void OnEnable()
    {
        EncounterHandler.OnEncounterStateChanged += ActivateWardrobeSlot;
    }

    void OnDisable()
    {
        //EncounterHandler.OnEncounterStateChanged -= ActivateWardrobeSlot;
    }

    void Awake()
    {
        children = new List<GameObject>();
        foreach (RectTransform child in gameObject.GetComponentsInChildren<RectTransform>()) children.Add(child.gameObject); 
        buttons = GetComponentsInChildren<Button>();
    }

    public void Init(Unit _unit)
    {
        unit = _unit;
    }

    public void ShiftWardrobe(int _direction)
    {
        OnShiftingWardrobe?.Invoke(_direction, clothingSlot, unit);
    }

    private void ActivateWardrobeSlot(EncounterState _state)
    {
        bool _isActive = _state != EncounterState.InProgress;
        foreach (GameObject child in children) child.SetActive(_isActive);
    }
}