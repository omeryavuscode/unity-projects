using UnityEngine;

public class Enemy : MonoBehaviour
{
    Player player;
    public Vector3 targetRoad;
    int targetRoadChoise;
    public float extraDistance;
    float slow = 1;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        AddedLocation();
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        RaycastHit2D raycast = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 1f), transform.up, 1f);
        if (raycast.collider != null && raycast.collider.tag == "Enemy" && slow < raycast.collider.GetComponent<Enemy>().slow)
            slow = raycast.collider.GetComponent<Enemy>().slow;

        transform.position = new Vector3(player.roadCoord[targetRoadChoise].x, transform.position.y);
        transform.localScale = new Vector3(player.scale, player.scale); //Boyut güncellemesi.
        if (transform.position != new Vector3(player.roadCoord[targetRoadChoise].x, targetRoad.y - extraDistance))
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetRoad.x, targetRoad.y - extraDistance), (player.spacingY / slow) * Time.deltaTime);
        else
        {
            Dead(); //Hedefe ulaþýrsa yok et.
            player.score++;
            player.CanvasControl();
        }
    }

    public void Dead()
    {
        gameObject.SetActive(false);
    }

    //Rastgele yol seçme ve yola atama.
    void AddedLocation()
    {
        slow = Random.Range(player.minSlow, 4);
        targetRoadChoise = Random.Range(0, player.roadCount);
        targetRoad = player.roadCoord[targetRoadChoise];
        transform.position = new Vector3(targetRoad.x, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).y + extraDistance);

        transform.localScale = new Vector3(player.scale, player.scale); //Boyut güncellemesi.
    }

}
