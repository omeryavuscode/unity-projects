using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SifreGuncelleVT : MonoBehaviour
{
    //Girdiler
    public TMP_InputField eskiSifre;
    public TMP_InputField yeniSifre;
    public TMP_InputField yeniSifreTekrar;

    //Güncelleme Butonu
    public Button guncelleButonu;

    //Olay
    Olay olay;

    private void Start()
    {
        olay = FindAnyObjectByType<Olay>();
    }

    private void Update()
    {
        //SIFRE FORMATI DOÐRU ÝSE BUTONU AKTÝF HALE GETÝRÝR
        if (eskiSifre.text.Length >= 4 && eskiSifre.text.Length <= 10 && Regex.IsMatch(eskiSifre.text, "^[a-zA-Z0-9]*$") &&
            yeniSifre.text.Length >= 4 && yeniSifre.text.Length <= 10 && Regex.IsMatch(yeniSifre.text, "^[a-zA-Z0-9]*$") &&
            yeniSifre.text == yeniSifreTekrar.text && eskiSifre.text != yeniSifre.text)
        {
            guncelleButonu.interactable = true;
        }
        else
        {
            guncelleButonu.interactable = false;
        }
    }

    //GÜNCELLEME BUTONUNA BASILIRSA
    public void SifreGuncelleButonu()
    {
        StartCoroutine(SifreGuncelle());
    }

    //ESKÝ VE YENÝ SÝFRE BÝLGÝLERÝNÝ ALARAK KULLANICI ADINA AÝT ÞÝFREYÝ DEÐÝÞTÝRÝR
    IEnumerator SifreGuncelle()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "sifreGuncelle");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));
        form.AddField("eskiSifre", eskiSifre.text);
        form.AddField("yeniSifre", yeniSifre.text);


        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/SifreGuncelle.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                olay.OlayBaslatici(www.downloadHandler.text);
            }
        }
    }
}
