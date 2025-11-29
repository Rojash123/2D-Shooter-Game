using GoogleMobileAds.Api;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    bool isAdmobInitialized;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private Action<bool> adCallBack;
    public override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        isAdmobInitialized = false;
        InitializeAdSDK();
    }
    internal void InitializeAdSDK()
    {
        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                isAdmobInitialized = false;
                return;
            }
            isAdmobInitialized = true;
            LoadInterstitialAd();
            LoadRewardedAd();
        });
    }

    #region Interstitital Ads
    void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        var adRequest = new AdRequest();
        InterstitialAd.Load(AdsData.interstitialAdId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                return;
            }
            interstitialAd = ad;
            RegisterEvent(interstitialAd);
        });
    }
    void RegisterEvent(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            // Raised when the ad is estimated to have earned money.
        };
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            
        };
        interstitialAd.OnAdClicked += () =>
        {
            // Raised when a click is recorded for an ad.
        };
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            // Raised when the ad opened full screen content.
        };
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
           LoadInterstitialAd();
        };
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            adCallBack?.Invoke(false);
            adCallBack = null;
        };
    }
    public void ShowInterstitialAds()
    {
        if (!isAdmobInitialized)
        {
            InitializeAdSDK();
            adCallBack?.Invoke(false); adCallBack = null;
            return;
        }
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        
    }
    
    #endregion

    #region RewardedAds
    void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        var adRequest = new AdRequest();

        RewardedAd.Load(AdsData.rewardedAdId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                return;
            }
            rewardedAd = ad;
            RegisterEvent(rewardedAd);
        });
    }
    void RegisterEvent(RewardedAd rewardedAd)
    {
        rewardedAd.OnAdPaid += (AdValue adValue) =>
        {
            // Raised when the ad is estimated to have earned money.
        };
        rewardedAd.OnAdImpressionRecorded += () =>
        {
            HandleAdWatched();
        };
        rewardedAd.OnAdClicked += () =>
        {
            // Raised when a click is recorded for an ad.
        };
        rewardedAd.OnAdFullScreenContentOpened += () =>
        {
            // Raised when the ad opened full screen content.
        };
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            LoadRewardedAd();
        };
        rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            adCallBack?.Invoke(false);
            adCallBack = null;
        };
    }
    public void ShowRewardedAd(Action<bool> callBack)
    {
        adCallBack = callBack;
        if (!isAdmobInitialized)
        {
            InitializeAdSDK();
            adCallBack?.Invoke(false); adCallBack = null;
            return;
        }


        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
            });
        }
        else
        {
            adCallBack?.Invoke(false);adCallBack = null;
        }
    }

    #endregion

    void HandleAdWatched()
    {
        adCallBack.Invoke(true);adCallBack = null;
    }
}
