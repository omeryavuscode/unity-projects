using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class KullanicilarVT : MonoBehaviour
{
    //Kullanýcý adý ve baslik için objeler
    public GameObject baslik;
    public GameObject prefab;

    //KULLANCILARIN ÝSÝMLERÝNÝ ÇEKER VE BÖLEREK DÝZÝYE ATAR
    public IEnumerator Kullanicilar()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "aktifKullanicilar");

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/Kullanicilar.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                string[] kullaniciAdlari = www.downloadHandler.text.Split('\n'); //Metni bölerek diziye atar.

                string aktifKullaniciSayisi = String.Format("{0:n0}", double.Parse(kullaniciAdlari[0]));
                baslik.GetComponent<TextMeshProUGUI>().text = $"{aktifKullaniciSayisi} Players";

                for (int i = 1; i < kullaniciAdlari.Length - 1; i++) //Kullanýcý isimlerini ekleme.
                {
                    GameObject kullanici = Instantiate(prefab, transform);
                    kullanici.GetComponent<TextMeshProUGUI>().text = $"{i}{kullaniciAdlari[i]}";

                    if (kullanici.GetComponent<TextMeshProUGUI>().text.Contains("Online"))
                        kullanici.GetComponent<TextMeshProUGUI>().color = Color.green;

                    if (i <= 3)
                        kullanici.GetComponent<TextMeshProUGUI>().color = Color.yellow;
                }

                gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(1, -100000, 1);
            }
        }
    }
}
