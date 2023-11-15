using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    public bool isAdsFree = true;
    public bool isTest = true;
    public GameObject adsPlaceholder = null;
    private static bool isCreated = false;
    private BannerView bannerAd = null;
    [SerializeField] private string appID = "";
    [SerializeField] private string bannerID = "";


    void Awake()
    {


        if (isAdsFree) { adsPlaceholder.SetActive(!isAdsFree); }
        else
        {
            if (!isCreated)
            {
                DontDestroyOnLoad(gameObject);
                isCreated = true;
                if (isTest) { bannerID = "ca-app-pub-3940256099942544/6300978111"; }
                Init();
                RequestBanner();
            }
            else { Destroy(gameObject); }
        }

    }

    private void Init() { MobileAds.Initialize(appID); }
    private void RequestBanner()
    {
        if (bannerAd != null) { bannerAd.Destroy(); }
        bannerAd = new BannerView(bannerID, AdSize.Banner, AdPosition.Top);
        AdRequest request = new AdRequest.Builder().Build();
        bannerAd.LoadAd(request);
    }
    void OnApplicationQuit()
    {
        if (bannerAd != null) { bannerAd.Destroy(); }
    }
}
