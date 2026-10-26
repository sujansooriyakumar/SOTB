using UnityEngine;

[CreateAssetMenu(fileName = "ItemResourceModEffectSO", menuName = "SOTB/Effects/ItemResourceModEffectSO")]
public class ItemResourceModEffectSO : ItemEffectSO
{
    public ResourceType resourceType;
    public int effectValue;
    public override bool ExecuteEffect(CombatParticipantData target)
    {
        string systemMessage = "";
        switch (resourceType)
        {
            case ResourceType.HP:
                if (target.currentHP >= target.maxHP) return false;
                int newHP = target.currentHP + effectValue;
                newHP = Mathf.CeilToInt(target.maxHP);
                int hpGained = newHP - target.currentHP;
                target.currentHP = newHP;
                systemMessage = target.characterName + " gained " + hpGained.ToString() + " HP!";
                DialogDisplay.Instance.DisplaySystemMessage(systemMessage);
                
                break;
            case ResourceType.SP:
                if (target.currentSP >= target.maxSP) return false;

                int newSP = target.currentSP + effectValue;
                newSP = Mathf.CeilToInt(target.maxSP);
                int spGained = newSP - target.currentSP;
                target.currentSP = newSP;
                systemMessage = target.characterName + " gained " + spGained.ToString() + " SP!";
                DialogDisplay.Instance.DisplaySystemMessage(systemMessage);

                break;
        }
        return true;

    }
}
