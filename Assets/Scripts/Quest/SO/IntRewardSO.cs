using UnityEngine;

[CreateAssetMenu(fileName = "IntRewardSO", menuName = "Quest System/Rewards/IntRewardSO")]
public class IntRewardSO : RewardSO
{
    public IntRewardType rewardType;
    public int val;
    public override RewardData CreateRuntimeData()
    {
        return new IntRewardData(this);
    }
}
public enum IntRewardType
{
    MONEY,
    EXP
}

