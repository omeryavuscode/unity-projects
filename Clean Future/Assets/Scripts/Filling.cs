using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Filling : MonoBehaviour
{
    public int filling;

    private void Start()
    {
        StartCoroutine(Passive());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "White")
        {
            // Item alýnma sesi çalar.
            collision.gameObject.GetComponent<White>().Items();
            
            // Yeni eklenecek item ile birlikte karakterin dolumu max caný geçmiyorsa can eklenir.
            if (name.Contains("Health"))
            {
                if (collision.gameObject.GetComponent<White>().health + filling <= collision.gameObject.GetComponent<White>().maxHealth)
                    collision.gameObject.GetComponent<White>().health += filling;
                else
                    collision.gameObject.GetComponent<White>().health = collision.gameObject.GetComponent<White>().maxHealth;
            }
            else if (name.Contains("Power"))
            {
                collision.gameObject.GetComponent<White>().maxCollect += filling;
            }
            else if (name.Contains("Armor"))
            {
                GameObject.FindGameObjectWithTag("White").GetComponent<White>().Armor();
            }

            // Yok olur.
            gameObject.SetActive(false);
        }
    }

    IEnumerator Passive()
    {
        // Canlandýktan 5 saniye sonra yok olur.
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
