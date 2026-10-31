using System;
using UnityEngine;

public class AttackCombatAction : CombatAction
{
    public override event Action OnActionComplete;
    private CombatParticipantData attacker;
    private CombatParticipantData target;

    public AttackCombatAction(CombatParticipantData attacker_, CombatParticipantData target_)
    {
        this.attacker = attacker_;
        this.target = target_;
    }

    public AttackCombatAction(CombatParticipantData attacker_)
    {
        this.attacker = attacker_;
    }
    public override void ExecuteAction()
    {
        AttackData attack = new AttackData(attacker.equipmentData.GetEquipement(EquipmentSlot.WEAPON) as WeaponItemSO);
        float damage = DamageCalculator.CalculateDamage(attacker, target, attack);
        target.ModHP(-damage);
        OnActionComplete?.Invoke();

    }
}
