using com.unity3d.mediation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPlayAds : MonoBehaviour
{
    private string adContext;

    // Start is called before the first frame update
    void Start()
    {
        //IronSource.Agent.setMetaData("is_test_suite", "enable");

        // Init the SDK when implementing the Multiple Ad Units API for Interstitial and Banner formats, with Rewarded using legacy APIs 
        LevelPlayAdFormat[] legacyAdFormats = new[] { LevelPlayAdFormat.REWARDED }; 
        LevelPlay.Init("20993bd25", "UserId", legacyAdFormats);
    }

    private void OnEnable()
    {
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
    }

    private void OnDisable()
    {
        // Unsubscribe from events to prevent multiple listeners
        LevelPlay.OnInitSuccess -= SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed -= SdkInitializationFailedEvent;

        IronSourceRewardedVideoEvents.onAdOpenedEvent -= RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent -= RewardedVideoOnAdClickedEvent;
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    private void SdkInitializationCompletedEvent(LevelPlayConfiguration configuration)
    {
        //Debug.Log($"SDK initialization completed successfully. Configuration: {configuration}");
        //IronSource.Agent.launchTestSuite(); // Launch test suite
    }

    private void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.LogError($"SDK initialization failed. Error code: {error.ErrorCode}, Message: {error.ErrorMessage}");
    }

    public void ShowRewardedAd(string context)
    {
        adContext = context;

        if(IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("No Reward Video Available");
        }
    }

    //Rewarded Ad Callbacks

    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        Debug.Log("######################################### REWARDED ###############################################:      " + adContext);

        switch (adContext)
        {
            case "100Credits":
                CurrencyManager.credits += 100;
                PlayerPrefs.SetInt("credits", CurrencyManager.credits);
                PlayerPrefs.Save();
                break;
            case "Revive":
                Player _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

                if (PerksManager.hasBetterArmour == true)
                {
                    _playerScript.health = 2;
                }
                else
                {
                    _playerScript.health = 1;
                }

                GameObject[] Pirates = GameObject.FindGameObjectsWithTag("Pirate");
                foreach (GameObject pirate in Pirates)
                {
                    pirate.SetActive(false);
                }

                _playerScript.DeathOverlay.SetActive(false);

                _playerScript.ReviveLastDamageTimeUpdate();

                _playerScript.HealthRegenTimer.GetComponent<Image>().fillAmount = 0;
                Time.timeScale = 1.0f;
                break;
            default:
                Debug.LogWarning("Unknown context for rewarded ad.");
                break;
        }
    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
    }
    private void OnDestroy()
    {
        OnDisable();
    }
}
