using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    [CreateAssetMenu(fileName = "SecondChanceReward", menuName = "Second Chance Reward", order = 57)]
    public class SecondChanceReward : AdReward
    {
        public override void RewardThePlayer()
        {
            GameObject.Find("GameController").GetComponent<GameController>().coreGame.SecondChance();
        }
    }
}
