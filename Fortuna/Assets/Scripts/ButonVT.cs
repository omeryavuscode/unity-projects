using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButonVT : MonoBehaviour
{
    //OBJELER
    public TextMeshProUGUI sureMetni; //Sürenin yazýldýðý deðiþken.
    public Button buton; //Oys kazandýran buton.

    //ANÝMASYON
    public Animator gecisAnimasyonu; //Sahne geçiþ animasyonu.

    //VERÝLER
    int sureVT; //Veri tabanýndan alýnan süre verisi.

    private void Start()
    {
        gecisAnimasyonu.Play("2"); //Sahne giriþinde animasyon çaðýrýr.
        StartCoroutine(ButonBilgileri()); //Buton bilgileri çaðýrýlýr.
    }

    //Buton oyunu süresini ve kullanýcýnýn butonun aktif olup olmadýðýnýn verisini çeker.
    IEnumerator ButonBilgileri()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "butonBilgileri");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/Buton.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                //Kalan süre verisini çeker ve ekrana yazdýrýr.
                sureVT = int.Parse(www.downloadHandler.text.Split('\n')[0]); //Kalan süreyi çeker.
                sureMetni.text = sureVT.ToString(); //Kalan süreyi ekrana yazar.

                //Buton aktifliði verisini çeker ve ekranda belirtir.
                int aktiflik = int.Parse(www.downloadHandler.text.Split('\n')[1]); 

                if (aktiflik == 1)
                    buton.interactable = true;
                else
                    buton.interactable = false;
            }

            yield return new WaitForSeconds(1);
            StartCoroutine(ButonBilgileri());
        }
    }

    public void ButonButonu()
    {
        buton.interactable = false;
        StartCoroutine(ButonIslevi());
    }

    //Butona basýldýðýnda sisteme gönderir.
    IEnumerator ButonIslevi()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "buton");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/Buton.php", form))
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
        StartCoroutine(geri());
    }

    IEnumerator geri()
    {
        gecisAnimasyonu.Play("1");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
