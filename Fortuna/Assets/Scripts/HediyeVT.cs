using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HediyeVT : MonoBehaviour
{
    //OBJELER
    public TextMeshProUGUI sureMetni; //Sürenin yazýldýðý deðiþken.
    public Button[] hediyeler; //Ekranda ki hediye kutularý.
    public Sprite seciliHediye; //Secili hediye kostümü.
    string seciliHediyeNo; //Seçili hediyenin numarasý.

    //BAHÝS
    public TextMeshProUGUI bahisBilgileri;
    public TMP_InputField girilenBahis;
    public Button azalt;
    public Button arttir;
    public Button bahisYapmaButonu;
    public TextMeshProUGUI para;

    //ANÝMASYON
    public Animator gecisAnimasyonu; //Sahne geçiþ animasyonu.

    //VERÝLER
    string rastgeleHediyeVT;
    int sureVT;
    int bahisYapilmisMiVT;
    int girilenBahisVT;
    int seciliHediyeNoVT;

    // Start is called before the first frame update
    void Start()
    {
        gecisAnimasyonu.Play("2");
        StartCoroutine(HediyeBilgileri());
    }

    IEnumerator HediyeBilgileri()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "hediyeBilgileri");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/Hediye.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                //Hediye bilgilerini toplar.
                rastgeleHediyeVT = www.downloadHandler.text.Split('\n')[0]; //Rastgele hediyeler.
                sureVT = int.Parse(www.downloadHandler.text.Split('\n')[1]); //Kalan süre.
                sureMetni.text = sureVT.ToString(); //Kalan süreyi ekrana yazar.
                bahisYapilmisMiVT = int.Parse(www.downloadHandler.text.Split('\n')[2]); //Bahis yapilmis mi?

                if (bahisYapilmisMiVT != 0 && www.downloadHandler.text.Split('\n')[3] == "Gift") //Öncesinde hediye oyununa bahis yapýlmýþ is
                {
                    girilenBahisVT = int.Parse(www.downloadHandler.text.Split('\n')[4]); //Girilmiþ olan bahis miktarini çeker.
                    seciliHediyeNoVT = int.Parse(www.downloadHandler.text.Split('\n')[5]); //Seçilmiþ olan hediye numarasýný çeker.

                    girilenBahis.text = girilenBahisVT.ToString(); //Bahis girme alanýna bahis miktarýný yazar.
                    hediyeler[seciliHediyeNoVT - 1].image.sprite = seciliHediye; //Seçilen hediyeyi ekranda gösterir.

                    bahisBilgileri.text = $"Bet Amount: {girilenBahisVT} - Gift: {seciliHediyeNoVT}"; //Bahis bilgilerini ekrana yazar.
                }
                else
                {
                    seciliHediyeNoVT = -1;

                    if (bahisYapilmisMiVT != 0)
                        bahisBilgileri.text = $"Bet On Game {www.downloadHandler.text.Split('\n')[3]}";
                    else
                        bahisBilgileri.text = "No Bet";
                }
            }

            HediyeOyunu();
            yield return new WaitForSeconds(1);
            StartCoroutine(HediyeBilgileri());
        }
    }

    void HediyeOyunu()
    {
        SureMetniRengi();

        //Öncesinde bahis yapýlmýþsa veya süre 5 saniyeden az kalmýþsa.
        if (bahisYapilmisMiVT != 0 || sureVT <= 5 && sureVT > 0)
        {
            HediyeleriKarartma();
            ButonAktifligi(false);
        }
        else
        {
            //Seçili hediyeyi kostümlerine bakarak deðiþkene aktarýr.
            foreach (Button h in hediyeler)
            {
                if (h.image.sprite.name.StartsWith('s'))
                {
                    seciliHediyeNo = h.name[8].ToString();
                }
            }

            KazananHediyeler();
            ButonAktifligi(true);
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
        foreach (Button h in hediyeler)
        {
            h.interactable = aktiflik;
        }

        //Bahis miktarý girme alaný ve bahis yapma butonunun aktifliðini deðiþtirir.
        girilenBahis.interactable = aktiflik;

        //Arttýrma ve azaltma butonlarýnýn aktifliðini girilen bahis sýnýrlarýný aþmaycaak þekilde aç kapa.
        if (girilenBahis.text != "" && int.Parse(girilenBahis.text) <= 1)
            azalt.interactable = false;
        else
            azalt.interactable = aktiflik;

        if (girilenBahis.text != "" && int.Parse(girilenBahis.text) >= 999)
            arttir.interactable = false;
        else
            arttir.interactable = aktiflik;

        //Bahis yapma butonunun aktifliðini eksiksiz deðer yoksa aç yoksa kapat.
        if (girilenBahis.text != "" && int.Parse(girilenBahis.text) > 0 && int.Parse(para.text.Split(".")[0].Replace(",", "")) >= int.Parse(girilenBahis.text) && seciliHediyeNo != null)
            bahisYapmaButonu.interactable = aktiflik;
        else
            bahisYapmaButonu.interactable = false;
    }

    void HediyeleriKarartma()
    {
        foreach (Button h in hediyeler) //Hediyeler arasýnda dolaþýr.
        {
            //Seçilen hediye numarasý haricindekileri karartýr.
            if (h.name[8].ToString() == seciliHediyeNoVT.ToString())
                h.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            else
                h.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        }
    }

    void KazananHediyeler()
    {
        foreach (Button h in hediyeler)  //Hediyeler arasýnda dolaþýr.
        {
            //Rastgele hediye numarasý haricindekileri karartýr.
            if (h.name[8].ToString() != rastgeleHediyeVT[0].ToString() && h.name[8].ToString() != rastgeleHediyeVT[1].ToString() && h.name[8].ToString() != rastgeleHediyeVT[2].ToString())
                h.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
            else
                h.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        }
    }

    public void BahisButonu()
    {
        ButonAktifligi(false);

        //Seçili hediyeyi kostümlerine bakarak deðiþkene aktarýr.
        foreach (Button h in hediyeler)
        {
            if (h.image.sprite.name.StartsWith('s'))
            {
                seciliHediyeNo = h.name[8].ToString();
            }
        }

        if (girilenBahis.text != "" && int.Parse(girilenBahis.text) > 0 && int.Parse(para.text.Split(".")[0].Replace(",", "")) >= int.Parse(girilenBahis.text) && seciliHediyeNo != null)
            StartCoroutine(Bahis());
    }

    IEnumerator Bahis()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "hediyeBahisleri");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));
        form.AddField("sifre", PlayerPrefs.GetString("sifre"));
        form.AddField("bahisMiktari", girilenBahis.text);
        form.AddField("secilenHediye", seciliHediyeNo);

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/Hediye.php", form))
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
