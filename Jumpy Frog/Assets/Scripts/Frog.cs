using System.Collections;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Frog : MonoBehaviour
{
    //KODLAR
    OyunYoneticisi oyunYoneticisi;

    //FROG
    int frog_konumu = 1;
    public float frog_hizi;
    public Animator frog_animator;
    bool frog_olduMu = false;

    //DOKUNMA
    Touch dokunma;
    bool dokunma_Izni = true;

    void Start()
    {
        oyunYoneticisi = GameObject.FindGameObjectWithTag("OyunYoneticisi").GetComponent<OyunYoneticisi>(); //Oyun yöneticisi kod sayfasýný deðiþkene aktar.
    }

    void Update()
    {
        if (!frog_olduMu && oyunYoneticisi.oyunBasladiMi) //Frog ölmediyse ve oyun baþladýysa kontroller çalýþýr.
        {
            HareketVeKontrol();
            ZeminKontrol();
        }
    }

    void HareketVeKontrol()
    {
        if (Input.touchCount > 0)
        {
            dokunma = Input.GetTouch(0);

            if (dokunma_Izni) //Extra dokunmalarý engellemek için kontrol ettirir.
            {
                dokunma_Izni = false;

                if (dokunma.position.y < Screen.height / 2) //Ekranýn altýna mý dokundu.
                {
                    oyunYoneticisi.dokunmaOlduMu = true; //Dokunma olduðuna dair bilgiyi deðiþkene taþý.

                    if (dokunma.position.x < Screen.width / 2) //Ekranýn saðýna mý soluna mý dokundu.
                    {
                        transform.position = oyunYoneticisi.yapraklar[frog_konumu].transform.position; //Frog'u sola gönder.
                    }
                    else
                    {
                        transform.position = oyunYoneticisi.yapraklar[frog_konumu + 1].transform.position; //Frog'u saða gönder.
                    }

                    if (dokunma.position.x < Screen.width / 2 && oyunYoneticisi.dusmanlar[frog_konumu] == true) //Frog sola gittiyse ve solda düþman varsa öldür.
                    {
                        StartCoroutine(FrogOlum()); //Frog öldü.
                    }
                    else if (dokunma.position.x > Screen.width / 2 && oyunYoneticisi.dusmanlar[frog_konumu + 1] == true) //Frog saða gittiyse ve saðda düþman varsa öldür.
                    {
                        StartCoroutine(FrogOlum()); //Frog öldü.
                    }

                    oyunYoneticisi.YaprakYerlestirme(); //Frog adým attðý için yeni yaprak yerleþtirilir.

                    oyunYoneticisi.yaprak_KaldirilacakId = frog_konumu; //Üstüne geldiði yapraðýn yok olmasýný baþlatmak için yok olacak yaprak konumuna frog konumu gönderip kodu çalýþtýrma.
                    StartCoroutine(oyunYoneticisi.YaprakKaldirma()); //Yaprak yok etme fonksiyonunu yeni konumla çalýþtýrma.

                    oyunYoneticisi.skor.text = (int.Parse(oyunYoneticisi.skor.text) + 1).ToString(); //Hareket ettiði için skoru güncelleme.

                    if (int.Parse(oyunYoneticisi.skor.text) > PlayerPrefs.GetInt("enIyiSkor")) //Eðer en yüksek skor geçildiyse yeni en yüksek skoru güncelle ve metni yenile.
                    {
                        PlayerPrefs.SetInt("enIyiSkor", int.Parse(oyunYoneticisi.skor.text));
                        oyunYoneticisi.enIyiSkor.text = $"Best Score: {PlayerPrefs.GetInt("enIyiSkor")}";
                    }

                    frog_konumu += 2; //Frogun bir sonraki sefere geçeceði konumu güncelleme.
                }
            }

            if (dokunma.phase == TouchPhase.Ended) //Dokunmanýn bittiðini ve yeni bir dokunma gelmesi için dokunma izni verilmesini saðlar.
            {
                dokunma_Izni = true;
            }
        }
    }

    void ZeminKontrol()
    {
        if (frog_konumu > 1 && !oyunYoneticisi.yapraklar[frog_konumu - 2].IsDestroyed() == false) //Frog ilk yaprakta deðilse ve altýnda olduðu yaprak yok olduysa Frog'u öldür.
        {
            StartCoroutine(FrogOlum());
        }
    }

    IEnumerator FrogOlum()
    {
        frog_olduMu = true; //Frog öldü deðiþkenini güncelle.
        frog_animator.Play("Frog_Olum"); //Frog ölüm animasyonunu çalýþtýr.
        gameObject.GetComponent<AudioSource>().Play(); //Ölüm sesi çalýnýr.
        yield return new WaitForSeconds(2); //2 saniye bekle.
        SceneManager.LoadScene(0); //Ayný sahneyi tekrar çalýþtýr.
    }
}
