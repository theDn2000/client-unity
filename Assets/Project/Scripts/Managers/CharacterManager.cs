using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using SpacetimeDB.Types;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // This class is responsible for managing character-related tasks
    public static CharacterManager Instance; // Persistent between scenes
    public List<Character> MyCharacters { get; private set; } = new List<Character>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Handler to load characters when the subscription is applied
    public async Task GetMyCharacters()
    {
        // Obtain data from the characters table
        var table = SpacetimeManager.Instance.Conn.Db.Character;
        foreach (var row in table.Iter())
        {
            MyCharacters.Add(row);
        }

        if (MyCharacters.Count == 0)
        {
            Debug.Log("No characters found on this account");
            return;
        }
        else
        {
            // Log the first character's name for debugging purposes
            Debug.Log($"Total characters loaded: {MyCharacters.Count}, the first one's name is: {MyCharacters[0].Name}");   
        }
    }
}
