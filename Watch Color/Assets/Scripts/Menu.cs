using GoogleMobileAds.Api;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI lastText;
    public TextMeshProUGUI bestText;

    BannerView _bannerView;

    string _adUnitId = "ca-app-pub-8576167677253512/9947871116";

    private void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadAd();
        });

        bestText.text = $"Best: {PlayerPrefs.GetInt("Best")}";
        lastText.text = $"Last: {PlayerPrefs.GetInt("Last")}";
        StartCoroutine(ColorSwitch());
    }

    IEnumerator ColorSwitch()
    {
        Vector3 a = UnityEngine.Random.insideUnitSphere * 255;
        bestText.color = new Color(Mathf.RoundToInt(a.x) / 255f, Mathf.RoundToInt(a.y) / 255f, Mathf.RoundToInt(a.z) / 255f);
        Vector3 b = UnityEngine.Random.insideUnitSphere * 255;
        lastText.color = new Color(Mathf.RoundToInt(b.x) / 255f, Mathf.RoundToInt(b.y) / 255f, Mathf.RoundToInt(b.z) / 255f);
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(ColorSwitch());
    }

    public void Play() // Changed to public to make it callable from Unity UI
    {
        SceneManager.LoadScene(1);
    }

    public void CreateBannerView()
    {
        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Top);
        ListenToAdEvents();
    }

    public void LoadAd()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    private void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
}
