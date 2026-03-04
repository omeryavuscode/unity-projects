using System.Collections;
using TMPro;
using UnityEngine;

public class Olay : MonoBehaviour
{
    //Olay araçlarý
    public TextMeshProUGUI olayMetni;
    bool olayBasladiMi = false;

    //Olay göstergesi ilk konum
    Vector2 konum;

    void Start()
    {
        konum = new Vector2(0f,GetComponent<RectTransform>().rect.y + GetComponent<RectTransform>().rect.height); //Ýlk konum alýndý
    }

    public void OlayBaslatici(string metin)
    {
        olayMetni.text = metin; //Olay metni aktarma

        if (olayBasladiMi == false) //Eðer hali hazýrda olay yoksa baþlat
        {
            olayBasladiMi = true;
            StartCoroutine(OlayAnim());
        }
    }

    IEnumerator OlayAnim()
    {
        Vector2[] yeniKonum = new Vector2[2]; //Ýlk konum ve sonraki konumu tutar

        yeniKonum[0] = konum; //Ýlk konum
        yeniKonum[1] = new Vector2(0, yeniKonum[0].y - gameObject.GetComponent<RectTransform>().rect.height); //Gideceði konum

        while (yeniKonum[1].y < gameObject.GetComponent<RectTransform>().anchoredPosition.y)
        {
            Vector2 yeniAnchoredPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
            yeniAnchoredPosition.y -= 5;
            gameObject.GetComponent<RectTransform>().anchoredPosition = yeniAnchoredPosition;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(3); //Beklet

        //Eski konuma geri dön
        while (yeniKonum[0].y > gameObject.GetComponent<RectTransform>().anchoredPosition.y)
        {
            Vector2 yeniAnchoredPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
            yeniAnchoredPosition.y += 5;
            gameObject.GetComponent<RectTransform>().anchoredPosition = yeniAnchoredPosition;
            yield return new WaitForSeconds(0.01f);
        }

        olayBasladiMi = false; //Yeni olay baþlatýlabilir
    }
}
