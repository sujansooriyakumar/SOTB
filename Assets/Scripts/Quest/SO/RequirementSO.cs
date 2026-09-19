using UnityEngine;

[CreateAssetMenu(fileName = "RequirementSO", menuName = "Scriptable Objects/RequirementSO")]
public abstract class RequirementSO : ScriptableObject
{
    public string requirementName;
    public abstract RequirementData CreateRuntimeData();
}
