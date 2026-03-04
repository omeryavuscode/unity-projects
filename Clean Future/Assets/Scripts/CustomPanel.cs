using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomPanel : MonoBehaviour
{
    public Menu menu;

    private void Start()
    {
        menu = FindObjectOfType<Menu>();
        menu.customs = GameObject.FindGameObjectsWithTag("Custom").ToArray(); // Ekrandaki butonlar diziye eklenrir.

        // Butonlar spritelar ile giydirilir ve satýn alýnýlmýþ mý kontrolü yaparak boyar.
        for (int i = 0; i < menu.whiteCustoms.Length; i++)
        {
            menu.customs[i].gameObject.GetComponent<Image>().sprite = menu.whiteCustoms[i]; // Buton giydirme.

            if (PlayerPrefs.GetString("SelectedCustom") == menu.whiteCustoms[i].name) // Seçili kostüm de mi?
                menu.customs[i].GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
            else if (PlayerPrefs.GetInt(menu.customs[i].name) == 1) // Alýnmýþ mý?
                menu.customs[i].GetComponentInChildren<TextMeshProUGUI>().text = "Purchased";
            else if (PlayerPrefs.GetInt(menu.customs[i].name) == 0)
            { // Alýnmadýysa?
                menu.customs[i].GetComponentInChildren<TextMeshProUGUI>().text = menu.customsPay[i].ToString();
                menu.customs[i].gameObject.GetComponent<Image>().color = Color.black;
            }
        }
    }

    // Geri tuþu.
    public void Resume()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void Buy(GameObject w) // Basýlan tuþ.
    {
        for (int i = 0; i < menu.whiteCustoms.Length; i++) // Toplam tuþ kadar dönen döngü.
        {
            // Tuþun ismi menüdeki tuþlardan biriyse ve bu tuþ alýnabiliyorsa ve daha önce alýnmadýysa alýr.
            if (menu.customs[i].name == w.name && menu.customsPay[i] <= PlayerPrefs.GetInt("Coin") && PlayerPrefs.GetInt(w.name) != 1)
            {
                PlayerPrefs.SetInt(w.name, 1);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - menu.customsPay[i]);
                menu.customs[i].gameObject.GetComponent<Image>().sprite = menu.whiteCustoms[i];
                menu.customs[i].gameObject.GetComponent<Image>().color = (PlayerPrefs.GetInt(menu.customs[i].name) == 1) ? Color.white : Color.black;
                menu.customs[i].GetComponentInChildren<TextMeshProUGUI>().text = "Purchased";
                menu.sound.clip = menu.unlockS;
                menu.sound.Play();
            }
            // Tuþ alýnamýyorsa ve tuþ ismi menüdeki tuþlardan biriyse ve tuþ alýnmýþ ise tuþu seçer.
            else if (menu.customs[i].name == w.name && PlayerPrefs.GetInt(w.name) == 1)
            {
                PlayerPrefs.SetString("SelectedCustom", menu.whiteCustoms[i].name);
                menu.selectedCustom.GetComponent<Image>().sprite = menu.whiteCustoms[i];
                menu.sound.clip = menu.selectedS;
                menu.sound.Play();
            }
            else if (menu.customs[i].name == w.name && PlayerPrefs.GetInt(w.name) == 0)
            {
                menu.sound.clip = menu.lockS;
                menu.sound.Play();
            }
        }

        for (int i = 0; i < menu.whiteCustoms.Length; i++)
            if (PlayerPrefs.GetString("SelectedCustom") == menu.whiteCustoms[i].name)
                menu.customs[i].GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
            else if (PlayerPrefs.GetInt(menu.customs[i].name) == 1)
                menu.customs[i].GetComponentInChildren<TextMeshProUGUI>().text = "Purchased";
    }
}
