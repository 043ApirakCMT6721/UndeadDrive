using UnityEngine;

public class CarSpeed : MonoBehaviour
{
    public Rigidbody rb;

    public float speed;

    public float safeSpeed = 1.5f; // ความเร็วขั้นต่ำ ถ้าต่ำกว่านี้ถือว่าหยุด

    void Update()
    {
        if (rb != null)
        {
            // ใช้ความเร็วจาก Rigidbody ตรงๆ (m/s)
            speed = rb.linearVelocity.magnitude;
            Debug.Log(GetSpeed());
        }
    }

    public float GetSpeed()
    {
        return speed * 3.6f;
    }

    public bool CanTakeDamage()
    {
        // ถ้ารถช้าหรือหยุด จะโดนดาเมจได้
        return speed < safeSpeed;
    }
}