using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSahneGecis : MonoBehaviour
{
    //Paneller
    public Image KullanicilarPaneli;
    public Image SifreDegistirmePaneli;
    public Image ParaCekmePaneli;
    public Image IslemlerPaneli;
    public Image AyarlarPaneli;
    public Image UyariPanel;

    //Butonlar
    public Image KullanicilarButon; public TextMeshProUGUI KullanicilarMetni;
    public Image KullaniciButon; public TextMeshProUGUI KullaniciMetni;
    public Image ParaCekmeButon; public TextMeshProUGUI ParaMetni;
    public Button[] OyunButonlarý;

    //Sprite
    public Sprite kullanicilar1;
    public Sprite kullanicilar2;
    public Sprite kullanici1;
    public Sprite kullanici2;
    public Sprite paraCekme1;
    public Sprite paraCekme2;

    //Kullanicilar
    public GameObject KullaniciAdlari;
    public GameObject IslemBilgileri;

    //Scripts
    public KurVT kur;

    //Animasyon
    public GameObject gecisAnimasyonu;

    private void Start()
    {
        gecisAnimasyonu.GetComponent<Animator>().Play("2");

        if (!PlayerPrefs.HasKey("Uyari"))
        {
            PlayerPrefs.SetInt("Uyari", 1);
            UyariPanel.gameObject.SetActive(true);
        }

        StartCoroutine(OyunButonuAktifligi());
    }

    //AKTIFKULLANICILAR PANELÝNÝ AÇAR VEYA KAPATIR BUTON GÖRÜNÜMÜNÜ DEÐÝÞTÝRÝR VE KULLANICILARI ÇAÐIRIR
    public void Kullanicilar()
    {
        if (KullanicilarPaneli.gameObject.activeSelf)
        {
            HerSeyiKapat();
        }
        else
        {
            HerSeyiKapat();
            KullanicilarPaneli.gameObject.SetActive(true);
            StartCoroutine(KullaniciAdlari.GetComponent<KullanicilarVT>().Kullanicilar());
            KullanicilarButon.sprite = kullanicilar2;
            KullanicilarMetni.color = Color.yellow;
        }
    }

    //KULLANICI PANELÝNÝ AÇAR VEYA KAPATIR BUTON GÖRÜNÜMÜNÜ DEÐÝÞTÝRÝR
    public void Kullanici()
    {
        if (SifreDegistirmePaneli.gameObject.activeSelf)
        {
            HerSeyiKapat();
        }
        else
        {
            HerSeyiKapat();
            SifreDegistirmePaneli.gameObject.SetActive(true);
            KullaniciButon.sprite = kullanici2;
            KullaniciMetni.color = Color.yellow;
        }
    }

    //PARA ÇEKME PANELÝNÝ AÇAR VEYA KAPATIR BUTON GÖRÜNÜMÜNÜ DEÐÝÞTÝRÝR
    public void ParaCekme()
    {
        if (ParaCekmePaneli.gameObject.activeSelf)
        {
            HerSeyiKapat();
        }
        else
        {
            HerSeyiKapat();
            ParaCekmePaneli.gameObject.SetActive(true);
            ParaCekmeButon.sprite = paraCekme2;
            ParaMetni.color = Color.yellow;
            kur.KurCalistir();
        }
    }

    //ÝÞLEMLER PANELÝNÝ AÇAR VEYA KAPATIR BUTON GÖRÜNÜMÜNÜ DEÐÝÞTÝRÝR
    public void Islemler()
    {
        if (IslemlerPaneli.gameObject.activeSelf)
        {
            HerSeyiKapat();
        }
        else
        {
            HerSeyiKapat();
            IslemlerPaneli.gameObject.SetActive(true);
            IslemBilgileri.GetComponent<GecmisParaCekmeIslemleriVT>().Start();
        }
    }

    //AYARLAR PANELÝNÝ AÇAR VEYA KAPATIR BUTON GÖRÜNÜMÜNÜ DEÐÝÞTÝRÝR
    public void Ayarlar()
    {
        if (AyarlarPaneli.gameObject.activeSelf)
        {
            HerSeyiKapat();
        }
        else
        {
            HerSeyiKapat();
            AyarlarPaneli.gameObject.SetActive(true);
            AyarlarPaneli.GetComponent<Ayarlar>().Start();
        }
    }

    //TÜM PANELLERÝ KAPATIR
    void HerSeyiKapat()
    {
        foreach (Transform child in KullaniciAdlari.transform) //Önceki isimleri siler.
        {
            Destroy(child.gameObject);
        }

        KullanicilarPaneli.gameObject.SetActive(false);
        SifreDegistirmePaneli.gameObject.SetActive(false);
        ParaCekmePaneli.gameObject.SetActive(false);
        IslemlerPaneli.gameObject.SetActive(false);
        AyarlarPaneli.gameObject.SetActive(false);

        KullaniciMetni.color = Color.white;
        KullanicilarMetni.color = Color.white;
        ParaMetni.color = Color.white;

        KullanicilarButon.sprite = kullanicilar1;
        KullaniciButon.sprite = kullanici1;
        ParaCekmeButon.sprite = paraCekme1;
    }

    //KULLANICI HESABINDAN ÇIKIÞ YAPAR
    public void CikisYapButon()
    {
        PlayerPrefs.SetString("kullaniciAdi", "");
        PlayerPrefs.SetString("sifre", "");
        StartCoroutine(CikisYap());
    }

    IEnumerator CikisYap()
    {
        gecisAnimasyonu.GetComponent<Animator>().Play("1");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }

    IEnumerator OyunButonuAktifligi()
    {
        for (int i = 1; i < OyunButonlarý.Length; i++)
        {
            if (ParaMetni.text.Split('.')[0] == "0")
            {
                OyunButonlarý[i].interactable = false;
                OyunButonlarý[i].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            }
            else
            {
                OyunButonlarý[i].interactable = true;
                OyunButonlarý[i].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            }
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(OyunButonuAktifligi());
    }
}
