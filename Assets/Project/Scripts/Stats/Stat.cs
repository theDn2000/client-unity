using System;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public float BaseValue { get; set; }

    private List<StatModifier> _modifiers = new List<StatModifier>();
    private bool _isDirty = true;
    private float _finalValue;

    public Stat(float baseValue = 0f)
    {
        BaseValue = baseValue;
    }

    public float Value
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
                case StatModifierType.PercentAdd: // First sum all percent and then apply (1 + (0.1 + 0.2) = 1.3)
                    percentAdd += modifier.Value;
                    break;
                case StatModifierType.PercentMult: // Apply percent multipliers directly (1 * 1.1 * 1.2 = 1.32)
                    final *= 1 + modifier.Value;
                    break;
            }
        }
        final *= 1 + percentAdd;
        return (float)Math.Round(final, 4);
    }
}
