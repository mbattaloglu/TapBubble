using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    [CreateAssetMenu(fileName = "New Reward", menuName = "Reward", order = 57)]
    public class AdReward : ScriptableObject
    {
        public string rewardName;
        public PlayerResource reward;
        public int rewardCount;

        public virtual void RewardThePlayer()
        {
            Debug.LogError("Rewarding The Player:" + rewardName);
        }
    }
}
