using UnityEngine;

public class CarSpeed : MonoBehaviour
{
    public Rigidbody rb;

    public float speedKmh;

    void Update()
    {
        if (rb != null)
        {
            // แปลงความเร็วจาก m/s เป็น km/h
            speedKmh = rb.linearVelocity.magnitude * 3.6f;
        }
    }

    public float GetSpeed()
    {
        return speedKmh;
    }
}