using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    [System.Serializable]
    public struct ConsumableIAPItem
    {
        public int amount;
        public PlayerResource resource;
    }

    public class ConsumableProduct : InAppProduct
    {
        public ConsumableIAPItem consumableIAPItem;

        public override void OnPlayerPurchasedThisSpecial()
        {
            Debug.LogError("Purchased:" + productId);
            base.OnPlayerPurchasedThisSpecial();

            consumableIAPItem.resource.UpdateResourceAmount(consumableIAPItem.amount);
        }
    }
}
