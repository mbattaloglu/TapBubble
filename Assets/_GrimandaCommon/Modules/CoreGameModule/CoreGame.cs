using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum FailReason { Stuck, OutOfMoves }

namespace Grimanda.Common
{
    public class CoreGame : MonoBehaviour
    {
        public GameController gameController;
        public CoreGameScreen coreGameScreen;
        public bool isGamePaused = true;
        public GameObject[] thingsToBeHiddenWhenDeactivated;
        public bool hideCoreGameElementsAtDeactivation;

        public UnityEvent OnGamePaused;
        public UnityEvent OnGameStarted;
        public UnityEvent OnPlayerFailed;
        public UnityEvent OnPlayerWon;
        public UnityEvent OnSecondChance;

        public Transform currentAvatarVisualRoot;

        int score=0;

        private int coinsEarnedInThisSeesion=0;
        public Transform coinIcon;
        public bool isAdUsedInThisTurn;

        public void SetAdUsage(bool isAdUsedInThisTurn)
        {
            this.isAdUsedInThisTurn = isAdUsedInThisTurn;
        }

        public void ConnectToGameController(GameController gameController)
        {
            this.gameController = gameController;
            InitCoreGame();
        }


        public virtual void DoGameSpecificThingsStart()
        {
            gameObject.SetActive(false);
            isGamePaused = true;
        }

        public void InitCoreGame()
        {
            DoGameSpecificThingsStart();
        }


        public virtual int GetScore()
        {
            return score;
        }

        public virtual void SetScore(int score)
        {
            this.score = score;
            coreGameScreen.UpdateScoreText();
        }

        public virtual void IncreaseScore(int delta)
        {
            score += delta;
            coreGameScreen.UpdateScoreText();
        }


        public virtual int GetCoinsEarnedInThisSession()
        {
            return coinsEarnedInThisSeesion;
        }

        public virtual void SetCoinCountsEarnedInThisSession(int score)
        {
            this.coinsEarnedInThisSeesion = score;
            coreGameScreen.UpdateCoinCountText();
        }

        public virtual void IncreaseCoinCountsEarnedInThisSession(int delta)
        {
            coinsEarnedInThisSeesion += delta;
            coreGameScreen.UpdateCoinCountText();
        }


        public virtual void SecondChance(FailReason failReason)
        {
            gameObject.SetActive(true);
        }

        public virtual void PlayerQuitedTheGame()
        {
            gameObject.SetActive(false);
        }

        public virtual void PlayerRestartedTheGame()
        {
            gameObject.SetActive(true);
            isGamePaused = false;
        }

        public virtual void StartPlayingGame()
        {
            Debug.LogError("Start Playing Level");
            gameObject.SetActive(true);
            SetCoinCountsEarnedInThisSession(0);
            SetScore(0);
            coreGameScreen.OpenDialog();

            OnGameStarted.Invoke();

            if (currentAvatarVisualRoot.childCount > 0)
            {
                if (currentAvatarVisualRoot.GetChild(0).name != gameController.playerData.activeAvatar.prefab.name)
                {
                    Destroy(currentAvatarVisualRoot.GetChild(0).gameObject);
                    GameObject g = Instantiate(gameController.playerData.activeAvatar.prefab, currentAvatarVisualRoot);
                    g.name = gameController.playerData.activeAvatar.prefab.name;
                }
            }

            if (currentAvatarVisualRoot.childCount == 0)
            {
                GameObject g = Instantiate(gameController.playerData.activeAvatar.prefab, currentAvatarVisualRoot);
                g.name = gameController.playerData.activeAvatar.prefab.name;
            }

            SetAdUsage(false);
        }

        public virtual void PlayerFailed()
        {
            gameObject.SetActive(false);
            isGamePaused = true;
            OnGamePaused.Invoke();
            gameController.failScreen.SetFade(true);
            gameController.soundManager.SetAdditionalAudioSources(isGamePaused);
        }

        public virtual void PlayerPaused()
        {
            gameObject.SetActive(false);
            isGamePaused = true;
            OnGamePaused.Invoke();
            coreGameScreen.SetNextDialogToBeOpen(gameController.pauseScreen);
            coreGameScreen.CloseDialog();
            gameController.soundManager.SetAdditionalAudioSources(isGamePaused);
        }

        public virtual void PlayerRestartedGame()
        {
            gameObject.SetActive(true);
            SetScore(0);
            coreGameScreen.OpenDialog();
            gameController.coreGame.SetAdUsage(false);
        }

        public virtual void PlayerWon()
        {
            gameObject.SetActive(true);
            OnPlayerWon.Invoke();
        }

        public virtual void SecondChance()
        {
            gameObject.SetActive(true);
            OnSecondChance.Invoke();
            coreGameScreen.OpenDialog();
            SetAdUsage(true);
        }

        public virtual void ResumeGame()
        {
            gameObject.SetActive(true);
            isGamePaused = false;
            gameObject.SetActive(true);
            gameController.soundManager.SetAdditionalAudioSources(isGamePaused);
        }

        public virtual void PlayerCollectedResourceInGame(PlayerResource playerResource)
        {

        }
    }
}