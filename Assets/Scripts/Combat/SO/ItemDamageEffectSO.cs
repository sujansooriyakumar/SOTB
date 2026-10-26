using UnityEngine;
[CreateAssetMenu(fileName = "ItemDamageEffectSO", menuName = "SOTB/Effects/Items/ItemDamageEffect")]

public class ItemDamageEffectSO : ItemEffectSO
{
    public DamageType damageType;
    public DamageAttribute damageAttribute;
    public float damageAmount;
    public override bool ExecuteEffect(CombatParticipantData target)
    {
        float damage = DamageCalculator.CalculateItemDamage(target, damageAmount, damageAttribute, damageType);
        target.currentHP -= Mathf.CeilToInt(damage);
        return true;

    }
}
