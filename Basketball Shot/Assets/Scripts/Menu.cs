using System.Collections;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    //MP GAME.
    public int p1 = 0;
    public int p2 = 0;
    public int startTime;

    public TextMeshProUGUI p1Text;
    public TextMeshProUGUI p2Text;
    public TextMeshProUGUI timeText;
    public GameObject ballMPObje;
    public GameObject[] GameMPObject;

    //OBJE
    public GameObject cameraObje;
    public GameObject[] pauseObject;

    public void GameMPStart()
    {
        foreach (GameObject go in GameMPObject)
            go.SetActive(true);

        cameraObje.GetComponent<Camera>().isRunning = true;
        ballMPObje.GetComponent<BallMP>().isRunning = true;

        ballMPObje.GetComponent<BallMP>().FirstStart();

        foreach (GameObject go in pauseObject)
            go.SetActive(false);

        GameMPUpdate();
        StartCoroutine(GameMPTime());
    }

    public void GameMPUpdate()
    {
        p1Text.text = p1.ToString();
        p2Text.text = p2.ToString(); 
    }

    IEnumerator GameMPTime() 
    {
        if (startTime <= 0)
            ballMPObje.GetComponent<BallMP>().endGame = true;
        else 
        {
            yield return new WaitForSeconds(1);
            startTime -= 1;
            timeText.text = startTime.ToString() + "s";
            StartCoroutine(GameMPTime());
        }

        if (startTime > 45)
            timeText.color = Color.green;
        else if (startTime > 15)
            timeText.color = Color.yellow;
        else 
            timeText.color = Color.red;
    }
}
