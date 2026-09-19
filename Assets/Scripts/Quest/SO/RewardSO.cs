using UnityEngine;

[CreateAssetMenu(fileName = "RewardSO", menuName = "Quest System/Rewards/RewardSO")]
public abstract class RewardSO : ScriptableObject
{
    public string rewardName;
    public abstract RewardData CreateRuntimeData();

}

