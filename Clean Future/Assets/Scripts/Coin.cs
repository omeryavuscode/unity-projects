using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "White")
        {
            GameObject.FindObjectOfType<White>().Coin();
            coin = PlayerPrefs.GetInt("Coin") + 1;
            PlayerPrefs.SetInt("Coin", coin);
            gameObject.SetActive(false);
        }
    }
}
