using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    // In this manager, we will handle character selection logic
    // First, we will define a singleton
    // Then, we need to get the chaaracters by consulting the public list of characters in the CharacterManager instance
    // From there, we call the UI_CharacterSelection to display the characters and allow the player to select one
    // In this script we need to handle the character creation and selection logic
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static CharacterSelectionManager instance;
    void Awake()
    {
        // Implement singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
