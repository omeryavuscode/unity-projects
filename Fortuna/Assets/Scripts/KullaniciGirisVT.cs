using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class KullaniciGirisVT : MonoBehaviour
{
    //Kullanýcý giriþ bilgileri
    public TMP_InputField girilenKullaniciAdi;
    public TMP_InputField girilenSifre;
    string kullaniciAdi;
    string sifre;

    //Olay gösterimi araçlarý
    public string olayMetni;
    Olay olay;

    //Giriþ butonu
    public Button girisButon;

    //Animasyon
    public GameObject gecisAnimasyonu;

    private void Start()
    {
        olay = FindAnyObjectByType<Olay>();
    }

    private void Update()
    {
        Kontroller();
    }

    //OYUDNAKÝ KONTROLLERÝ SAÐLAR
    void Kontroller()
    {
        //KULLANICI ADI VE ÞÝFRE DOÐRULAMA
        if (girilenKullaniciAdi.text.Length >= 4 && girilenKullaniciAdi.text.Length <= 10 && Regex.IsMatch(girilenKullaniciAdi.text, "^[a-zA-Z0-9]*$") &&
            girilenSifre.text.Length >= 4 && girilenSifre.text.Length <= 10 && Regex.IsMatch(girilenSifre.text, "^[a-zA-Z0-9]*$"))
        {
            girisButon.interactable = true;
        }
        else
        {
            girisButon.interactable = false;
        }
    }


    public void GirisButonu()
    {
        kullaniciAdi = girilenKullaniciAdi.text.ToLower();
        sifre = girilenSifre.text;
        StartCoroutine(Giris());
    }

    public void KayitliGiris()
    {
        if (PlayerPrefs.GetString("kullaniciAdi") != "" && PlayerPrefs.GetString("sifre") != "")
        {
            kullaniciAdi = PlayerPrefs.GetString("kullaniciAdi");
            sifre = PlayerPrefs.GetString("sifre");
            StartCoroutine(Giris());
        }
    }

    //KULLANICININ OYUNA GÝRÝÞ YAPMASINI SAÐLAR
    IEnumerator Giris()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "giris");
        form.AddField("girilenKullaniciAdi", kullaniciAdi);
        form.AddField("girilenSifre", sifre);

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/KullaniciKayitlari.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                olayMetni = "Connection error";
            }
            else
            {
                if (www.downloadHandler.text.Contains("APPROVED"))
                {
                    gecisAnimasyonu.GetComponent<Animator>().Play("1"); //Animasyonu çalýþtýrýr.
                    yield return new WaitForSeconds(0.5f); //Animasyon süresi.
                    PlayerPrefs.SetString("kullaniciAdi", kullaniciAdi);
                    PlayerPrefs.SetString("sifre", sifre);
                    SceneManager.LoadScene(1);
                }
                else
                {
                    PlayerPrefs.SetString("kullaniciAdi", "");
                    PlayerPrefs.SetString("sifre", "");
                    olayMetni = www.downloadHandler.text;
                }
            }

            if (olay != null)
                olay.OlayBaslatici(olayMetni);
        }
    }
}
