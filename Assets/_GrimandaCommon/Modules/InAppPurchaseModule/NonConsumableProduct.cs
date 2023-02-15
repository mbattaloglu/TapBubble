using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DataType {String,Int,Bool,AdditiveInt}
namespace Grimanda.Common
{

    [System.Serializable]
    public struct NonConsumableIAPItem
    {
        public string key;
        public DataType dataType;
        public bool boolValue;
        public int intValue;
        public string stringValue;
        public PlayerResource playerResource;

        public void setPlayerData(GameController gameController)
        {
            switch (dataType)
            {
                case DataType.Bool:
                    gameController.playerData.SetBool(key, boolValue);
                    break;
                case DataType.String:
                    gameController.playerData.SetString(key, stringValue);
                    break;
                case DataType.Int:
                    gameController.playerData.SetInt(key, intValue);
                    break;
                case DataType.AdditiveInt:
                    gameController.playerData.UpdateInt(key, intValue);
                    break;
            }
        }
    }

    public class NonConsumableProduct : InAppProduct
    {

        public NonConsumableIAPItem nonConsumableIAPItem;

        public override void OnPlayerPurchasedThisSpecial()
        {
            base.OnPlayerPurchasedThisSpecial();
            nonConsumableIAPItem.setPlayerData(gameController);
            nonConsumableIAPItem.playerResource.UpdateResourceAmount(1);
        }
    }

}