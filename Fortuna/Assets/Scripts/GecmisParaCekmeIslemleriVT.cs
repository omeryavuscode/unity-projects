using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GecmisParaCekmeIslemleriVT : MonoBehaviour
{
    public GameObject prefab;

    public void Start()
    {
        StartCoroutine(GecmisParaCekmeIslemleri());
    }

    IEnumerator GecmisParaCekmeIslemleri()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "gecmisParaCekmeIslemleri");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));
        form.AddField("sifre", PlayerPrefs.GetString("sifre"));

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/GecmisParaCekmeIslemleri.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                string[] islemler = www.downloadHandler.text.Split('\n'); //Metni bölerek diziye atar.

                foreach (Transform child in transform) //Önceki alt nesneleri siler
                {
                    Destroy(child.gameObject);
                }

                if (islemler.Length > 1)
                {
                    for (int i = 0; i < islemler.Length - 1; i++) //Ýþlemleri ekleme
                    {
                        GameObject islem = Instantiate(prefab,transform);
                        islem.GetComponent<TextMeshProUGUI>().text = $"{islemler[i]}";
                    }
                }
                else
                {
                    GameObject islem = Instantiate(prefab, transform);
                    islem.GetComponent<TextMeshProUGUI>().text = "No past transactions found.";
                }

            }
        }
    }
}
