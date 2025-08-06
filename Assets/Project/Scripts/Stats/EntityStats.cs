using UnityEngine;

public class EntityStats : MonoBehaviour
{
    // Define the 4 main stats
    public Stat Strength = new Stat(10);
    public Stat Intelligence = new Stat(10);
    public Stat Agility = new Stat(10);
    public Stat Vitality = new Stat(10);

    // Define secondary stats
    public Stat armor = new Stat();
    public Stat magicResistance = new Stat();
    public Stat criticalHitChance = new Stat();
    public Stat criticalHitDamage = new Stat();
    public Stat attackSpeed = new Stat();
    public Stat movementSpeed = new Stat();
    public Stat attackRange = new Stat();
    public Stat physicalDamage = new Stat();
    public Stat magicalDamage = new Stat();



    public void RecalculateDerivedStats()
    {
        // Examples of how to recalculate derived stats based on main stats
        armor.BaseValue = Vitality.Value * 0.5f;
        magicResistance.BaseValue = Intelligence.Value * 0.5f;
        criticalHitChance.BaseValue = Agility.Value * 0.1f;
        criticalHitDamage.BaseValue = Strength.Value * 1.5f;
        attackSpeed.BaseValue = Agility.Value * 0.2f;
        movementSpeed.BaseValue = Agility.Value * 0.3f;
        physicalDamage.BaseValue = Strength.Value * 2;
        magicalDamage.BaseValue = Intelligence.Value * 2;

        // Log the recalculated stats for debugging
        Debug.Log($"Recalculated Stats: Armor={armor.Value}, Magic Resistance={magicResistance.Value}, " +
                  $"Critical Hit Chance={criticalHitChance.Value}, Critical Hit Damage={criticalHitDamage.Value}, " +
                  $"Attack Speed={attackSpeed.Value}, Movement Speed={movementSpeed.Value}, " +
                  $"Physical Damage={physicalDamage.Value}, Magical Damage={magicalDamage.Value}");
    }



    private void Start()
    {
        // Initialize derived stats
        RecalculateDerivedStats();
    }
}


