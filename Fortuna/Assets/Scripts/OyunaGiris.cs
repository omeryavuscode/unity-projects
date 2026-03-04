using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OyunaGiris : MonoBehaviour
{
    public string oyunAdi;
    public Animator gecisAnimasyonu;

    public void oyunaGiris()
    {
        StartCoroutine(giris());
    }

    IEnumerator giris()
    {
        gecisAnimasyonu.GetComponent<Animator>().Play("1");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(oyunAdi);
    }
}
