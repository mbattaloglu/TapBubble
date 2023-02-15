using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    public class ShopItem : MonoBehaviour
    {
        protected GameController gameController;

        public virtual void OnClickPrivate()
        {

        }

        public void OnClick()
        {
            if (gameController == null)
            {
                gameController = GameObject.Find("GameController").GetComponent<GameController>();
            }
            OnClickPrivate();
        }
    }
}
