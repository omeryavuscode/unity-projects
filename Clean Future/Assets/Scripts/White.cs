using System.Collections;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class White : MonoBehaviour
{
    // Hasar aldýktan sonra tekrar almak için bekleme süresi.
    public float hitSecond;
    public bool getHit = true;
    public bool isDead = false;
    public bool startDead = false;

    // Can deðeri.
    public float health;
    public float maxHealth = 100;
    public Image healthBar;
    public TextMeshProUGUI healthText;

    // Collect deðeri.
    public float collect;
    public float maxCollect;
    public float collectTime;
    public Image collectBar;
    public TextMeshProUGUI collectText;

    // Score deðeri.
    public int score;
    public Image scoreBar;
    public TextMeshProUGUI scoreText;

    // Kaydýrma noktalarý ve kaydýrma farký.
    Vector3 swipeStart;
    Vector3 swipeEnd;
    Vector3 swipe;
    public int maxSpeed;
    public float sensitivity;

    // Karakter yönü ve hýzý.
    Vector3 direction;

    // Animasyon ve ses.
    Animator anim;
    SpriteRenderer sprite;
    AudioSource audioSource;
    public AudioClip hitS;
    public AudioClip collectS;
    public AudioClip coinS;
    public AudioClip deadS;
    public AudioClip protectionS;
    public Color transColor;

    // Kamera.
    public Camera cameraShake;

    // Panel
    public GameObject deadPanel;

    private void Start()
    {
        health = maxHealth; // Baþlangýçtaki can max.
        collect = maxCollect; // Baþlangýçtaki collect max.
        StartCoroutine(Control()); // Collect controlleri.
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (health <= 0)
            isDead = true;

        if (isDead == false)
        {
            Touch(); // Dokunma algýlama yön taini.
            MoveAndAnim(); // Hareket ve hareket animasyonlarý.
        }
        else
        {
            StartCoroutine(Dead()); // Dead.
            startDead = true; // Ölüm baþladý.
        }

        sensitivity = PlayerPrefs.GetFloat("Sens") * 100;
    }

    // White hareketi ve hareket animasyonlarý.
    void MoveAndAnim()
    {
        if (math.abs(direction.x) > 0 && math.abs(direction.y) > 0)
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, math.clamp((math.abs(direction.x) + math.abs(direction.y)), 0, maxSpeed) * Time.deltaTime * math.sin(45));
        else
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, math.clamp((math.abs(direction.x) + math.abs(direction.y)), 0, maxSpeed) * Time.deltaTime);

        if ((math.abs(direction.x) > 0 || math.abs(direction.y) > 0) && Time.timeScale == 1) // Eðer hareket varsa.
        {
            //Gittiði yöne göre animasyonlar.
            if (direction.y > 0 && direction.y > math.abs(direction.x))
            {
                anim.Play("Up " + new string(PlayerPrefs.GetString("SelectedCustom").Where(char.IsDigit).ToArray()));
            }
            else if (direction.y < 0 && math.abs(direction.y) > math.abs(direction.x))
            {
                anim.Play("Down " + new string(PlayerPrefs.GetString("SelectedCustom").Where(char.IsDigit).ToArray()));
            }
            else if (direction.x > 0)
            {
                sprite.flipX = true;
                anim.Play("LeftOrRight " + new string(PlayerPrefs.GetString("SelectedCustom").Where(char.IsDigit).ToArray()));
            }
            else if (direction.x < 0)
            {
                sprite.flipX = false;
                anim.Play("LeftOrRight " + new string(PlayerPrefs.GetString("SelectedCustom").Where(char.IsDigit).ToArray()));
            }

        }
        else
        {
            anim.Play("Idle " + new string(PlayerPrefs.GetString("SelectedCustom").Where(char.IsDigit).ToArray())); // Haraket yoksa boþta animasyonu.
        }
    }

    // Ekrana dokunma algýlama yön verme.
    void Touch()
    {
        if (UnityEngine.Input.touchCount > 0) // Ekrandaki dokunma sayýsý 0 dan büyük mü?
        {
            Touch touch = UnityEngine.Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position; // Ýlk dokunma.
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                swipeEnd = touch.position; // Devam eden dokunma.
                swipe = swipeEnd - swipeStart;

                direction.x = math.clamp(swipe.x / sensitivity, -maxSpeed, maxSpeed);
                direction.y = math.clamp(swipe.y / sensitivity, -maxSpeed, maxSpeed);
            }
        }
        else
        {
            direction = Vector3.zero;
        }
    }

    // Temas.
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Eðer hasar verecek karakter varsa ve hasar alabilecek dururmdaysa hasar verir.
        if (collision.gameObject.tag == "Robo" && getHit == true && !collision.gameObject.GetComponent<Robo>().isDead && isDead == false)
        {
            if (health - collision.gameObject.GetComponent<Robo>().damage >= 0) // Can - ye düþmüyorsa.
                health -= collision.gameObject.GetComponent<Robo>().damage; // Hasar alma.
            else
                health = 0;

            getHit = false; // Vurulamaz.
            StartCoroutine(Hit());
        }
        else if (collision.gameObject.tag == "Phone" && getHit == true && !collision.gameObject.GetComponent<Phone>().isDead && isDead == false)
        {
            if (health - collision.gameObject.GetComponent<Phone>().damage >= 0) // Can - ye düþmüyorsa.
                health -= collision.gameObject.GetComponent<Phone>().damage; // Hasar alma.
            else
                health = 0;

            getHit = false; // Vurulamaz.
            StartCoroutine(Hit());
        }
        else if (collision.gameObject.tag == "Message" && isDead == false && getHit == true)
        {
            collision.gameObject.SetActive(false);
            if (health - 5 >= 0) // Can - ye düþmüyorsa.
                health -= 5; // Hasar alma.
            else
                health = 0;
            StartCoroutine(Hit());
        }
    }

    // Hasar aldýðýnda.
    IEnumerator Hit()
    {
        cameraShake.Shake();
        audioSource.clip = hitS; // Hasar sesini yükle.
        audioSource.Play(); // Hasar sesi.
        sprite.color = Color.red; // Renk kýrmýzý.
        yield return new WaitForSeconds(hitSecond);
        sprite.color = Color.white; // Renk beyaz.
        getHit = true; // Vurulabilir.
    }

    // Ölüm.
    IEnumerator Dead()
    {
        if (startDead == false)
        {
            PlayerPrefs.SetInt("Last Score", score);
            audioSource.clip = deadS; audioSource.Play(); // Ölüm sesi.
            anim.Play("Dead " + new string(PlayerPrefs.GetString("SelectedCustom").Where(char.IsDigit).ToArray())); // Ölüm animasyonu.
            yield return new WaitForSeconds(3); // 3sn sonra öl.
            GameObject.Find("Pause").SetActive(false);
            deadPanel.SetActive(true);
            GetComponent<Insterstitial>().ShowInterstitialAd();
        }
    }

    public void Armor()
    {
        StartCoroutine(Protection());
    }

    public IEnumerator Protection() // 3 Saniye boyunca hasar alýmýný engeller.
    {
        getHit = false;
        audioSource.clip = protectionS; audioSource.Play();
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.25f);
            gameObject.GetComponent<SpriteRenderer>().color = transColor;
            yield return new WaitForSeconds(0.25f);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        getHit = true;
    }

    IEnumerator Control()
    {
        yield return new WaitForSeconds(collectTime); // Belirtilen süre kadar hýzlý collect ve can azaltýmý olur.

        // Collect 0 dan büyükse collect deðilse.
        // Health 0 dan büyükse health azaltýlýr.
        if (collect > 0)
            collect -= 1;
        else if (health > 0)
            health -= 1;

        float h = health / maxHealth; // Can yüzdesi.
        healthBar.GetComponent<RectTransform>().localScale = new Vector3(h, 1f, 1f); // Can çubuðu ayarý.
        healthText.text = health.ToString(); // Can çubuðu text.

        float c = collect / maxCollect; // Collect yüzdesi.
        collectBar.GetComponent<RectTransform>().localScale = new Vector3(c, 1, 1); // Collect çubuðu ayarý.
        collectText.text = collect.ToString() + " / " + maxCollect.ToString(); // Collect çubuðu text.

        scoreText.text = score.ToString(); // Score çubuðu text.

        StartCoroutine(Control());
    }

    public void Items() { audioSource.clip = collectS; audioSource.Play(); }
    public void Coin() { audioSource.clip = coinS; audioSource.Play(); }
}
