using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Robo : MonoBehaviour
{
    // Temel.
    public int damage; // Verilecek hasar deðeri.
    public float radius; // White takip mesafesi.
    public float speed; // Karakter hýzý.

    // White takip edilmiyorken izlenecek yol ve her yol oluþturulduðunda 0 ile hangi mesafeler arasýnda yol alsýn.
    Vector3 targetPoint;
    public int targetDifference;

    // Tanýmlamalar.
    White whiteScript;
    public GameObject white;
    Animator anim;
    AudioSource audioSoruce;

    // Dead.
    public bool isDead = false; // Öldü mü?
    bool idead = true;
    public int deadTime; // Öldükten kaç saniye sonra yok olsun.
    public AudioClip deadSound; // Ölüm sesi.
    public int deadDifference; // Whitten uzaklaþýrsa öleceði mesafe.

    // Level.
    public TextMeshPro lvl;

    private void Start()
    {
        whiteScript = FindObjectOfType<White>(); // White Script.
        white = GameObject.FindGameObjectWithTag("White"); // White objesi sahneden çekiliyor.
        anim = GetComponent<Animator>(); // Animatör.
        audioSoruce = GetComponent<AudioSource>(); // Audio Source

        // Hedef boþta ise rastgele noktaya gider.
        targetPoint = transform.position + new Vector3(UnityEngine.Random.Range((-targetDifference), targetDifference + 1), UnityEngine.Random.Range((-targetDifference), targetDifference + 1), 0);
    }

    private void Update()
    {
        if (!isDead) // Ölmediyse.
        {
            MoveAndAnim();
        }
        else
        {
            StartCoroutine(Dead());
        }

        if (Distance(white) >= deadDifference)
            gameObject.SetActive(false);

    }

    // Haraket.
    private void MoveAndAnim()
    {
        if (Distance(white) < radius && whiteScript.getHit == true && whiteScript.isDead == false) // White a vurulbilirse takip edilir.
        {
            if (white.transform.position.x - transform.position.x > 0)
                GetComponent<SpriteRenderer>().flipX = true;
            else
                GetComponent<SpriteRenderer>().flipX = false;


            transform.position = Vector3.MoveTowards(transform.position, white.gameObject.transform.position, speed * Time.deltaTime); // Hareket.
            anim.Play("Walk");

            // White uzaklaþýrsa robonun gideceði yeni konum.
            targetPoint = transform.position + new Vector3(UnityEngine.Random.Range((-targetDifference), targetDifference + 1), UnityEngine.Random.Range((-targetDifference), targetDifference + 1), 0);
        }
        else
        {
            if (targetPoint.x - transform.position.x > 0)
                GetComponent<SpriteRenderer>().flipX = true;
            else
                GetComponent<SpriteRenderer>().flipX = false;

            if (targetPoint == transform.position) // Hedef noktaya ulaþtýysa yeni konum.
            {
                targetPoint = transform.position + new Vector3(UnityEngine.Random.Range((-targetDifference), targetDifference + 1), UnityEngine.Random.Range((-targetDifference), targetDifference + 1), 0);
            }
            else // Ulaþmadýysa hedef noktaya hareket.
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
                anim.Play("Walk");
            }
        }
    }

    // Kim ile arasýndaki mesafeyi kontrol etmek istiyor?
    private float Distance(GameObject target)
    {
        float distance = Vector3.Distance(target.transform.position, transform.position); // Seçilen nesne ile arasýndaki fark.
        return Math.Abs(distance); // Mutlak deðer ile gönderiliyor.
    }

    // Robo öldüðünde.
    private IEnumerator Dead()
    {
        if (idead == true) // 1 kere çaðrýldýðýný kontrol etmek için.
        {
            idead = false; // Control.
            audioSoruce.clip = deadSound; audioSoruce.Play(); // Ölüm sesi yüklendi ve çaldý.
            anim.Play("Dead"); // Ölüm animasyonu.
            yield return new WaitForSeconds(deadTime); // Kaç saniye sonra yok olmasý gerektiði.
            gameObject.SetActive(false); // Yok oluþ.
        }
    }
}
