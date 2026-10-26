using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillCombatAction : CombatAction
{
    public override event Action OnActionComplete;
    private List<CombatParticipantData> targets;
    private CombatParticipantData user;
    private SkillSO skill;
    public SkillCombatAction(CombatParticipantData user_, List<CombatParticipantData> targets_, SkillSO skill_)
    {
        targets = targets_;
        user = user_;
        skill = skill_;
    }

    public override void ExecuteAction()
    {
        skill.ExecuteSkill(user, targets);
        OnActionComplete?.Invoke();
    }
}
