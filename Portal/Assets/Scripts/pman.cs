using UnityEngine;
using UnityEngine.UI;

public class pman : MonoBehaviour
{
    // Hareket
    public int hiz;
    int yatayYon = 0;
    bool zipliyorMu = false;

    // Bileþenler
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    // Butonlar
    public Image butonSol;
    public Image butonSag;
    public Image butonYukari;

    private void Start()
    {
        // Animator bileþenini al
        animator = GetComponent<Animator>();

        // Sprite Renderer bileþenini al
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Rigidbody bileþenini al
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HareketVeAnim();
    }

    void HareketVeAnim()
    {
        // Karakter yönünü ayarlama
        if (yatayYon == 1)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        // Hareket et
        transform.position += new Vector3(hiz * yatayYon * Time.deltaTime, 0, 0);

        // Animasyonlar
        if (zipliyorMu)
        {
            // Zýplama animasyonunu oynat
            animator.Play("ziplama");
        }
        else if (yatayYon != 0)
        {
            // Yürüme animasyonunu oynat
            animator.Play("yurume");
        }
        else
        {
            // Boþ animasyonu oynat
            animator.Play("bos");
        }
    }

    public void YonButonSol() // Sol Yön Butonu
    {
        yatayYon = -1;
        butonSol.color = Color.gray;
    }

    public void YonButonSag() // Sað Yön Butonu
    {
        yatayYon = 1;
        butonSag.color = Color.gray;
    }

    public void YonButonYukari() // Yukarý Yön Butonu
    {
        if (!zipliyorMu)
        {
            zipliyorMu = true;

            rb.AddForce(Vector2.up * hiz, ForceMode2D.Impulse);
        }
    }

    public void YonButonYok() // Yön Butonu Yok
    {
        yatayYon = 0;
        butonSol.color = Color.white;
        butonSag.color = Color.white;
    }

}
