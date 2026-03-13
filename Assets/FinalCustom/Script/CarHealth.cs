using UnityEngine;

public class CarHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public float minSafeSpeed = 20f;
    public float damage = 20f;

    public GameObject bloodEffect;
    public GameObject explosionEffect;

    Rigidbody rb;
    bool isDestroyed = false;

    CarNitroSystem carNitroSystem;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody>();
        carNitroSystem = GetComponent<CarNitroSystem>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed) return;

        if (collision.gameObject.CompareTag("Zombie"))
        {
            float speed = carNitroSystem.GetSpeed();

            if (bloodEffect != null)
            {
                Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.identity);
            }

            // รถช้า → รถเสียเลือด
            if (speed < minSafeSpeed)
            {
                TakeDamage(damage);
            }
            // รถเร็ว → ซอมบี้ตาย
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }

    void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        Debug.Log("Car Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            DestroyCar();
        }
    }

    void DestroyCar()
    {
        isDestroyed = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // หยุดรถ
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        // ปิดระบบไนโตร / ควบคุมรถ
        if (carNitroSystem != null)
        {
            carNitroSystem.enabled = false;
        }

        Debug.Log("GAME OVER");
    }
}