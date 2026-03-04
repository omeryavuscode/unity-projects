using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject targetObje;
    public float cameraSpeed;

    //Baþlangýç Kordinatlarý.
    Vector3 startPos;
    Quaternion startAngle;

    public bool isRunning = false;

    private void Start()
    {
        startPos = transform.position;
        startAngle = transform.rotation;
    }

    private void Update()
    {
        if (isRunning)
            Game();
        else
            Menu();
    }

    public void Menu()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetObje.GetComponent<MeshRenderer>().bounds.center.y + 10, targetObje.GetComponent<MeshRenderer>().bounds.center.z - 10), Time.deltaTime * cameraSpeed);
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    public void Game()
    {
        if (targetObje.transform.position.y > 3)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetObje.GetComponent<MeshRenderer>().bounds.center.y + 19, targetObje.GetComponent<MeshRenderer>().bounds.center.z), Time.deltaTime * cameraSpeed);
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * cameraSpeed);
            transform.rotation = startAngle;
        }
    }
}
