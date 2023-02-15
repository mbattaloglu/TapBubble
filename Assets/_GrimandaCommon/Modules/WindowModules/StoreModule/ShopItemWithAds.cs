using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    public class ShopItemWithAds : ShopItem
    {
        public AdReward adReward;

        public override void OnClickPrivate()
        {
            Debug.LogError("Click On Shop Item");

            gameController.advertisementModule.ShowRewarded(adReward);
        }
    }
}
