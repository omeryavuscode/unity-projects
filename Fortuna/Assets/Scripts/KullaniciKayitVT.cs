using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class KullaniciKayitVT : MonoBehaviour
{
    //Kullanýcý Bilgileri
    public TMP_InputField girilenKullaniciAdi;
    public TMP_InputField girilenSifre;
    public TMP_InputField girilenSifreTekrar;
    public TextMeshProUGUI guvenlikSorusu;
    public TMP_InputField girilenGuvenlikCevabi;

    //Olay Kutusu
    public string olayMetni;
    Olay olay;

    //Butonlar
    public Button devamButonu;
    public Button kaydolButon;

    //Girilen eski kullanýcý adý
    string eskiGirilenKullaniciAdi = "";

    private void Start()
    {
        olay = FindAnyObjectByType<Olay>();
    }

    private void Update()
    {
        Kontroller();
    }

    //OYUNDAKÝ KONTROLLERÝ SAÐLAR
    void Kontroller()
    {

        //GÝRÝLEN KULLANICI ADI DEÐÝÞTÝ MÝ
        if (girilenKullaniciAdi.text != eskiGirilenKullaniciAdi)
        {
            StartCoroutine(KullaniciAdiSorgu()); //Kullanýcý Adý kontrolü
            eskiGirilenKullaniciAdi = girilenKullaniciAdi.text;
        }

        //KULLANICI ADI KULLANILABÝLÝRLÝÐÝNÝ KONTROL EDER
        if (girilenKullaniciAdi.text.Length >= 4 && girilenKullaniciAdi.text.Length <= 10 && Regex.IsMatch(girilenKullaniciAdi.text, "^[a-zA-Z0-9]*$") &&
            olayMetni == $"{girilenKullaniciAdi.text.ToLower()} is available" && olayMetni != "Connection error")
        {
            devamButonu.interactable = true;
        }
        else
        {
            devamButonu.interactable = false;
        }


        //ÞÝFRE DOÐRULAMA
        if (girilenSifre.text == girilenSifreTekrar.text && girilenSifre.text.Length >= 4 && girilenSifre.text.Length <= 10 && !girilenKullaniciAdi.text.Contains(" "))
        {
            MatchCollection sayilar = Regex.Matches(guvenlikSorusu.text, @"\d+");

            if (girilenGuvenlikCevabi.text != "")
            {
                int sayi1 = int.Parse(sayilar[0].Value);
                int sayi2 = int.Parse(sayilar[1].Value);

                if (sayi1 + sayi2 == int.Parse(girilenGuvenlikCevabi.text))

                    kaydolButon.interactable = true;
                else
                    kaydolButon.interactable = false;
            }
        }
        else
            kaydolButon.interactable = false;
    }


    //KULLANICI ADI KULLANILABÝLÝRLÝÐÝ SORGUSU
    IEnumerator KullaniciAdiSorgu()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "kullaniciAdiSorgu");
        form.AddField("girilenKullaniciAdi", girilenKullaniciAdi.text.ToLower());


        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/KullaniciKayitlari.php", form))
        {
            yield return www.SendWebRequest();

            string sonuc;

            if (www.result != UnityWebRequest.Result.Success)
            {
                sonuc = "Connection error";
            }
            else
                sonuc = www.downloadHandler.text;

            olayMetni = sonuc;
            olay.OlayBaslatici(olayMetni);
        }
    }


    public void DevamButonu()
    {
        StartCoroutine(Kaydol1());
    }


    //KULLANICI ADI SORGULAMA VE KULLANICI ADI ÝÇÝN BÝR GÜVENLÝK SORUSU OLUÞTURMA
    IEnumerator Kaydol1()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "guvenlik");
        form.AddField("girilenKullaniciAdi", girilenKullaniciAdi.text.ToLower());

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/KullaniciKayitlari.php", form))
        {
            yield return www.SendWebRequest();

            string sonuc;

            if (www.result != UnityWebRequest.Result.Success)
            {
                sonuc = www.downloadHandler.text;
            }
            else
            {
                sonuc = www.downloadHandler.text;
                guvenlikSorusu.text = sonuc;
            }

        }
    }


    public void KaydolButonu()
    {
        StartCoroutine(Kaydol2());
    }


    //KAYDOLMA
    IEnumerator Kaydol2()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "kaydol");
        form.AddField("girilenKullaniciAdi", girilenKullaniciAdi.text.ToLower());
        form.AddField("girilenSifre", girilenSifre.text);
        form.AddField("girilenGuvenlikCevabi", girilenGuvenlikCevabi.text);

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/KullaniciKayitlari.php", form))
        {
            yield return www.SendWebRequest();

            string sonuc;

            if (www.result != UnityWebRequest.Result.Success)
            {
                sonuc = www.downloadHandler.text;
            }
            else
            {
                sonuc = www.downloadHandler.text;
                PlayerPrefs.SetString("kullaniciAdi", girilenKullaniciAdi.text.ToLower());
                PlayerPrefs.SetString("sifre", girilenSifre.text);
                GetComponent<KullaniciGirisVT>().KayitliGiris();
            }

            olayMetni = sonuc;
            olay.OlayBaslatici(sonuc);
        }
    }
}
