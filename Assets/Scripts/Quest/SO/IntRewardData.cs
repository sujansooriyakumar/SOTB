using UnityEngine;

public class IntRewardData: RewardData
{
    IntRewardType rewardType;
    int value;


    public IntRewardData(IntRewardSO config)
    {
        Config = config;
        rewardType = config.rewardType;
        value = config.val;
    }
}

