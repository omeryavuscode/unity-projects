using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public GameObject target;
    public GameObject white;

    private void Update()
    {
        if (target != null)
            transform.position = Vector3.LerpUnclamped(white.transform.position, target.transform.position, 0.1f);
        //Debug.Log($"FPS : {1 / Time.deltaTime}");
    }
}
