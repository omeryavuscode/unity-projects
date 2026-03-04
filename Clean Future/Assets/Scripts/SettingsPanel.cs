using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settingsanel : MonoBehaviour
{
    public GameObject musicSlider;
    public GameObject musicText;
    public GameObject music;
    float musicValue;

    public GameObject sensSlider;
    public GameObject sensText;
    float sensValue;

    // Menuye dönüþ.
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    // Panel açýldýðýnda ayarlarýn eskisi gibi çekilmesi.
    private void Start()
    {
        musicValue = PlayerPrefs.GetFloat("Music");
        musicSlider.GetComponent<Slider>().value = musicValue;

        sensValue = PlayerPrefs.GetFloat("Sens");
        sensSlider.GetComponent<Slider>().value = sensValue;
    }

    private void Update()
    {
        Time.timeScale = 0;

        musicValue = musicSlider.GetComponent<Slider>().value;
        musicText.GetComponent<TextMeshProUGUI>().text = "Music : " + ((int)(musicValue * 100)).ToString();
        PlayerPrefs.SetFloat("Music", musicValue);

        sensValue = sensSlider.GetComponent<Slider>().value;
        sensText.GetComponent<TextMeshProUGUI>().text = "Sensitivity : " + ((int)((sensValue) * 100)).ToString();
        PlayerPrefs.SetFloat("Sens", sensValue);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
