using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SkillEffect
{
    public abstract void ExecuteEffect(CombatParticipantData user, List<CombatParticipantData> targets);
}
