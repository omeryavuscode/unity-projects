using UnityEngine;

public class ButonSesi : MonoBehaviour
{
    void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("efekt") / 100f; //Buton sesini sürekli olarak günceller.
    }
}
