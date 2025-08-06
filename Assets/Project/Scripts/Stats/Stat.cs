using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    // --- Private fields ---
    private float _baseValue;
    private List<StatModifier> _modifiers = new List<StatModifier>();
    private bool _isDirty = true;
    private float _finalValue;

    // --- Constructor ---
    public Stat(float baseValue = 0f)
    {
        BaseValue = baseValue;
    }

    // --- Public properties ---
    public float BaseValue // Property to set the base value of the stat (myStat.BaseValue = 10)
    {
        get => _baseValue;
        set
        {
            if (_baseValue != value)
            {
                _baseValue = value;
                _isDirty = true;
            }
        }
    }

    public float Value // Property to get the final value of the stat (myStat.Value)
    {
        get
        {
            if (_isDirty)
            {
                _finalValue = CalculateFinalValue();
                _isDirty = false;
            }
            return _finalValue;
        }
    }

    // --- Public methods ---
    public void AddModifier(StatModifier modifier)
    {
        _modifiers.Add(modifier);
        _isDirty = true;
    }

    public void RemoveModifier(StatModifier modifier)
    {
        _modifiers.Remove(modifier);
        _isDirty = true;
    }

    public void ClearModifiers()
    {
        _modifiers.Clear();
        _isDirty = true;
    }

    // --- Private methods ---
    private float CalculateFinalValue()
    {
        float final = BaseValue;
        float percentAdd = 0;

        _modifiers.Sort((a, b) => a.Order.CompareTo(b.Order));

        foreach (var modifier in _modifiers)
        {
            switch (modifier.Type)
            {
                case StatModifierType.Flat:
                    final += modifier.Value;
                    break;
                case StatModifierType.PercentAdd:
                    percentAdd += modifier.Value;
                    break;
                case StatModifierType.PercentMult:
                    final *= 1 + modifier.Value;
                    break;
            }
        }
        final *= 1 + percentAdd;
        return (float)Math.Round(final, 4);
    }
}
