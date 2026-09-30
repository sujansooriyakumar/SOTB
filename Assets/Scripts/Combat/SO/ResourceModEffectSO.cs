using UnityEngine;

[CreateAssetMenu(fileName = "ResourceModEffectSO", menuName = "SOTB/Effects/ResourceModEffectSO")]
public class ResourceModEffectSO : EffectSO
{
    public ResourceType resourceType;
    public int effectValue;
    public override void ExecuteEffect(CombatParticipantData[] targets)
    {
        foreach (CombatParticipantData target in targets)
        {
            switch (resourceType)
            {
                case ResourceType.HP:
                    target.currentHP += effectValue;
                    break;
                case ResourceType.SP:
                    target.currentSP += effectValue;
                    break;
            }
        }
    }
}
