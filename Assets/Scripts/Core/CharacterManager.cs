using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabaseSO characterDatabase;
    private Dictionary<int, CharacterSO> characterDictionary = new Dictionary<int, CharacterSO>();
    public static CharacterManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
        characterDictionary = characterDatabase.InitializeDictionary();
    }

    public CharacterSO GetCharacterData(int characterID)
    {
        return characterDictionary[characterID];
    }

    public Sprite GetSprite(int characterID)
    {
        return characterDictionary[characterID].characterPortrait;
    }
}
