using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadPanel : MonoBehaviour
{
    GameObject white;
    public GameObject pause;

    private void Start()
    {
        white = GameObject.FindGameObjectWithTag("White");
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        white.GetComponent<Rewarded>().ShowRewardedAd();
    }

    private void Update()
    {
        Time.timeScale = 0;
    }
}
