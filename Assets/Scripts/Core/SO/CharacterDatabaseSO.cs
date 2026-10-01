using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabaseSO", menuName = "SOTB/CharacterDatabaseSO")]
public class CharacterDatabaseSO : ScriptableObject
{
    public List<CharacterSO> characterList;

    public Dictionary<int, CharacterSO> InitializeDictionary()
    {
        Dictionary<int, CharacterSO> characterDictionary = new Dictionary<int, CharacterSO>();

        foreach (CharacterSO character in characterList)
        {
            characterDictionary.Add(character.characterID, character); 
        }

        return characterDictionary;
    }
}
