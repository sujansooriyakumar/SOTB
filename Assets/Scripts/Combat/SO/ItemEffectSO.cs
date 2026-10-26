using UnityEngine;

public abstract class ItemEffectSO : ScriptableObject
{
    public abstract bool ExecuteEffect(CombatParticipantData target);
}
