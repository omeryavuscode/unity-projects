using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParaCekmeVT : MonoBehaviour
{
    //Girdiler
    public TMP_InputField payfixNo;
    public TMP_InputField OYSMiktari;
    public TMP_InputField sifre;

    //Para Çekme Butonu
    public Button paraCekmeButonu;

    //Olay
    Olay olay;

    private void Start()
    {
        olay = FindAnyObjectByType<Olay>();
    }

    private void Update()
    {
        //Banka bilgi uzunluklarýndan biri 10 uzunluðunda olmalý ve sifre dogru formatta olmalýdýr
        if (payfixNo.text.Length == 10 && sifre.text.Length >= 4 && sifre.text.Length <= 10 && Regex.IsMatch(sifre.text, "^[a-zA-Z0-9]*$") && OYSMiktari.text != "" && int.Parse(OYSMiktari.text) >= 1000)
        {
            paraCekmeButonu.interactable = true;
        }
        else
        {
            paraCekmeButonu.interactable= false;
        }
    }

    //PARA ÇEKME BUTONU
    public void ParaCekmeTalebiButonu()
    {
        if (int.Parse(OYSMiktari.text) >= 1000)
        {
            StartCoroutine(ParaCekmeTalebi());
        }
        else
        {
            olay.OlayBaslatici("Min Amount of Money 1000");
        }
    }

    //PARA ÇEKME TALEBÝNDE BULUNUR
    IEnumerator ParaCekmeTalebi()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "paraCekmeTalebi");
        form.AddField("kullaniciAdi", PlayerPrefs.GetString("kullaniciAdi"));
        form.AddField("OYSMiktari", OYSMiktari.text);
        form.AddField("payfixNo", payfixNo.text);
        form.AddField("sifre", sifre.text);


        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/ParaCekmeTalebi.php", form))
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
