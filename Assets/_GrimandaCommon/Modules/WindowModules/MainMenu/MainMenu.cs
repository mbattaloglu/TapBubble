using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Grimanda.Common
{
    public class MainMenu : GrimandaWindow
    {
        [SerializeField] GameObject RemoveAdsButton;
        [SerializeField] TextMeshProUGUI TMPCoinCount;
        [SerializeField] Transform avatarRoot;
        //[SerializeField] Vector3 avatarScale;
        [SerializeField] Transform MainMenuMiddleRoot;
        [SerializeField] Transform AvatarIconRoot;

        public void PlayButtonOnClick()
        {
            SetFade(true);
            SetCloseAndopenAction(CloseActions.PlayCoreGame);
            nextDialogToBeOpenedWhenClosed = gameController.coreGame.coreGameScreen;
            animatableUIElements[0].fadeOutAtEnd = true;
            CloseDialog();
        }

        public void LeaderboardButtonOnClick()
        {
            SetNextDialogToBeOpen(gameController.leaderboardScreen);
            CloseDialog();
        }
        public void GiftButtonOnClick()
        {
            SetNextDialogToBeOpen(gameController.giftScreen);
            CloseDialog();

        }
        public void SupportButtonOnClick()
        {
            SetNextDialogToBeOpen(gameController.supportScreen);
            CloseDialog();
        }

        public override void OnCloseComplete()
        {
            base.OnCloseComplete();
            if (closeAction==CloseActions.PlayCoreGame)
            {
                gameController.coreGame.StartPlayingGame();
            }
        }

        public void SettingsButtonOnClick()
        {
            SetFade(false);
            gameController.soundManager.PlaySoundClip(SoundNames.ButtonClick1);
            //animatableUIElements[0].fadeOutAtEnd = false;
            nextDialogToBeOpenedWhenClosed = gameController.settingsScreen;
            CloseDialog();
        }

        public void ChangeAvatar()
        {
            SetFade(false);
            Destroy(avatarRoot.GetChild(0).gameObject);
            animatableUIElements[0].fadeOutAtEnd = false;
            Transform newAvatarObject = Instantiate(gameController.playerData.activeAvatar.prefab, avatarRoot).transform;
            newAvatarObject.localScale = gameController.activeGameConfig.avatarScale;
            newAvatarObject.transform.Translate(gameController.activeGameConfig.avatarOffset);

        }

        public override void DoSpecificThingsAtOpen()
        {
            base.DoSpecificThingsAtOpen();

            if(AvatarIconRoot.childCount==0)
            {
                Instantiate(gameController.activeGameConfig.specialAvatarIcon, AvatarIconRoot);
            }
            if(MainMenuMiddleRoot.childCount==0)
            {
                Instantiate(gameController.activeGameConfig.specialMainMenuPrefab,MainMenuMiddleRoot);
            }

            if (gameController.playerData.GetBool(PlayerDataTags.RemoveAds))
            {
                RemoveAdsButton.SetActive(false);
            }

            TMPCoinCount.text = gameController.playerData.GetInt(PlayerDataTags.CoinCount).ToString();

            if(avatarRoot.childCount==0)
            {
                Transform newAvatarObject=Instantiate(gameController.playerData.activeAvatar.prefab, avatarRoot).transform;
                newAvatarObject.localScale = gameController.activeGameConfig.avatarScale;
                newAvatarObject.transform.Translate(gameController.activeGameConfig.avatarOffset);
            }
        }
    }
}