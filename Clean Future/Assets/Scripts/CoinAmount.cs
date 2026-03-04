using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinAmount : MonoBehaviour
{
    public GameObject coinText;

    void Update()
    {
        coinText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Coin").ToString();
    }
}
