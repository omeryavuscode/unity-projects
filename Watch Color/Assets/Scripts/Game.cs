using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;

public class Game : MonoBehaviour
{
    //Buton Sesi
    public AudioSource click;

    //Tekrar Edilmesi Gereken Tuþlar.
    public List<Button> record;
    public List<Button> buttons;

    //Doðru renk kaç kere yanýp sönsün.
    public int trueColor;

    // Dönüþülecek Renkler
    public List<Color> colors;

    //Yeni Liste Gösterilsin Mi Kontrolü.
    bool callFinish = true;

    int i = 0;
    int best = 0;

    BannerView _bannerView;

    string _adUnitId = "ca-app-pub-8576167677253512/9947871116";

    private void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadAd();
        });

        best = PlayerPrefs.GetInt("Best");
        callFinish = true;
    }

    private void Update()
    {
        if (callFinish == true) // Çaðýrýlma Booleaný True Ýse Yeni Liste Gösterilir.
        {
            callFinish = false; // Çaðýrýlma Booleaný Kapatýlýr.
            BackColor();
            StartCoroutine(callColor());
        }
    }

    // Asýl Oyun.
    IEnumerator callColor()
    {
        BackColor();
        record.Add(buttons[UnityEngine.Random.Range(0, 4)]); // Rastgele Renkler Çaðrýlýr Ve Kaydedilir.
        yield return new WaitForSeconds(2f);

        foreach (Button color in record) // Kayýtta Olan Renkler Kullanýcýya Gösterilir.
        {
            yield return new WaitForSeconds(0.5f);

            //Renk Tespiti Ve Renk Deðiþimi
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i] == color)
                {
                    buttons[i].image.color = colors[i];
                    break;
                }
            }

            yield return new WaitForSeconds(0.5f); // Renk 1 Saniye Aktif Kalýr.

            BackColor(); // Beyaz Renge Geri Dönülür.
        }

        ButtonEnabled();
    }

    // Butonlarý Beyaza Çeviren Fonksiyon.
    void BackColor()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].image.color = colors[4];
            buttons[i].enabled = false;
        }
    }

    //Butonlarý aktif hale getirmek
    void ButtonEnabled()
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].enabled = true;
    }

    // Renk Butonlarýna Basýldýðýnda Butonlarýn Boolean Deðerleri True Olur.
    public void blackClick()
    {
        StartCoroutine(Control(buttons[0]));
    }
    public void greenClick()
    {
        StartCoroutine(Control(buttons[1]));
    }
    public void redClick()
    {
        StartCoroutine(Control(buttons[2]));
    }
    public void blueClick()
    {
        StartCoroutine(Control(buttons[3]));
    }

    IEnumerator Control(Button value)
    {
        click.Play();

            if (record[i] == value)
            {
                i += 1;
                if (record.Count == i) // Dizinin/listenin uzunluðu
                {
                    GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = i.ToString();
                    if (i > best)
                    {
                        best = i;
                        PlayerPrefs.SetInt("Best", best);
                    }
                    i = 0;
                    callFinish = true; // Tüm butonlara doðru bastýn yeni renk oluþtur
                }
            }
            else
            {
                BackColor();
                value.image.color = colors[buttons.IndexOf(value)];
                for (int k = 0; k < trueColor; k++)
                {
                    record[i].image.color = colors[buttons.IndexOf(record[i])];
                    yield return new WaitForSeconds(0.25f);
                    record[i].image.color = colors[4];
                    yield return new WaitForSeconds(0.25f);
                }
                yield return new WaitForSeconds(1f);
                PlayerPrefs.SetInt("Last", record.Count - 1);
                SceneManager.LoadScene(0);
            }
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