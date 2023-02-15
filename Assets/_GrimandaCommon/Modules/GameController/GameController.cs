using UnityEngine;
using UnityEngine.Events;

namespace Grimanda.Common
{
    public class GameController : MonoBehaviour
    {
        public AdvertisementModule advertisementModule;
        public DebugModule debugModule;
        public InAppPurchaseModule inAppPurchaseModule;
        public SoundModule soundManager;
        public CoreGame coreGame;
        public PlayerData playerData;
        public GameConfig activeGameConfig;
        public GrimandaWindow buyCoinsPopup;
        public GrimandaWindow removeAdsPopup;
        public GrimandaWindow failScreen;
        public SettingsScreen settingsScreen;
        public SupportScreen supportScreen;
        public LeaderboardScreen leaderboardScreen;
        public GiftScreen giftScreen;
        public MainMenu mainMenuModule;
        public PauseScreen pauseScreen;
        public AvatarSelectScreen avatarSelectScreen;

        public GameObject[] canvasRoots;

        public UnityEvent OnStartBegin;
        public UnityEvent OnStartEnd;

        public void ChangeActiveGameConfig(GameConfig gameConfig)
        {
            activeGameConfig = gameConfig;
        }

        public void DoCommonThingsAtStart()
        {
            activeGameConfig = Resources.Load<GameConfig>("Data/GameConfig1");

            for (int i = 0; i < canvasRoots.Length; i++)
            {
                for (int j = 0; j < canvasRoots[i].transform.childCount; j++)
                {
                    canvasRoots[i].transform.GetChild(j).gameObject.SetActive(false);
                }
            }

            debugModule.ConnectToGameController(this);
            inAppPurchaseModule.ConnectToGameController(this);
            mainMenuModule.ConnectToGameController(this);
            avatarSelectScreen.ConnectToGameController(this);
            advertisementModule.ConnectToGameController(this);
            soundManager.ConnectToGameController(this);
            coreGame.ConnectToGameController(this);
            coreGame.coreGameScreen.ConnectToGameController(this);
            pauseScreen.ConnectToGameController(this);
            settingsScreen.ConnectToGameController(this);
            failScreen.ConnectToGameController(this);
            buyCoinsPopup.ConnectToGameController(this);
            removeAdsPopup.ConnectToGameController(this);
            supportScreen.ConnectToGameController(this);
            leaderboardScreen.ConnectToGameController(this);
            giftScreen.ConnectToGameController(this);

            mainMenuModule.OpenDialog();
            playerData.activeAvatar = playerData.GetActiveAvatar(activeGameConfig.avatars);

            soundManager.PlayMusic();
        }

        public virtual void DoGameSpecificThingsAtStart()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            OnStartBegin.Invoke();
            DoCommonThingsAtStart();
            DoGameSpecificThingsAtStart();
            OnStartEnd.Invoke();
        }
    }
}
