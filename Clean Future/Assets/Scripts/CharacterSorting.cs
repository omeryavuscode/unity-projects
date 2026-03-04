using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSorting : MonoBehaviour
{
    public List<Transform> transforms;

    private void Update()
    {
        //Sahnedeki tüm nesneleri transform listesine ekler
        transforms = new List<Transform>(FindObjectsOfType<Transform>());

        //Transform listesini yüksekliðe göre sýralar
        transforms.Sort((t1, t2) => t2.position.y.CompareTo(t1.position.y));

        //Sýralanmýþ Transform listesindeki her bir nesnenin order deðerini ayarlar
        for (int i = 0; i < transforms.Count; i++)
        {
            SpriteRenderer spriteRenderer = transforms[i].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = i;
            }
        }
    }
}

