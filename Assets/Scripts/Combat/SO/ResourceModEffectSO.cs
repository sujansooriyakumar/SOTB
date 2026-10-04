using UnityEngine;

[CreateAssetMenu(fileName = "ResourceModEffectSO", menuName = "SOTB/Effects/ResourceModEffectSO")]
public class ResourceModEffectSO : EffectSO
{
    public ResourceType resourceType;
    public int effectValue;
    public override void ExecuteEffect(CombatParticipantData target)
    {
        switch (resourceType)
        {
            case ResourceType.HP:
                int newHP = target.currentHP + effectValue;
                newHP = Mathf.CeilToInt(target.maxHP);
                int hpGained = newHP - target.currentHP;
                target.currentHP = newHP;
                string systemMessage = target.characterName + " gained " + hpGained.ToString() + " HP!";
                //DialogDisplay.Instance.DisplaySystemMessage(systemMessage);
                
                break;
            case ResourceType.SP:
                target.currentSP += effectValue;
                target.currentSP = Mathf.CeilToInt(target.maxSP);

                break;
        }

    }
}
