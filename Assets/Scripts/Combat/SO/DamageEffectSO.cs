using UnityEngine;
[CreateAssetMenu(fileName = "DamageEffectSO", menuName = "SOTB/Effects/Damage")]

public class DamageEffectSO : EffectSO
{
    public DamageType damageType;
    public DamageAttribute damageAttribute;
    public float damageAmount;
    public override void ExecuteEffect(CombatParticipantData[] targets)
    {
        foreach (CombatParticipantData target in targets)
        {
            float damage = DamageCalculator.CalculateItemDamage(target, damageAmount, damageAttribute, damageType);
            target.currentHP -= Mathf.CeilToInt(damage);
        }
    }
}
