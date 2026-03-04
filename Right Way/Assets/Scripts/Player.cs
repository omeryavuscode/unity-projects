using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Can ve score.
    int heal;
    public int score = 0;
    public GameObject healBar;
    public GameObject hearthObje;
    public GameObject scoreText;
    public List<GameObject> hearth;

    //Hýz ve yol.
    public float speed; //Hýz.
    public int roadCount; //Yol sayýsý.
    public Vector3[] roadCoord; //Yol coord.
    int activeCoord; //Aktif coord.

    //Ekran.
    public float spacingX; //Ekran geniþliði.
    public float spacingY; //Ekran uzunluðu.

    //Dokunma
    Vector3 swipeStart; //Ýlk dokunma.
    Vector3 swipeMoved; //Kayan dokunma.
    float swipeDistanceX; //Kaydýrma mesafesi.
    public float sensibility; //Kaydýrma mesafesinin olmasý gereken deðer.
    bool newTouch = false; //Yeni dokunma

    //Boyut
    public float scale;

    //Line
    public LineRenderer lineRenderer;

    //Enemy Slow
    public int minSlow = 3;

    private void Start()
    {
        RoadUpdate();
        hearth = GameObject.FindGameObjectsWithTag("Hearth").ToList<GameObject>();
        heal = hearth.Count;
    }

    private void Update()
    {
        Move();
        Swipe();
        Dead();
        float fps = 1 / Time.deltaTime;
        //Debug.Log($"Fps : {fps}");
    }

    //Hareket.
    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, roadCoord[activeCoord], (spacingX / roadCount) * speed * Time.deltaTime);
    }

    //Yol güncellemesi.
    public void RoadUpdate()
    {
        roadCoord = new Vector3[roadCount]; //Yol boyutunda vector dizi oluþturur.

        //Ekran köþeleri.
        Vector3 bottomLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 bottomRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
        Vector3 topLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));

        spacingX = bottomRightCorner.x - bottomLeftCorner.x; //Ekran geniþliði.
        spacingY = topLeftCorner.y - bottomRightCorner.y; //Ekran yüksekliði.

        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road"); //Ekrandaki yollar.
        for (int i = 0; i < roads.Length; i++) //Eski yollar kaldýrýlýr.
            roads[i].SetActive(false);


        for (int i = 1; i <= roadCount; i++) //Yol konumlarý belirlenir ve yol çizgileri çizilir.
        {
            roadCoord[i - 1] = new Vector3(bottomLeftCorner.x + (spacingX / (roadCount + 1)) * i, bottomLeftCorner.y + (spacingY * (1f / 5f)), 0); //Yollar atandý.

            //Çizgi oluþumu.
            LineRenderer gameObject = Instantiate(lineRenderer);
            gameObject.positionCount = 2; //Çizginin nokta sayýsý.
            gameObject.sortingOrder = -1;
            gameObject.SetWidth(spacingX / roadCount / 2, spacingX / roadCount / 2);
            scale = spacingX / roadCount / 3;
            gameObject.SetPosition(0, new Vector3(roadCoord[i - 1].x, bottomLeftCorner.y));
            gameObject.SetPosition(1, new Vector3(roadCoord[i - 1].x, topLeftCorner.y));
        }

        transform.localScale = new Vector3(scale,scale); //Boyut güncellemesi.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SetActive(false);
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        if (heal > 0)
        {
            heal--;
            CanvasControl();
            //Handheld.Vibrate();
            gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            yield return new WaitForSeconds(0.25f);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void CanvasControl()
    {
        if (heal < hearth.Count)
            for (int i = hearth.Count; i > heal; i--)
                hearth[i - 1].gameObject.SetActive(false);

        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    void Dead()
    {
        if (heal == 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    //Kaydýrma.
    void Swipe()
    {
        if (Input.touchCount > 0) //Ekrana dokunma.
        {
            Touch touch = Input.GetTouch(0); //Dokunma özellikleri.

            if (touch.phase == TouchPhase.Began) //Ýlk dokunma.
            {
                swipeStart = touch.position; //Ýlk pozisyon.
                newTouch = true; //Yeni dokunma aktif.
            }
            else if (touch.phase == TouchPhase.Moved && newTouch == true) // Ekrandan elini kaldýrma
            {
                swipeMoved = touch.position;

                swipeDistanceX = swipeMoved.x - swipeStart.x; //Yatay kaydýrma mesafesi.

                if (swipeDistanceX > sensibility && activeCoord != roadCount - 1) //Saða kayabilme durumu.
                {
                    activeCoord++; //Sað.
                    newTouch = false; //Yeni haraket sonlandý.
                }

                if (swipeDistanceX < -sensibility && activeCoord != 0) //Sola kayabilme durumu.
                {
                    activeCoord--; //Sol.
                    newTouch = false; //Yeni haraket sonlandý.
                }
            }
            else if (touch.phase == TouchPhase.Ended) //Dokunma bitti.
                newTouch = false; //Yeni haraket sonlandý.
        }
    }
}
