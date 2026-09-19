using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "Character", menuName = "Sins of the Brother/Character")]
public class Character : SerializedScriptableObject // Must extend this for Odin serialization!
{
    public string characterName;
    public int characterID;

    [DictionaryDrawerSettings(KeyLabel = "Emotion", ValueLabel = "Sprite", DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
    [OdinSerialize]
    public Dictionary<EmotionStates, Sprite> emotionSprites = new();
}

public enum EmotionStates
{
    NEUTRAL,
    HAPPY,
    ANGRY,
    SAD,
    SHOCKED
}
