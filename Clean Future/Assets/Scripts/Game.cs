using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Best Score.
    int bestScore;

    // White.
    White whiteScript;

    // Haritaya basýlacak Enemyler.
    public GameObject[] enemy;
    Robo roboScript;
    Phone phoneScript;

    // Haritaya basýlacak Jammer.
    public GameObject jam;

    // Haritaya basýlacak itemler.
    public GameObject[] items;

    // Basýlacak ENEMYLERÝN:
    public int enemyDifference; // Uzaklýðý.
    public int enemyTime; // Süresi.
    bool enemyStart = false;
    int enemyCount = 50;

    // Basýlacak JAMMERLARIN:
    public int jamDifference; // Uzaklýðý.
    public bool jamStart = false;

    // Basýlacak ITEMLERÝN:
    public int itemsDifference; // Uzaklýðý.
    public int itemsTime; // Süresi.
    bool itemsStart = false;

    // Etraftaki objeler.
    public GameObject[] prefab;
    public int numObjects;
    public float difference;

    // Basýlacak Coinler.
    public GameObject coin;
    public int coinDifference;
    public int coinTime;
    bool coinStart = false;

    // Panel.
    public GameObject pausePanel;

    private void Start()
    {
        // Karakterin sensitivity ayarý yapýlýr.
        FindObjectOfType<White>().sensitivity = ((int)(PlayerPrefs.GetFloat("Sens") * 100));

        // Baþlangýçta harita oluþumu.
        for (int i = 0; i < numObjects; i++)
        {
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-difference, difference), Random.Range(-difference, difference), 0);
            Instantiate(prefab[Random.Range(0, prefab.Length)], randomPosition, Quaternion.identity);
        }

        whiteScript = gameObject.GetComponent<White>();
        roboScript = enemy[0].GetComponent<Robo>();
        phoneScript = enemy[1].GetComponent<Phone>();
        bestScore = PlayerPrefs.GetInt("Best Score"); // Best Score çekimi.
    }

    void Update()
    {
        Score();

        if (!enemyStart && GameObject.FindGameObjectsWithTag("Phone").Length + GameObject.FindGameObjectsWithTag("Robo").Length <= enemyCount)
            StartCoroutine(Produce());

        if (!itemsStart)
            StartCoroutine(ItemProduce());

        if (!jamStart)
        {
            jamStart = true;
            JammerProduce();
        }

        if (coinStart == false && GameObject.FindGameObjectsWithTag("Coin").Length <= 100)
        {
            StartCoroutine(Coin());
        }
    }

    // Enemy üretim.
    IEnumerator Produce()
    {

        enemyStart = true; // Ard arda basýlmamasý için.
        yield return new WaitForSeconds(enemyTime);

        // Difference mesafesinde bir yerde.
        Vector3 position = new Vector3(Random.Range(0, 2) == 0 ? transform.position.x + enemyDifference : transform.position.x - enemyDifference, Random.Range(0, 2) == 0 ? transform.position.y + enemyDifference : transform.position.y - enemyDifference, 0);

        if (whiteScript.score < 1600)
            Instantiate(enemy[0], position, Quaternion.identity);
        else
            Instantiate(enemy[1], position, Quaternion.identity);

        enemyStart = false; // Oluþum baþlatýlsýn.
    }

    void JammerProduce()
    {
        Vector3 position = new Vector3(Random.Range(0, 2) == 0 ? transform.position.x + jamDifference : transform.position.x - jamDifference, Random.Range(0, 2) == 0 ? transform.position.y + jamDifference : transform.position.y - jamDifference, 0);
        Instantiate(jam, position, Quaternion.identity);
    }

    // Item üretim
    IEnumerator ItemProduce()
    {
        itemsStart = true; // Ard arda basýlmamasý için.
        yield return new WaitForSeconds(itemsTime);

        // Difference mesafesinde bir yerde.
        Vector3 position = new Vector3(Random.Range(0, 2) == 0 ? transform.position.x + itemsDifference : transform.position.x - itemsDifference, Random.Range(0, 2) == 0 ? transform.position.y + itemsDifference : transform.position.y - itemsDifference, 0);
        Instantiate(items[Random.Range(0, items.Length)], position, Quaternion.identity);

        itemsStart = false; // Oluþum baþlatýlsýn.
    }

    IEnumerator Coin()
    {
        coinStart = true;
        yield return new WaitForSeconds(coinTime);
        // coinDifference mesafesinde bir yerde.
        Vector3 randomPosition = transform.position + new Vector3(Random.Range(-coinDifference, coinDifference + 1), Random.Range(-coinDifference, coinDifference + 1), 0);
        Instantiate(coin, randomPosition, Quaternion.identity);
        coinStart = false;
    }

    // Scora göre zorluk.
    void Score()
    {
        int score = whiteScript.score;
        if (score < 50)
        {
            enemyCount = 25;
            whiteScript.collectTime = 0.4f;
            roboScript.radius = 2f;
            roboScript.speed = 3;
            roboScript.damage = 10;
            enemy[0].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            roboScript.lvl.text = "lvl 1";
        }
        else if (score < 200)
        {
            whiteScript.collectTime = 0.350f;
            roboScript.radius = 1.9f;
            roboScript.speed = 3;
            roboScript.damage = 15;
            enemy[0].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            roboScript.lvl.text = "lvl 2";
        }
        else if (score < 400)
        {
            whiteScript.collectTime = 0.300f;
            roboScript.radius = 1.8f;
            roboScript.speed = 3;
            roboScript.damage = 20;
            enemy[0].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            roboScript.lvl.text = "lvl 3";
        }
        else if (score < 800)
        {
            whiteScript.collectTime = 0.250f;
            roboScript.radius = 1.7f;
            roboScript.speed = 3;
            roboScript.damage = 25;
            enemy[0].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            roboScript.lvl.text = "lvl 4";
        }
        else if (score < 1600)
        {
            enemyCount = 50;
            whiteScript.collectTime = 0.150f;
            roboScript.radius = 1.5f;
            roboScript.speed = 2;
            roboScript.damage = 35;
            enemy[0].GetComponent<Transform>().localScale = new Vector3(2, 2, 1);
            roboScript.lvl.text = "lvl 5";
        }
        else if (score < 2000) // Phone
        {  
            whiteScript.collectTime = 0.15f;
            phoneScript.radius = 2f;
            phoneScript.speed = 3;
            phoneScript.damage = 20;
            enemy[1].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            phoneScript.lvl.text = "lvl 1";
        }
        else if (score < 2500)
        {
            whiteScript.collectTime = 0.15f;
            phoneScript.radius = 1.9f;
            phoneScript.speed = 3;
            phoneScript.damage = 25;
            enemy[1].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            phoneScript.lvl.text = "lvl 2";
        }
        else if (score < 3000)
        {
            whiteScript.collectTime = 0.15f;
            phoneScript.radius = 1.8f;
            phoneScript.speed = 3;
            phoneScript.damage = 30;
            enemy[1].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            phoneScript.lvl.text = "lvl 3";
        }
        else if (score < 3500)
        {
            enemyCount = 75;
            whiteScript.collectTime = 0.15f;
            phoneScript.radius = 1.7f;
            phoneScript.speed = 3;
            phoneScript.damage = 35;
            enemy[1].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            phoneScript.lvl.text = "lvl 4";
        }
        else if (score < 4000)
        {
            enemyCount = 100;
            whiteScript.collectTime = 0.15f;
            phoneScript.radius = 1.5f;
            phoneScript.speed = 2;
            phoneScript.damage = 45;
            enemy[1].GetComponent<Transform>().localScale = new Vector3(2, 2, 1);
            phoneScript.lvl.text = "lvl 5";
        }

        if (score > bestScore)
            PlayerPrefs.SetInt("Best Score", score);
    }

    public void Pause() // Oyun duraksamasý ve pause panelinin açýlmasý.
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        pausePanel.SetActive(!pausePanel.activeSelf);
    }
}
