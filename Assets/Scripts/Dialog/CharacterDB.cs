using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "CharacterDB", menuName = "Sins of the Brother/CharacterDB")]
public class CharacterDB : SerializedScriptableObject
{
    [LabelText("All Characters (Drag Character assets here)")]
    public List<Character> characterList;

    [LabelText("Character Lookup Dictionary")]
    [DictionaryDrawerSettings(KeyLabel = "Character ID", ValueLabel = "CharacterData", DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
    [OdinSerialize]
    public Dictionary<int, CharacterData> characterDictionary = new();

    [Button("Initialize Dictionary From List")]
    public void InitializeCharacters()
    {
        characterDictionary = new Dictionary<int, CharacterData>();
        foreach (Character c in characterList)
        {
            if (!characterDictionary.ContainsKey(c.characterID))
            {
                characterDictionary.Add(c.characterID, new CharacterData(c));
            }
            else
            {
                Debug.LogWarning($"Duplicate ID {c.characterID} in {c.name}");
            }
        }
    }

    public CharacterData GetCharacterData(int id)
    {
        return characterDictionary[id];
    }
  
}

[Serializable]
public class CharacterData
{
    public string characterName;
    public int characterID;

    [DictionaryDrawerSettings(KeyLabel = "Emotion", ValueLabel = "Sprite", DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
    [OdinSerialize]
    public Dictionary<EmotionStates, Sprite> emotionSprites = new();

    public CharacterData(Character c)
    {
        characterName = c.characterName;
        characterID = c.characterID;
        emotionSprites = new Dictionary<EmotionStates, Sprite>(c.emotionSprites);
    }
}
