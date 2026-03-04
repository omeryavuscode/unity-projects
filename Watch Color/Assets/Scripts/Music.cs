using UnityEngine;

public class Music : MonoBehaviour
{
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Music").Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
}
