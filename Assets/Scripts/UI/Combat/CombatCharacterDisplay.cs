using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatCharacterDisplay : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] TMP_Text characterName;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image spBar;

    private float currentHP;
    private float currentSP;
    private CombatParticipantData characterRef;



    public void InitUI(CombatParticipantData character)
    {
        characterName.text = character.characterName;
        portrait.sprite = CharacterManager.Instance.GetSprite(character.characterID);
        hpBar.fillAmount = (float)character.currentHP / (float)character.maxHP;
        spBar.fillAmount = (float)character.currentSP/(float)character.maxSP;
        currentHP = character.currentHP; 
        currentSP = character.currentSP;
        characterRef = character;
        character.OnCharacterUpdated += InitUpdate;

    }

    private void OnDestroy()
    {
        characterRef.OnCharacterUpdated -= InitUpdate;
    }

    public void InitUpdate(CombatParticipantData character)
    {
       StartCoroutine(UpdateUI(character)); 
    }

    private IEnumerator UpdateUI(CombatParticipantData character)
    {
        float targetHP = (float)character.currentHP / character.maxHP;
        float targetSP = (float)character.currentSP / character.maxSP;

        float startHP = hpBar.fillAmount;
        float startSP = spBar.fillAmount;

        float duration = 1.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            hpBar.fillAmount = Mathf.Lerp(startHP, targetHP, t);
            spBar.fillAmount = Mathf.Lerp(startSP, targetSP, t);
            yield return null;
        }

        hpBar.fillAmount = targetHP;
        spBar.fillAmount = targetSP;
    }


}
