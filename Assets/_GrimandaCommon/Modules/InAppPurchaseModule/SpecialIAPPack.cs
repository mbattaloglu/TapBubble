using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{

    public class SpecialIAPPack : InAppProduct
    {
        public NonConsumableIAPItem[] nonConsumableIAPItems;
        public ConsumableIAPItem[] ConsumableIAPItems;

        public override void OnPlayerPurchasedThisSpecial()
        {
            Debug.LogError("Purchased:" + productId);
            base.OnPlayerPurchasedThis();
            for (int i = 0; i < ConsumableIAPItems.Length; i++)
            {
                ConsumableIAPItems[i].resource.UpdateResourceAmount(ConsumableIAPItems[i].amount);
            }

            for (int i = 0; i < nonConsumableIAPItems.Length; i++)
            {
                nonConsumableIAPItems[i].setPlayerData(gameController);
            }
        }
    }
}
