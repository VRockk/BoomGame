using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsController : MonoBehaviour
{
    private string videoPlacement = "video";


#if UNITY_IOS
    private string gameId = "3324942";
#elif UNITY_ANDROID
    private string gameId = "3324943";
#endif

    private static AdsController instance;

    public static AdsController Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            Advertisement.Initialize(gameId);
        }
    }
    void OnDestroy()
    {
        if (this == instance)
        {
            instance = null;
        }
    }

    public AdsController()
    {
    }

    public void ShowVideoAd()
    {
        StartCoroutine(ShowVideoWhenReady());
    }

    public void ShowBannerAd()
    {
    }

    public IEnumerator ShowVideoWhenReady()
    {
        while (!Advertisement.IsReady(videoPlacement))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Show(videoPlacement);
    }
}