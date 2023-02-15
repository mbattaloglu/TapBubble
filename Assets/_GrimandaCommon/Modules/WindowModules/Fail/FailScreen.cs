using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Grimanda.Common
{
    public class FailScreen : Grimanda.Common.GrimandaWindow
    {
        [SerializeField] TMP_Text scoreText;
        [SerializeField] TMP_Text normalBextScoreText;
        [SerializeField] SecondChanceReward secondChancereward;
        [SerializeField] GameObject continueWithRewardedAdButton;
        [SerializeField] GameObject normalScoreRoot;
        [SerializeField] GameObject newBestScoreRoot;
        [SerializeField] TextMeshProUGUI newBestScoreText;
        [SerializeField] GameObject adIsNotReadyRoot;

        public override void DoSpecificThingsAtOpen()
        {
            base.DoSpecificThingsAtOpen();
            continueWithRewardedAdButton.SetActive(false);
            if (!gameController.coreGame.isAdUsedInThisTurn)
            {
                continueWithRewardedAdButton.SetActive(true);
            }

            if (gameController.coreGame.GetScore() > gameController.playerData.GetInt(PlayerDataTags.PlayerBestScore))
            {
                normalScoreRoot.gameObject.SetActive(false);
                newBestScoreRoot.gameObject.SetActive(true);
                newBestScoreText.text = gameController.coreGame.GetScore().ToString();
                gameController.playerData.SetInt(PlayerDataTags.PlayerBestScore, gameController.coreGame.GetScore());
            }
            else
            {
                normalScoreRoot.gameObject.SetActive(true);
                newBestScoreRoot.gameObject.SetActive(false);
                normalBextScoreText.text = "Best Score:" + gameController.playerData.GetInt(PlayerDataTags.PlayerBestScore);
                scoreText.text = "SCORE: " + gameController.coreGame.GetScore();
            }

            adIsNotReadyRoot.SetActive(false);
        }

        public void ContinueOnClick()
        {
            if (gameController.advertisementModule.isRewardedAdReady())
            {
                CloseDialog();
                gameController.failScreen.SetFade(true);
                nextDialogToBeOpenedWhenClosed = gameController.coreGame.coreGameScreen;
                gameController.advertisementModule.ShowRewarded(secondChancereward);
            }
            else
            {
                adIsNotReadyRoot.SetActive(true);
            }
        }

        public void QuitOnClick()
        {
            SetFade(false);
            gameController.mainMenuModule.SetFade(false);
            nextDialogToBeOpenedWhenClosed = gameController.mainMenuModule;
            CloseDialog();
        }

        public void Restart()
        {
            nextDialogToBeOpenedWhenClosed = gameController.coreGame.coreGameScreen;
            CloseDialog();
            gameController.coreGame.PlayerRestartedTheGame();
        }

    }
}