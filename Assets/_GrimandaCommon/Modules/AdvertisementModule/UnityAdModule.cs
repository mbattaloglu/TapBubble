using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace Grimanda.Common
{
    public class UnityAdModule : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] AdvertisementModule advertisementModule;
        [SerializeField] string _androidAdUnitId = "Rewarded_Android";
        [SerializeField] string _iOsAdUnitId = "Rewarded_iOS";
        string _adUnitId;

        [SerializeField] string _androidGameId;
        [SerializeField] string _iOsGameId;
        [SerializeField] bool _testMode = true;
        [SerializeField] bool _enablePerPlacementMode = true;
        private string _gameId;



        void Awake()
        {
            InitializeAds();

            // Get the Ad Unit ID for the current platform:
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsAdUnitId
                : _androidAdUnitId;
        }

        public void InitializeAds()
        {
            _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsGameId
                : _androidGameId;
            Advertisement.Initialize(_gameId, _testMode, _enablePerPlacementMode, this);
        }

        // Load content to the Ad Unit:
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            advertisementModule.gameController.debugModule.WriteToDebug("Loading Ad: " + _adUnitId,true,true);
            Advertisement.Load(_adUnitId, this);
        }

        // If the ad successfully loads, add a listener to the button and enable it:
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            advertisementModule.gameController.debugModule.WriteToDebug("Ad Loaded: " + adUnitId);

            if (adUnitId.Equals(_adUnitId))
            {
                advertisementModule.RewardedAdIsReady();
            }
        }

        // Implement a method to execute when the user clicks the button.
        public void ShowRewardedVideo()
        {
            advertisementModule.gameController.debugModule.WriteToDebug("Showing Rewarded Ad");
            Advertisement.Show(_adUnitId, this);
        }

        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                advertisementModule.gameController.debugModule.WriteToDebug("Unity Ads Rewarded Ad Completed");
                advertisementModule.RewardThePlayer();

                // Load another ad:
                Advertisement.Load(_adUnitId, this);
            }
        }

        // Implement Load and Show Listener error callbacks:
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            advertisementModule.gameController.debugModule.WriteToDebug($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            advertisementModule.gameController.debugModule.WriteToDebug($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
            advertisementModule.gameController.debugModule.WriteToDebug("Unity Ads initialization complete.");
            LoadAd();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
            advertisementModule.gameController.debugModule.WriteToDebug($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }
    }
}