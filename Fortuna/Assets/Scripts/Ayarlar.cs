using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ayarlar : MonoBehaviour
{
    public TextMeshProUGUI muzikMetni;
    public TextMeshProUGUI efektMetni;
    public Slider muzik;
    public Slider efekt;

    public void Start()
    {
        muzik.value = (int)PlayerPrefs.GetFloat("muzik");
        efekt.value = (int)PlayerPrefs.GetFloat("efekt");
    }

    void Update()
    {
        PlayerPrefs.SetFloat("muzik", muzik.value);
        muzikMetni.text = $"Music: {muzik.value}";

        PlayerPrefs.SetFloat("efekt", efekt.value);
        efektMetni.text = $"Effect: {efekt.value}";

        PlayerPrefs.Save();
    }
}
