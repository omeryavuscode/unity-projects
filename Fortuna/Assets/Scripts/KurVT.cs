using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class KurVT : MonoBehaviour
{
    public void KurCalistir()
    {
        StartCoroutine(Kur());
    }

    IEnumerator Kur()
    {
        WWWForm form = new WWWForm();
        form.AddField("fortuna", "kur");

        using (UnityWebRequest www = UnityWebRequest.Post("https://omeryavus.online/Fortuna/ParaCekmeTalebi.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                GetComponent<TextMeshProUGUI>().text = www.downloadHandler.text;
            }
        }

    }
}
