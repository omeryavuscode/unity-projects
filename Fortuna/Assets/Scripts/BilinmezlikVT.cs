using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BilinmezlikVT : MonoBehaviour
{
    //OBJELER
    public TextMeshProUGUI bilinmez;
    public GameObject bilinmezAnimasyonu;
    public TextMeshProUGUI sureMetni;

    //BAHÝS
    public TMP_InputField girilenBahis;
    public Button bahisYapmaButonu;
    public Button azalt;
    public Button arttir;
    public TextMeshProUGUI bahisBilgileri;
    public TextMeshProUGUI para;

    //ANÝMASYON
    public Animator gecisAnimasyonu;

    //VERÝLER
    int sureVT;
    int bahisYapilmisMiVT;
    int girilenBahisVT;

    void Start()
    {
        gecisAnimasyonu.Play("2");
        StartCoroutine(BilinmezlikBilgileri());
    }

    IEnumerator BilinmezlikBilgileri()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "bilinmezlikBilgileri");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/Bilinmezlik.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                //Bilinmezlik bilgilerini toplar.
                bilinmez.text = $"----------\n{www.downloadHandler.text.Split('\n')[0]}x\n----------"; //Gelen çarpan.
                sureVT = int.Parse(www.downloadHandler.text.Split('\n')[1]); //Kalan süre.
                sureMetni.text = sureVT.ToString(); //Kalan süreyi ekrana yazar.
                bahisYapilmisMiVT = int.Parse(www.downloadHandler.text.Split('\n')[2]); //Bahis yapilmis mi?

                if (bahisYapilmisMiVT != 0 && www.downloadHandler.text.Split('\n')[3] == "Obscurity") //Öncesinde bahis yapýlmýþ ise
                {
                    girilenBahisVT = int.Parse(www.downloadHandler.text.Split('\n')[4]); //Bahis miktarýný çeker.
                    girilenBahis.text = girilenBahisVT.ToString(); //Bahis girme alanýna bahis miktarýný yazar.

                    bahisBilgileri.text = $"Bet Amount: {girilenBahisVT}";
                }
                else
                {
                    if (bahisYapilmisMiVT != 0)
                        bahisBilgileri.text = $"Bet On Game {www.downloadHandler.text.Split('\n')[3]}";
                    else
                        bahisBilgileri.text = "No Bet";
                }
            }

            BilinmezlikOyunu();
            yield return new WaitForSeconds(1);
            StartCoroutine(BilinmezlikBilgileri());
        }
    }

    void BilinmezlikOyunu()
    {
        SureMetniRengi();

        //Öncesinde bahis yapýlmýþ mý?
        if (bahisYapilmisMiVT != 0 || sureVT <= 5 && sureVT > 0)
        {
            ButonAktifligi(false);
        }
        else
        {
            ButonAktifligi(true);
        }

        if (sureVT <= 5 && sureVT > 0)
        {
            bilinmezAnimasyonu.SetActive(true);
            bilinmez.gameObject.SetActive(false);
        }
        else
        {
            bilinmezAnimasyonu.SetActive(false);
            bilinmez.gameObject.SetActive(true);
        }
    }

    void SureMetniRengi()
    {
        if (sureVT <= 5)
        {
            sureMetni.color = Color.grey;
        }
        else
        {
            sureMetni.color = Color.white;
        }
    }

    void ButonAktifligi(bool aktiflik)
    {
        //Bahis miktarý girme alaný ve bahis yapma butonunun aktifliðini deðiþtirir.
        girilenBahis.interactable = aktiflik;

        if (girilenBahis.text != "" && int.Parse(girilenBahis.text) <= 1)
            azalt.interactable = false;
        else
            azalt.interactable = aktiflik;

        if (girilenBahis.text != "" && int.Parse(girilenBahis.text) >= 999)
            arttir.interactable = false;
        else
            arttir.interactable = aktiflik;

        if (girilenBahis.text != "" && int.Parse(girilenBahis.text) > 0 && int.Parse(para.text.Split(".")[0].Replace(",", "")) >= int.Parse(girilenBahis.text))
            bahisYapmaButonu.interactable = aktiflik;
        else
            bahisYapmaButonu.interactable = false;
    }

    public void Arttir()
    {
        if (girilenBahis.text != "" && int.Parse(girilenBahis.text) < 999)
        {
            girilenBahis.text = (int.Parse(girilenBahis.text) + 1).ToString();
        }
        else
        {
            girilenBahis.text = "1";
        }

        ButonAktifligi(true);
    }

    public void Azalt()
    {
        if (girilenBahis.text != "" && int.Parse(girilenBahis.text) > 1)
        {
            girilenBahis.text = (int.Parse(girilenBahis.text) - 1).ToString();
        }
        else
        {
            girilenBahis.text = "1";
        }

        ButonAktifligi(true);
    }

    public void BahisButonu()
    {
        ButonAktifligi(false);

        if (int.Parse(girilenBahis.text) > 0 && bahisYapilmisMiVT == 0 && int.Parse(para.text.Split(".")[0].Replace(",", "")) >= int.Parse(girilenBahis.text))
            StartCoroutine(Bahis());
    }

    IEnumerator Bahis()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "bilinmezlikBahisleri");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));
        form.AddField("sifre", PlayerPrefs.GetString("sifre"));
        form.AddField("bahisMiktari", girilenBahis.text);

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/Bilinmezlik.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void GeriButonu()
    {
        StartCoroutine(Geri());
    }

    IEnumerator Geri()
    {
        gecisAnimasyonu.Play("1");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
