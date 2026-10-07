using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public abstract bool ExecuteEffect(CombatParticipantData target);
}
