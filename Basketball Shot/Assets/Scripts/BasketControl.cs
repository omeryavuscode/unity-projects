using UnityEngine;

public class BasketControl : MonoBehaviour
{
    AudioSource audioSource;

    public GameObject UI;
    Menu game;

    public GameObject ball;
    BallMP ballCode;

    public GameObject eventText;

    private void Start()
    {
        game = UI.GetComponent<Menu>();
        audioSource = GetComponent<AudioSource>();
        ballCode = ball.GetComponent<BallMP>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.name[name.Length - 1] == '1')
            game.p2 += 1;

        else
            game.p1 += 1;

        game.GameMPUpdate();
        eventText.GetComponent<Event>().EventText("Point", UnityEngine.Color.white); //Yazý.
        ballCode.GameStart(true); //Top baþlangýç kordinatýna alýnýr.
    }
}
