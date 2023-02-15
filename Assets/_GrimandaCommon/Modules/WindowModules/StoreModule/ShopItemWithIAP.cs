using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    public class ShopItemWithIAP : ShopItem
    {
        public InAppProduct inAppProduct;

        public override void OnClickPrivate()
        {
            gameController.inAppPurchaseModule.BuyProduct(inAppProduct);
        }

    }
}
