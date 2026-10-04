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
                target.currentHP += effectValue;
                target.currentHP = Mathf.CeilToInt(target.maxHP);
                break;
            case ResourceType.SP:
                target.currentSP += effectValue;
                target.currentSP = Mathf.CeilToInt(target.maxSP);

                break;
        }

    }
}
