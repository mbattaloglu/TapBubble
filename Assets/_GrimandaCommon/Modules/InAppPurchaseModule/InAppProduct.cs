using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    public class InAppProduct : MonoBehaviour
    {
        public GameController gameController;
        public string productId;
        public string title;
        public string price;
        public SoundNames soundName;
        [SerializeField] GameObject objectToBeHideAfterPurchase;
        [SerializeField] GameObject objectToBeShownAfterPurchase;

        public void ConnectToGameController(GameController gameController)
        {
            objectToBeShownAfterPurchase.GetComponent<GrimandaWindow>().ConnectToGameController(gameController);
            this.gameController = gameController;
        }

        public void OnPlayerPurchasedThis()
        {
            GameObject.Find("GameController").GetComponent<GameController>().soundManager.PlaySoundClip(soundName);
            objectToBeHideAfterPurchase.GetComponent<GrimandaWindow>().SetNextDialogToBeOpen(objectToBeShownAfterPurchase.GetComponent<GrimandaWindow>());
            objectToBeHideAfterPurchase.GetComponent<GrimandaWindow>().CloseDialog();
            OnPlayerPurchasedThisSpecial();
        }
        public virtual void OnPlayerPurchasedThisSpecial()
        {

        }
    }
}