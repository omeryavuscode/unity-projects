using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZarVT : MonoBehaviour
{
    //OBJELER
    public TextMeshProUGUI sureMetni;
    public Image rastgeleZar;
    public Sprite[] rastgeleZarGorunusleri;
    public Button[] zarlar;
    string seciliZarNo;

    //BAHÝS
    public TMP_InputField girilenBahis;
    public Button bahisYapmaButonu;
    public Button azalt;
    public Button arttir;
    public TextMeshProUGUI bahisBilgileri;
    public TextMeshProUGUI para;

    //ANÝMASYON
    public Animator zarAnimasyonu;
    public Animator gecisAnimasyonu;

    //VERÝLER
    int rastgeleZarVT = 1;
    int sureVT;
    int bahisYapilmisMiVT;
    int girilenBahisVT;
    int seciliZarNoVT;

    private void Start()
    {
        gecisAnimasyonu.Play("2");
        StartCoroutine(ZarBilgileri());
    }

    IEnumerator ZarBilgileri()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "zarBilgileri");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/Zar.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                //Zar bilgilerini toplar.
                rastgeleZarVT = int.Parse(www.downloadHandler.text.Split('\n')[0]); //Gelen zar.
                sureVT = int.Parse(www.downloadHandler.text.Split('\n')[1]); //Kalan süre Verisi.
                sureMetni.text = sureVT.ToString(); //Kalan süreyi ekrana yazar.
                bahisYapilmisMiVT = int.Parse(www.downloadHandler.text.Split('\n')[2]); //Bahis yapilmis mi?

                if (bahisYapilmisMiVT != 0 && www.downloadHandler.text.Split('\n')[3] == "Dice") //Öncesinde bahis yapýlmýþ ise
                {
                    girilenBahisVT = int.Parse(www.downloadHandler.text.Split('\n')[4]); //Bahis miktarýný çeker.
                    seciliZarNoVT = int.Parse(www.downloadHandler.text.Split('\n')[5]); //Bahse girilen zarý çeker.

                    girilenBahis.text = girilenBahisVT.ToString(); //Bahis girme alanýna bahis miktarýný yazar.
                    zarlar[seciliZarNoVT - 1].image.sprite = rastgeleZarGorunusleri[seciliZarNoVT - 1]; //Bahse girilen zarýn kostümünü ayarlar.

                    bahisBilgileri.text = $"Bet Amount: {girilenBahisVT} - Dice: {seciliZarNoVT}";
                }
                else
                {
                    if (bahisYapilmisMiVT != 0)
                        bahisBilgileri.text = $"Bet On Game {www.downloadHandler.text.Split('\n')[3]}";
                    else
                        bahisBilgileri.text = "No Bet";
                }
            }

            ZarOyunu();
            yield return new WaitForSeconds(1);
            StartCoroutine(ZarBilgileri());
        }
    }

    void ZarOyunu()
    {
        SureMetniRengi();

        //Öncesinde bahis yapýlmýþ mý?
        if (bahisYapilmisMiVT != 0 || sureVT <= 5 && sureVT > 0)
        {
            ButonAktifligi(false);
        }
        else
        {

            //Seçilen zarý kostümlerine bakarak deðiþkene aktarýr.
            foreach (Button z in zarlar)
            {
                if (z.image.sprite.name.StartsWith('s'))
                {
                    seciliZarNo = z.image.sprite.name[4].ToString();
                }
            }

            ButonAktifligi(true);
        }

        if (sureVT <= 5 && sureVT > 0)
        {
            zarAnimasyonu.enabled = true;
        }
        else
        {
            rastgeleZar.sprite = rastgeleZarGorunusleri[rastgeleZarVT - 1];
            zarAnimasyonu.enabled = false;
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
        //Tüm zar butonlarýnýn aktifliðini deðiþtirir.
        foreach (Button z in zarlar)
        {
            z.interactable = aktiflik;
        }

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

        if (girilenBahis.text != "" && int.Parse(girilenBahis.text) > 0 && int.Parse(para.text.Split(".")[0].Replace(",", "")) >= int.Parse(girilenBahis.text) && seciliZarNo != null)
            bahisYapmaButonu.interactable = aktiflik;
        else
            bahisYapmaButonu.interactable = false;
    }

    public void BahisButonu()
    {
        ButonAktifligi(false);

        //Seçilen zarý kostümlerine bakarak deðiþkene aktarýr.
        foreach (Button z in zarlar)
        {
            if (z.image.sprite.name.StartsWith('s'))
            {
                seciliZarNo = z.image.sprite.name[4].ToString();
            }
        }

        if (seciliZarNo != null && int.Parse(girilenBahis.text) > 0 && bahisYapilmisMiVT == 0 && int.Parse(para.text.Split(".")[0].Replace(",", "")) >= int.Parse(girilenBahis.text))
            StartCoroutine(Bahis());
    }

    IEnumerator Bahis()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "zarBahisleri");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));
        form.AddField("sifre", PlayerPrefs.GetString("sifre"));
        form.AddField("bahisMiktari", girilenBahis.text);
        form.AddField("secilenZar", seciliZarNo);

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/Zar.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
        }
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
