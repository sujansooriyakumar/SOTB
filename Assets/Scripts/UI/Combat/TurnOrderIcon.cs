using UnityEngine;
using UnityEngine.UI;

public class TurnOrderIcon : MonoBehaviour
{
    public CombatParticipantData participantRef;
    public Image image;
    public RectTransform rectTransform;

    public RectTransform GetRect() => rectTransform;
    public bool updated = false;
    public CombatParticipantData GetParticipantRef () => participantRef;
    public Image GetImage () => image;

    public void SetParticipantRef(CombatParticipantData participantRef_)
    {
        participantRef = participantRef_;
        image.sprite = CharacterManager.Instance.GetSprite(participantRef.characterID);
        updated = true;
    }
}
