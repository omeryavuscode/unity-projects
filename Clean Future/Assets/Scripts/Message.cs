using UnityEngine;

public class Message : MonoBehaviour
{
    Vector3 whitePos;
    Vector3 target;
    Animation anim;
    public int speed;

    void Start()
    {
        anim = GetComponent<Animation>();
        whitePos = GameObject.FindGameObjectWithTag("White").transform.position;
        target = new Vector3(whitePos.x + ((whitePos.x - transform.position.x) * 2), whitePos.y + ((whitePos.y - transform.position.y) * 2), 0);
    }

    void Update()
    {
        if (target != transform.position)
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        else
            gameObject.SetActive(false);
    }
}
