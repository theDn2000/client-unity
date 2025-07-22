using UnityEngine;

// This script manages all the stats for every entity (with stats) in the game.
public class StatsManager : MonoBehaviour
{
    // Define the 4 main Stats
    [Header("Main Stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    // Define the health stats
    [Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;

    // Define the mana stats
    [Header("Mana Stats")]
    public int maxMana;
    public int currentMana;

    // Define secondary stats
    [Header("Secondary Stats")]
    public int armor;
    public int magicResistance;
    public int criticalHitChance;
    public int criticalHitDamage;
    public int attackSpeed;
    public int movementSpeed;
    public int damage;
    public int atackRange;
}
