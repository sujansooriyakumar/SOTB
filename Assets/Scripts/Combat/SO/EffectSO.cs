using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public abstract void ExecuteEffect(CombatParticipantData target);
}
