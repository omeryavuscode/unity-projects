using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Event : MonoBehaviour
{
    string text = "";

    TextMeshProUGUI mainText;

    private void Start()
    {
        mainText = GetComponent<TextMeshProUGUI>();
    }

    public void EventText(string _text, Color color)
    {
        if (mainText.text != "")
            mainText.text = "";

        text = _text;
        StartCoroutine(Events(color));
    }

    IEnumerator Events(Color color)
    {
        mainText.text = "";
        mainText.color = color;
        foreach (char c in text)
        {
            mainText.text += c;
            yield return new WaitForSeconds(0.1f);
        }
        mainText.text = text;
        yield return new WaitForSeconds(1f);

        mainText.text = "";
    }
}
