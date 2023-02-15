using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Grimanda.Common
{
    public class PriceText : MonoBehaviour
    {
        public InAppProduct inAppProduct;

        public void UpdatePriceText()
        {
            GetComponent<TextMeshProUGUI>().text = inAppProduct.price;
        }
    }
}
