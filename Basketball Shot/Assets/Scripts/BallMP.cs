using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BallMP : MonoBehaviour
{
    //Yön hýzlarý.
    float verticalSpeed = 0;
    float horizontalSpeed = 0;

    //Component.
    public Rigidbody rb;
    public GameObject eventText;
    public GameObject pot1;
    public GameObject pot2;
    public GameObject area;

    //Hýz ve sekme ayarý.
    public float maxPower;
    float p1Power = 0f;
    float p2Power = 0f;
    public float tabScale;

    //Energy.
    public float startEnergy;
    float p1Energy;
    float p2Energy;
    public GameObject p1EText;
    public GameObject p2EText;

    //Sesler.
    AudioSource audioSource;
    public AudioClip tabSound;

    //Oyun.
    public bool isRunning = false;
    public bool endGame = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = rb.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isRunning)
        {
            ScreenMovements();
            Control();
            EnergyColor();
        }
    }

    void ScreenMovements()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector3 touchPos = touch.position;

                if (touchPos.x <= Screen.width / 2)
                {
                    if (touch.phase == TouchPhase.Stationary && p1Power <= maxPower && p1Energy > 0)
                    {
                        p1Power += Time.deltaTime * maxPower;
                        p1Energy -= Time.deltaTime * maxPower;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        rb.AddForce(new Vector3(1, 1, 0) * p1Power, ForceMode.Impulse); //Saða güç.
                        p1Power = 0f;
                    }
                }
                else
                {
                    if (touch.phase == TouchPhase.Stationary && p2Power <= maxPower && p2Energy > 0)
                    {
                        p2Power += Time.deltaTime * maxPower;
                        p2Energy -= Time.deltaTime * maxPower;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        rb.AddForce(new Vector3(-1, 1, 0) * p2Power, ForceMode.Impulse); //Sola güç.
                        p2Power = 0f;
                    }
                }
            }
        }

        //Klavye.
        if (Input.GetKey(KeyCode.A) && p1Power <= maxPower && p1Energy > 0)
        {
            p1Power += Time.deltaTime * maxPower;
            p1Energy -= Time.deltaTime * maxPower;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            float distanceX = Mathf.Abs((transform.position.x - pot1.transform.position.x) / (pot2.transform.position.x - pot1.transform.position.x));

            rb.AddForce(new Vector3(distanceX + 0.2f, 1f, 0) * p1Power, ForceMode.Impulse); //Saða güç.
            p1Power = 0;
        }

        if (Input.GetKey(KeyCode.L) && p2Power <= maxPower && p2Energy > 0)
        {
            p2Power += Time.deltaTime * maxPower;
            p2Energy -= Time.deltaTime * maxPower;
        }
        else if (Input.GetKeyUp(KeyCode.L)) 
        {
            float distanceX = Mathf.Abs((transform.position.x - pot2.transform.position.x) / (pot2.transform.position.x - pot1.transform.position.x));

            rb.AddForce(new Vector3(-distanceX - 0.2f, 1f, 0) * p2Power, ForceMode.Impulse); //Sola güç.
            p2Power = 0;
        }
    }

    void Control()
    {
        //HIZ KONTROLLERI.
        if (rb.velocity.y < verticalSpeed) //Düþeyde en yüksek hýz.
            verticalSpeed = -rb.velocity.y;

        if (Mathf.Abs(rb.velocity.y) > Mathf.Abs(horizontalSpeed)) //Yatayda en yüksek hýz.
            horizontalSpeed = rb.velocity.x;

        if (Mathf.Abs(transform.position.x) > 7.5f) //Saha sýnýrlarý.
        {
            eventText.GetComponent<Event>().EventText("Air Ball", UnityEngine.Color.white);
            GameStart();
        }

        //Enerji gösterge güncellemesi.
        p1EText.GetComponent<TextMeshProUGUI>().text = ((int)p1Energy).ToString();
        p2EText.GetComponent<TextMeshProUGUI>().text = ((int)p2Energy).ToString();


        //Enerji bitti mi kontrolü.
        if ((int)p1Energy == 0 && (int)p2Energy == 0 && p1Power == 0 && p2Power == 0 && Mathf.Abs(verticalSpeed) < 0.01f && Physics.Raycast(transform.position, Vector3.down, 0.1f))
        {
            GameStart();
            eventText.GetComponent<Event>().EventText("Are You Tired?", UnityEngine.Color.yellow);
        }

        //Süre bitti.
        if (endGame == true && Mathf.Abs(verticalSpeed) < 0.01f && Physics.Raycast(transform.position, Vector3.down, 0.01f))
            EndGame();
    }

    public void EnergyColor()
    {
        //Enerji gösterge renkleri.

        if (p1Energy >= 20)
            p1EText.GetComponent<TextMeshProUGUI>().color = UnityEngine.Color.green;
        else if (p1Energy >= 10)
            p1EText.GetComponent<TextMeshProUGUI>().color = UnityEngine.Color.yellow;
        else
            p1EText.GetComponent<TextMeshProUGUI>().color = UnityEngine.Color.red;

        if (p2Energy >= 20)
            p2EText.GetComponent<TextMeshProUGUI>().color = UnityEngine.Color.green;
        else if (p2Energy >= 10)
            p2EText.GetComponent<TextMeshProUGUI>().color = UnityEngine.Color.yellow;
        else
            p2EText.GetComponent<TextMeshProUGUI>().color = UnityEngine.Color.red;
    }

    public void GameStart(bool point = false)//TOPU BASLANGIC NOKTASINA GETIRME.
    {
        isRunning = false;

        if (((int)p1Energy == 0 && (int)p2Energy == 0) || point)
        {
            p1Energy = startEnergy; p2Energy = startEnergy;
        }

        transform.position = new Vector3(0f, -1f, 13.561f);
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        p1Power = 0; p2Power = 0;
        area.GetComponent<AudioSource>().Play();

        isRunning = true;
    }

    public void EndGame()
    {
        isRunning = false;
        area.GetComponent<AudioSource>().Play();
        eventText.GetComponent<Event>().EventText("Finish", UnityEngine.Color.red);
    }
        
    public void FirstStart()
    {
        GameStart();
        eventText.GetComponent<Event>().EventText("Good Luck", UnityEngine.Color.green);
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play(); //Sekme sesi çal.

        //DIKEYDE SEKME.
        if (verticalSpeed > tabScale)
        {
            rb.AddForce(Vector3.up * verticalSpeed / tabScale, ForceMode.Impulse); //Sekme yukarý.
            verticalSpeed = 0;
        }

        //YATAYDA SEKME.
        if (horizontalSpeed > 0 && collision.contacts[0].normal != Vector3.up)
        {
            rb.AddForce(Vector3.left * (horizontalSpeed / tabScale), ForceMode.Impulse); //Sekme Sola.
            horizontalSpeed = 0;
        }

        if (horizontalSpeed < 0 && collision.contacts[0].normal != Vector3.up)
        {
            rb.AddForce(Vector3.left * (horizontalSpeed / tabScale), ForceMode.Impulse); //Sekme Saða.
            horizontalSpeed = 0;
        }
    }
}


