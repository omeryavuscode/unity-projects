using UnityEngine;

public class Muzik : MonoBehaviour
{
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Muzik").Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("muzik"))
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("muzik") / 100f;
        else
        {
            PlayerPrefs.SetFloat("muzik", 25);
            PlayerPrefs.SetFloat("efekt", 100);
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("muzik") / 100f;
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("muzik") / 100f;
    }
}
