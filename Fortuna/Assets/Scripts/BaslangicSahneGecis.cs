using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BaslangicSahneGecis : MonoBehaviour
{
    //Menüdeki sahneler
    public GameObject GirisPanel;
    public GameObject KaydolPanel;
    public Image GuncellePanel;
    public Image SistemPanel;
    public GameObject Kaydol1;
    public GameObject Kaydol2;

    //Menüdeki butonlar
    public Button GirisButon;
    public Button KaydolButon;

    //Olay
    Olay olay;

    //Giris
    KullaniciGirisVT giris;

    //Animasyon
    public GameObject gecisAnimasyonu;

    private void Start()
    {
        gecisAnimasyonu.GetComponent<Animator>().Play("2");
        olay = FindAnyObjectByType<Olay>();
        giris = GetComponent<KullaniciGirisVT>();
        StartCoroutine(sistemKontrol());
    }

    //GÝRÝÞ PANELÝNÝ AÇAR
    public void Giris()
    {
        HerSeyiKapat();
        GirisPanel.gameObject.SetActive(true);
    }

    //ANA EKRAN GERÝ GELÝR BUTONLARI GERÝ GETÝRÝR
    public void Geri()
    {
        HerSeyiKapat();
        GirisButon.gameObject.SetActive(true);
        KaydolButon.gameObject.SetActive(true);
    }

    //ÞÝFRE VE GÜVENLÝK SORUSU PANELÝNÝ AÇAR
    public void Kaydol()
    {
        HerSeyiKapat();
        KaydolPanel.gameObject.SetActive(true);
        Kaydol1.gameObject.SetActive(true);
    }

    //KAYIT ÝÇÝN 2. SAYFAYA GEÇER
    public void Devam()
    {
        Kaydol1.gameObject.SetActive(false);
        Kaydol2.gameObject.SetActive(true);
    }

    //TÜM PANELLERÝ KAPATIR
    void HerSeyiKapat()
    {
        GirisButon.gameObject.SetActive(false);
        KaydolButon.gameObject.SetActive(false);
        GirisPanel.gameObject.SetActive(false);
        KaydolPanel.gameObject.SetActive(false);
        Kaydol1.gameObject.SetActive(false);
        Kaydol2.gameObject.SetActive(false);
    }

    //OYUNDAKÝ SÜRÜMÜ SÜREKLÝ OLARAK KONTROL EDER 
    IEnumerator sistemKontrol()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "sistem");

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/Sistem.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                olay.OlayBaslatici("Connection error");
            }
            else
            {
                yield return new WaitForSeconds(0.5f); //Baþlangýçtaki animasyonu görmek için süre.
                if (www.downloadHandler.text.Split('\n')[0] == "0")
                {
                    SistemPanel.gameObject.SetActive(true);
                }
                else if (www.downloadHandler.text.Split('\n')[1] != Application.version)
                {
                    GuncellePanel.gameObject.SetActive(true);
                    Guncelle();
                }
                else
                {
                    SistemPanel.gameObject.SetActive(false);
                    GuncellePanel.gameObject.SetActive(false);
                    giris.KayitliGiris();
                }
            }
            yield return new WaitForSeconds(10);
            StartCoroutine(sistemKontrol());
        }
    }

    //GÜNCELLEME PANALÝNÝ AÇAR VE YENÝ SÜRÜM LÝNKÝNÝ VERÝR
    public void Guncelle()
    {
#if UNITY_ANDROID
        string playStoreURL = "https://play.google.com/store/apps/details?id=com.Omeryavus.Fortuna";
        Application.OpenURL(playStoreURL);
#elif UNITY_IOS
        string appID = "";
        Application.OpenURL("itms-apps://itunes.apple.com/app/id" + appID);
#else
        string playStoreURL = "https://play.google.com/store/apps/details?id=com.Omeryavus.Fortuna";
        Application.OpenURL(playStoreURL);
#endif
    }
}
