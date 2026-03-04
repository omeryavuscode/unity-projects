using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Camera : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Titreþim süresi
    public float shakeMagnitude = 0.1f; // Titreþim þiddeti

    private float timeLeft = 0f; // Geri sayým
    private Vector3 initialPosition; // Kamera pozisyonu

    public GameObject target;
    public int speed;

    private void Update()
    {
        if (target != null && transform.position.z >= -1)
            transform.position -= new Vector3(0, 0, Time.deltaTime);

        if (target != null)
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, transform.position.z), Time.deltaTime * speed);

        if (timeLeft > 0 && Time.timeScale == 1)
        {
            initialPosition = Vector3.Lerp(gameObject.transform.position, new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, transform.position.z), Time.deltaTime * speed);
            // Rastgele pozisyon belirle
            Vector2 shakePos = Random.insideUnitCircle * shakeMagnitude;

            // Kamera pozisyonunu rastgele pozisyon ile güncelle
            transform.position = new Vector3(initialPosition.x + shakePos.x, initialPosition.y + shakePos.y, initialPosition.z);

            // Geri sayýmý azalt
            timeLeft -= Time.deltaTime;
        }
    }

    public void Shake()
    {
        timeLeft = shakeDuration;
    }
}
