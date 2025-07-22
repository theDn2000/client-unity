using UnityEngine;



public enum StatModifierType
{
    Flat, // +5
    PercentAdd, // +5%
    PercentMult // *5% (multiplies the acumulated value)
}

public class StatModifier : MonoBehaviour
{
    public readonly float Value;
    public readonly StatModifierType Type;
    public readonly int Order;
    public readonly object Source; // The source of the modifier, e.g., an item or skill

    public StatModifier(float value, StatModifierType type, int order = 0, object source = null)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }
}



