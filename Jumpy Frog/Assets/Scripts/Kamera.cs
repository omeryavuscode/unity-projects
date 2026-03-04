using UnityEngine;

public class Kamera : MonoBehaviour
{
    //KAMERA
    float kamera_Hizi;
    public float kamera_HiziKatsayi;
    public GameObject kamera_Hedef;

    private void Update()
    {
        Takip();
    }

    public void Takip()
    {
        if (kamera_Hedef != null)
        {
            kamera_Hizi = (kamera_Hedef.transform.position.y - transform.position.y) * kamera_HiziKatsayi;

            transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(transform.position.x, kamera_Hedef.transform.position.y, transform.position.z), Time.deltaTime * kamera_Hizi);
        }
    }
}
    