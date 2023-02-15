using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{

    [CreateAssetMenu(fileName = "New Resource", menuName = "Resource", order = 57)]
    public class PlayerResource : ScriptableObject
    {
        public string ResourceName;
        public string PlayerDataTag;

        public int maximumAmount;
        GameController gameController = null;

        public void UpdateResourceAmount(int delta)
        {
            if (gameController == null)
            {
                gameController = GameObject.Find("GameController").GetComponent<GameController>();
            }
            gameController.playerData.UpdateInt(PlayerDataTag, delta);
        }

        public void SetCurrentAmount(int amount)
        {
            if (gameController == null)
            {
                gameController = GameObject.Find("GameController").GetComponent<GameController>();
            }

            gameController.playerData.SetInt(PlayerDataTag,amount);
        }

        public int GetCurrentAmount()
        {
            if (gameController == null)
            {
                gameController = GameObject.Find("GameController").GetComponent<GameController>();
            }
            return gameController.playerData.GetInt(PlayerDataTag);
        }


    }

}