using UnityEngine;
using UnityEngine.Advertisements;


namespace Grimanda.Common
{
    public class AdvertisementModule : MonoBehaviour
    {
        AdReward currentReward;
        public GameController gameController;
        private bool rewardedAdReady;

        public void ConnectToGameController(GameController gameController)
        {
            this.gameController = gameController;
        }


        public void Init()
        {

        }

        public bool isRewardedAdReady()
        {
#if DEBUG_ENABLED
            if (gameController.playerData.GetBool(PlayerDataTags.ByPassRewardedAd))
            {
                return true;
            }
#endif
            return rewardedAdReady;

        }

        public void ShowRewarded(AdReward reward)
        {
            currentReward = reward;
            Debug.LogError("Showing Rewarded Ad:" + reward.rewardName);
            
#if DEBUG_ENABLED
            if (gameController.playerData.GetBool(PlayerDataTags.ByPassRewardedAd))
            {
                Debug.LogError("Rewarding in progress");
                RewardThePlayer();
                return;
            } else
            {
                Debug.Log("AD not present");
            }
#endif
            GetComponent<UnityAdModule>().ShowRewardedVideo();
        }

        public void RewardedAdIsReady()
        {
            rewardedAdReady = true;
        }

        public void RewardThePlayer()
        {
            currentReward.RewardThePlayer();
        }

        public void ShowInterstitial()
        {

        }

    }
}
