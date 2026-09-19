using UnityEngine;

[CreateAssetMenu(fileName = "WeaponClassSO", menuName = "SOTB/Inventory/WeaponClassSO")]
public class WeaponClassSO : ScriptableObject
{
    public string className;
    public DamageType damageType;
    public WeaponGripType gripType;
}
