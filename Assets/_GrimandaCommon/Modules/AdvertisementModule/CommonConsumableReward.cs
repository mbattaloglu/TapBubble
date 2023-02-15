using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    [CreateAssetMenu(fileName = "Common Consumable Reward", menuName = "CommonConsumableReward", order = 57)]
    public class CommonConsumableReward : AdReward
    {
        public override void RewardThePlayer()
        {
            Debug.LogError("Reward The Player");

            reward.UpdateResourceAmount(rewardCount);
        }
    }
}
