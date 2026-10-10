using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterDisplayButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Image portrait;
    [SerializeField] private GameObject selected;
    public CombatParticipantData characterData;
    public event Action<CombatParticipantData> OnSelected;
    public void InitializeButton(CombatParticipantData character)
    {
        characterData = character;
        characterName.text = character.characterName;
        portrait.sprite = CharacterManager.Instance.GetCharacterData(character.characterID).characterPortrait;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(characterData.characterName);
        selected.SetActive(true);
        OnSelected?.Invoke(characterData);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected.SetActive(false);
    }

    private void OnDisable()
    {
        selected.SetActive(false);
    }
}
