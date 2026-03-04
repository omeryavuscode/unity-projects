using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BilgiPaneliVT : MonoBehaviour
{
    //Kullanýcý Bilgileri
    public TextMeshProUGUI kullanici;
    public TextMeshProUGUI kullanicilar;
    public TextMeshProUGUI para;

    //Animasyon
    public Animator paraAnimasyonu;

    //Sesler
    public AudioSource paraSesi;
    public AudioClip kazanmaSesi;
    public AudioClip kaybetmeSesi;


    void Start()
    {
        StartCoroutine(AktifKullanici());
        StartCoroutine(Para());
        StartCoroutine(sistemKontrol());
    }

    //PARAYI SÜREKLÝ KONTROL EDER VE ÝNTERNET BAÐLANTISI GÝTTÝÐÝ ZAMAN ANA EKRANA GERÝ DÖNER
    IEnumerator Para()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "paraMiktari");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));
        form.AddField("sifre", PlayerPrefs.GetString("sifre"));

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/BilgiPaneli.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                string tamKisim = www.downloadHandler.text.Substring(0, www.downloadHandler.text.IndexOf("."));
                string kurusKisim = www.downloadHandler.text.Substring(www.downloadHandler.text.IndexOf(".") + 1);
                tamKisim = String.Format("{0:n0}", int.Parse(tamKisim));
                tamKisim = tamKisim.Replace(".", ",");
                para.text = $"{tamKisim}.{kurusKisim}";

                if (PlayerPrefs.GetInt("para") != int.Parse(tamKisim.Replace(",", "")))
                {
                    if (PlayerPrefs.GetInt("para") < int.Parse(tamKisim.Replace(",", ""))) //PARA KAZANMA
                        paraSesi.clip = kazanmaSesi;

                    else if (PlayerPrefs.GetInt("para") > int.Parse(tamKisim.Replace(",", ""))) //PARA KAYBETME      
                        paraSesi.clip = kaybetmeSesi;


                    PlayerPrefs.SetInt("para", int.Parse(tamKisim.Replace(",", ""))); //Paranýn tam kýsmýnýn verisini günceller.

                    paraSesi.volume = PlayerPrefs.GetFloat("efekt") / 100f; //Para sesini efekt ses düzeyi olarak ayarlar.
                    paraSesi.Play();

                    //Para animasyonu
                    paraAnimasyonu.enabled = true; //Para güncellenme animasyonu çalýþýr.
                    yield return new WaitForSeconds(3);
                    paraAnimasyonu.enabled = false; //Para güncellenme animasyonu durdurulur.
                    para.color = Color.white;
                }
                else
                    yield return new WaitForSeconds(1);

                StartCoroutine(Para());
            }
        }
    }


    //AKTÝF OYUNCU SAYISINI SÜREKLÝ KONTROL EDER VE ÝNTERNET BAÐLANTISI GÝTTÝÐÝ ZAMAN ANA EKRANA GERÝ DÖNER
    IEnumerator AktifKullanici()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "aktifKisiSayisi");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));
        form.AddField("sifre", PlayerPrefs.GetString("sifre"));

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/BilgiPaneli.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                if (kullanici != null)
                    kullanici.text = PlayerPrefs.GetString("kullaniciAdi").ToLower();

                if (kullanicilar != null)
                    kullanicilar.text = String.Format("{0:n0}", double.Parse(www.downloadHandler.text));

                yield return new WaitForSeconds(0.75f);
                StartCoroutine(AktifKullanici());
            }
        }
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
                SceneManager.LoadScene(0);
            }
            else
            {
                yield return new WaitForSeconds(0.5f); //Baþlangýçtaki animasyonu görmek için süre.
                if (www.downloadHandler.text.Split('\n')[1] != Application.version || www.downloadHandler.text.Split('\n')[0] == "0")
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    yield return new WaitForSeconds(10);
                    StartCoroutine(sistemKontrol());
                }
            }
        }
    }
}
