using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    //Enemys.
    public GameObject[] enemys;
    public int enemyAddTime; //Eklenme süresi.
    Player player;
    int extra = 0;
    bool startTime = false;
    int j = 1;

    private void Start()
    {
        player = GetComponent<Player>();
        StartCoroutine(addEnemys());
    }

    IEnumerator addEnemys()
    {
        if (!startTime)
        {
            yield return new WaitForSeconds(3);
            startTime = true;
        }

        for (int i = 0; i < Random.Range(1, player.roadCount); i++)
            Instantiate(enemys[Random.Range(0, enemys.Length)]);

        yield return new WaitForSeconds(enemyAddTime);

        if (player.score >= 25 * j && player.score != 0)
        {
            if (player.roadCount < 5)
            {
                player.roadCount++;
                player.RoadUpdate();
            }
            else if (player.minSlow != 1)
                player.minSlow--;

            j++;
        }


        StartCoroutine(addEnemys());
    }
}
