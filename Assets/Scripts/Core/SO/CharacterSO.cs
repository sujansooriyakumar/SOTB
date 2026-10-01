using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "SOTB/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public int characterID;
    public Sprite characterPortrait;
    public string characterName;
}
