using UnityEngine;

[CreateAssetMenu(fileName = "IntRequirementSO", menuName = "Quest System/Requirements/IntRequirementSO")]
public class IntRequirementSO : RequirementSO
{
    public int defaultValue;
    public int targetValue;
    public override RequirementData CreateRuntimeData() => new IntRequirementData(this);
}
