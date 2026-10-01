using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelDisplay;
    [SerializeField] private TMP_Text hpDisplay;
    [SerializeField] private TMP_Text spDisplay;
    [SerializeField] private Image portrait;

    public void InitializeDisplay(CombatParticipantData combatParticipantData)
    {
        levelDisplay.text = "Level: " + combatParticipantData.level.ToString();
        hpDisplay.text = "HP: " + combatParticipantData.currentHP.ToString() + "/" + combatParticipantData.maxHP.ToString();
        spDisplay.text = "SP: " + combatParticipantData.currentSP.ToString() + "/" + combatParticipantData.maxSP.ToString();
        portrait.sprite = CharacterManager.Instance.GetCharacterData(combatParticipantData.characterID).characterPortrait;
    }
}
