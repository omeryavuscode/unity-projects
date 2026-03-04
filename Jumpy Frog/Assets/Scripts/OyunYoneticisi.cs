using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OyunYoneticisi : MonoBehaviour
{
    //FROG PREFAB
    public GameObject frogPrefab;
    GameObject frog;
    Animator frog_Animator;

    TextMeshProUGUI frogMetni;
    public float frogMetni_YazmaSuresi;
    public float frogMetni_BeklemeSuresi;

    //YAPRAK PREFAB
    public GameObject yaprakPrefab;
    public int yaprak_BaslangicSayisi;
    int yaprak_Sayisi = 1;
    public int yaprak_KaldirilacakId = 1;
    float yaprak_KaldirmaSuresi = 0.025f;
    public float yaprak_OlusmaSuresi;
    public List<GameObject> yapraklar = new List<GameObject>();
    public List<bool> dusmanlar = new List<bool>();
    public Sprite[] yaprak_Kostumler;

    //SINEK PREFAB
    public GameObject sinekPrefab;

    //SESLER
    public AudioSource sesCalar;
    public AudioClip yaprak_EklemeSesi;
    public AudioClip frogMetni_YazmaSesi;

    //GÖSTERGELER
    public Image solGosterge;
    public Image sagGosterge;
    public Color gostergeRengi;
    public TextMeshProUGUI skor;
    public TextMeshProUGUI enIyiSkor;
    public Button menuButonu;

    //KODLAR
    public Kamera kamera;

    //OYUN
    public bool oyunBasladiMi = false;
    public bool dokunmaOlduMu = false;

    void Start()
    {
        StartCoroutine(Baslangic()); //Oyunu kurar baþlatýr.
    }

    IEnumerator Baslangic()
    {
        //Baþlangýçtaki ilk yapraðý oluþturur ve frog'u üzerine koyar.
        GameObject yaprak = Instantiate(yaprakPrefab, new Vector3(0, -2, 0), Quaternion.identity); //Yaprak oluþur.
        yapraklar.Add(yaprak); //Yaprak listeye eklendi.
        dusmanlar.Add(false); //Ýlk yapraðýn düþman olmadýðý belirtildi.
        YaprakEklemeCal();  //Yaprak ekleme sesi çalýndý.
        yaprak.GetComponent<SpriteRenderer>().sortingOrder = 0; //Frog'un altýnda olacak þekilde ayarlanýr.

        yield return new WaitForSeconds(yaprak_OlusmaSuresi); //Belirtilen süre beklenir.

        frog = Instantiate(frogPrefab, new Vector3(0, -2, 0), Quaternion.identity); //Frog'oluþur.
        YaprakEklemeCal();
        frog.GetComponent<SpriteRenderer>().sortingOrder = 1; //Yapraðýn üstünde olacak þekilde ayarlanýr.

        kamera.kamera_Hedef = frog; //Kameranýn takip edeceði obje Frog olarak belirlendi.

        //Baþlangýçta oyunda gözükecek sýrada yaprak oluþturur.
        for (int i = 0; i < yaprak_BaslangicSayisi; i++)
        {
            yield return new WaitForSeconds(yaprak_OlusmaSuresi); //Her sýra için beklenecek olan belirtilen süre.

            YaprakYerlestirme(); //Her beklemede 2 adet yaprak oluþturur.
        }


        if (!PlayerPrefs.HasKey("ilkOyun")) //Oyun ilk kez açýldýysa konuþma metnini gösterir.
        {
            yield return new WaitForSeconds(frogMetni_BeklemeSuresi); //Yapraklar koyulduktan belirlenen süre kadar bekle.

            frogMetni = GameObject.FindGameObjectWithTag("FrogMetni").GetComponent<TextMeshProUGUI>(); //Frog metin objesinin atamasýný yap.
            StartCoroutine(KonusmaBalonu("Hey!\nI'm Frog.\nHow are you?\nI hope you are fine.\nI need a leader to show me the right path for nutrition.")); //Konuþma metnini yazdýr.

        }
        else
        {
            StartCoroutine(Gosterge()); //Ekrana basmasýný gösteren fonksiyonu çaðýr.
        }
    }

    public void YaprakYerlestirme()
    {
        int dusmanKonumu = Random.Range(0, 2); //Rastgele düþman konumu belirlendi.

        for (int i = yaprak_Sayisi; i < yaprak_Sayisi + 2; i++) //2 adet yeni yaprak ekleyen döngü oluþturur.
        {
            GameObject yaprak = Instantiate(yaprakPrefab, new Vector3(i % 2 == 1 ? -1 : 1, i % 2 == 1 ? i - 1 : i - 2, 0), Quaternion.identity); //Yerleþecek yapraðýn konumu girilir.

            if (i % 2 == dusmanKonumu)
            {
                yaprak.GetComponent<SpriteRenderer>().sprite = yaprak_Kostumler[1];
                dusmanlar.Add(true); //Düþmanýn konumu listeye atandý.
            }
            else
            {
                dusmanlar.Add(false); //Düþman olmadýðý listeye atandý.
            }

            YaprakEklemeCal(); //Yaprak ekleme sesi çalýnýr.
            yaprak.GetComponent<SpriteRenderer>().sortingOrder = 0; //Yaprak görünüm sýrasýnda alta alýndý.
            yapraklar.Add(yaprak); //Yaprak listeye eklendi.
        }

        yaprak_Sayisi += 2; //Yaprak sayýsý 2 eklendiði belirtilir.
    }

    public IEnumerator YaprakKaldirma()
    {
        for (int i = 0; i < yaprak_KaldirilacakId - 10; i++) //Eðer son koyulan 10 yaprakdan fazla id numarasýna sahipse yaprak kaldýrýlýr.
        {
            if (yapraklar[yaprak_KaldirilacakId].IsDestroyed() == false)
            {
                Destroy(yapraklar[i]);
            }
        }

        int kaldirilacakId = yaprak_KaldirilacakId; //Kaldýrýlacak yaprak id si kontrol edilmek üzere baþka deðiþkene taþýnýr.

        for (float i = 1; i > 0 && kaldirilacakId == yaprak_KaldirilacakId; i -= 0.01f)
        {
            yapraklar[kaldirilacakId].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
            yapraklar[kaldirilacakId + 1].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(yaprak_KaldirmaSuresi);
        }

        if (kaldirilacakId == yaprak_KaldirilacakId)
        {
            Destroy(yapraklar[kaldirilacakId]);
            Destroy(yapraklar[kaldirilacakId + 1]);
        }
    }

    void YaprakEklemeCal()
    {
        sesCalar.clip = yaprak_EklemeSesi;
        sesCalar.Play();
    }

    void FrogMentiYazmaCal()
    {
        sesCalar.clip = frogMetni_YazmaSesi;
        sesCalar.Play();
    }

    IEnumerator KonusmaBalonu(string metin)
    {
        frog_Animator = GameObject.FindGameObjectWithTag("Frog").GetComponent<Animator>();

        frog_Animator.Play("Frog_Konusma");

        if (frogMetni.text == "")
        {
            frogMetni.text += metin[0];

            for (int i = 1; i < metin.Length; i++)
            {
                frogMetni.text += metin[i];

                if (metin[i - 1] == '.' || metin[i - 1] == ',' || metin[i - 1] == '!' || metin[i - 1] == '?')
                {
                    yield return new WaitForSeconds(frogMetni_BeklemeSuresi);
                    frogMetni.text = "";
                }
                else
                    yield return new WaitForSeconds(frogMetni_YazmaSuresi);
            }

            yield return new WaitForSeconds(frogMetni_BeklemeSuresi);
            frogMetni.text = "";
        }

        frog_Animator.Play("Frog_Bosta");

        PlayerPrefs.SetInt("ilkOyun", 1);

        StartCoroutine(Gosterge());
    }

    IEnumerator Gosterge()
    {
        oyunBasladiMi = true;

        skor.text = "0"; //Skoru 0'dan baþlat;
        enIyiSkor.text = $"Best Score: {PlayerPrefs.GetInt("enIyiSkor")}"; //En iyi skor verisini dosyadan çeker.

        for (int i = 0; !dokunmaOlduMu; i++)
        {
            if (i % 2 == 0)
            {
                solGosterge.color = gostergeRengi;
                sagGosterge.color = Color.clear;
            }
            else
            {

                sagGosterge.color = gostergeRengi;
                solGosterge.color = Color.clear;
            }

            yield return new WaitForSeconds(0.5f);
        }

        sagGosterge.color = Color.clear;
        solGosterge.color = Color.clear;
    }
}
