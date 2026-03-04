using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI bestScore;
    public TextMeshProUGUI lastScore;
    public AudioSource clickS;
    public GameObject settingsPanel;
    public GameObject customPanel;

    public GameObject[] customs; // Custom Butonlarý.

    public Sprite[] whiteCustoms; // Butonlarda gözükecek custom spritelarý.

    public int[] customsPay; // Customlarýn fiyatlarý.
    public Button selectedCustom; // Seçili olan custom.

    // Sesler
    public AudioSource sound;
    public AudioClip lockS;
    public AudioClip unlockS;
    public AudioClip selectedS;

    void Start()
    {
        Time.timeScale = 1;

        bestScore.text = "Best : " + PlayerPrefs.GetInt("Best Score").ToString();
        lastScore.text = "Last : " + PlayerPrefs.GetInt("Last Score").ToString();

        if (!PlayerPrefs.HasKey("Music")) // Varsayýlan deðer.
        {
            PlayerPrefs.SetFloat("Music", 0.75f);
            PlayerPrefs.SetFloat("Sens", 0.5f);
            PlayerPrefs.SetInt("Coin", 0);
            PlayerPrefs.SetInt("White (1)", 1);
            PlayerPrefs.SetString("SelectedCustom", "White 1");
        }

        for (int i = 0; i < whiteCustoms.Length; i++)
            if (PlayerPrefs.GetString("SelectedCustom") == whiteCustoms[i].name)
                selectedCustom.GetComponent<Image>().sprite = whiteCustoms[i];      
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        settingsPanel.SetActive(true);
    }

    public void Custom()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        customPanel.SetActive(true);
    }
}
