using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Jammer : MonoBehaviour
{
    // White.
    public GameObject white;
    Game game;

    // Radar.
    Radar radar;

    // Yok olma süresi.
    public int lostTime;
    bool lostStart = false;
    public TextMeshPro timeText;

    // Tanýmlamalar.
    Animator anim;
    White whiteScript;

    private void Start()
    {
        radar = FindObjectOfType<Radar>();
        radar.target = this.gameObject;
        white = GameObject.FindGameObjectWithTag("White");
        game = white.gameObject.GetComponent<Game>();
        whiteScript = FindObjectOfType<White>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (lostStart == true)
            timeText.text = lostTime.ToString();
    }

    // Jammer içine enemy girerse.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (white.GetComponent<White>().isDead == false)
            if (collision.gameObject.tag == "Robo" && collision.GetComponent<Robo>().isDead == false && lostStart)
            {
                collision.gameObject.GetComponent<Robo>().isDead = true; // Robo öldürme.

                if (whiteScript.collect + collision.gameObject.GetComponent<Robo>().damage <= whiteScript.maxCollect) // Eðer robot öldükten sonra collect 100 ü geçmeyecekse.
                    whiteScript.collect += collision.gameObject.GetComponent<Robo>().damage; // Collect damage kadar dolar.
                else
                    whiteScript.collect = whiteScript.maxCollect; // Collect max hale gelir.

                whiteScript.score += collision.gameObject.GetComponent<Robo>().damage; // Score u damage kadar arttýrma.
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + collision.gameObject.GetComponent<Robo>().damage / 5); // Düþman hasarýnýn 5 e bölümünden gelen sonuç kadar coin.}
            }
            else if (collision.gameObject.tag == "Phone" && collision.GetComponent<Phone>().isDead == false && lostStart)
            {
                collision.gameObject.GetComponent<Phone>().isDead = true; // Robo öldürme.

                if (whiteScript.collect + collision.gameObject.GetComponent<Phone>().damage <= whiteScript.maxCollect) // Eðer robot öldükten sonra collect 100 ü geçmeyecekse.
                    whiteScript.collect += collision.gameObject.GetComponent<Phone>().damage; // Collect damage kadar dolar.
                else
                    whiteScript.collect = whiteScript.maxCollect; // Collect max hale gelir.

                whiteScript.score += collision.gameObject.GetComponent<Phone>().damage; // Score u damage kadar arttýrma.
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + collision.gameObject.GetComponent<Phone>().damage / 5); // Düþman hasarýnýn 5 e bölümünden gelen sonuç kadar coin.}
            }
            else if (collision.gameObject.tag == "White" && lostStart == false)
            {
                lostStart = true; // Jammer kaybetme animasyonu baþlatýcý kontrol
                anim.SetBool("lostStart", true); // Çalýþma animasyonu.
                timeText.color = Color.white; // Süre yazýsýný beyaza çevirme.
                StartCoroutine(Lost()); // Kaybolma zamanlayýcýsý baþlatma.
            }
    }

    // Yok olma fonksiyonu.
    IEnumerator Lost()
    {
        yield return new WaitForSeconds(1);
        lostTime -= 1;


        if (lostTime <= 0)
        {
            game.jamStart = false;
            anim.SetBool("lost", true);
            yield return new WaitForSeconds(1);
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Lost());
    }
}


