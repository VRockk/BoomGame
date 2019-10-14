using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;

// Call this way
// AdsController.adsInstance.ShowVideoOrInterstitialAds();
public class AdsController : MonoBehaviour
{
    public static AdsController adsInstance;
    
    
    private string googlePlayStoreId = "3324943";
    private string appleAppStoreId = "3324942";

    private string video_ad = "video";
    private string rewarded_video_ad = "rewardedVideo";
    private string banner_ad = "bannerAds";

    private void Awake()
    {
        if(adsInstance != null)
        {
            Destroy(gameObject);
        } else
        {
            adsInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        //Check the platform
        string gameId = googlePlayStoreId;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            gameId = appleAppStoreId;
        }
        Monetization.Initialize(gameId, true);
    }

    // Update is called once per frame
    void Update()
    {
    /*    
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Monetization.IsReady(video_ad)){
                ShowAdPlacementContent ad = null;
                ad = Monetization.GetPlacementContent(video_ad) as ShowAdPlacementContent;

                if (ad != null)
                {
                    ad.Show();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Monetization.IsReady(rewarded_video_ad))
            {
                Debug.Log("test");
                ShowAdPlacementContent ad = null;
                ad = Monetization.GetPlacementContent(rewarded_video_ad) as ShowAdPlacementContent;

                if (ad != null)
                {
                    ad.Show();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Monetization.IsReady(banner_ad))
            {
                Debug.Log("test");
                ShowAdPlacementContent ad = null;
                ad = Monetization.GetPlacementContent(banner_ad) as ShowAdPlacementContent;

                if (ad != null)
                {
                    ad.Show();
                }
            }
        }*/
    }
    public void ShowVideoOrInterstitialAds()
    {
        //Play videoAds
        if (Monetization.IsReady(video_ad))
        {
            ShowAdPlacementContent ad = null;
            ad = Monetization.GetPlacementContent(video_ad) as ShowAdPlacementContent;

            if (ad != null)
            {
                ad.Show();
            }
        }
    }
}