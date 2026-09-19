using UnityEngine;

[CreateAssetMenu(fileName = "BoolRequirementSO", menuName = "Quest System/Requirements/BoolRequirementSO")]
public class BoolRequirementSO : RequirementSO
{
    public bool defaultValue;
    public override RequirementData CreateRuntimeData() => new BoolRequirementData(this);
}
