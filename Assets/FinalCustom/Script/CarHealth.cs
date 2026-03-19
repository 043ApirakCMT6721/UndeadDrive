using UnityEngine;

public class CarHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Damage Settings")]
    public float minSafeSpeed = 20f;
    public float damage = 20f;

    [Header("Effects")]
    public GameObject bloodEffect;
    public GameObject explosionEffect;

    [Header("UI")]
    public GameUIManager gameUIManager;

    Rigidbody rb;
    bool isDestroyed = false;

    CarNitroSystem carNitroSystem;
    CarDistance carDistance;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody>();
        carNitroSystem = GetComponent<CarNitroSystem>();

        // หา CarDistance ใน Scene
        carDistance = FindObjectOfType<CarDistance>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed) return;

        if (collision.gameObject.CompareTag("Zombie"))
        {
            float speed = carNitroSystem.GetSpeed();

            // เอฟเฟกเลือด
            if (bloodEffect != null)
            {
                Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.identity);
            }

            // 🚗 รถช้า → รถเสียเลือด
            if (speed < minSafeSpeed)
            {
                TakeDamage(damage);
            }
            // 💀 รถเร็ว → ซอมบี้ตาย
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

        // 💥 เอฟเฟกระเบิด
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 🛑 หยุดรถ
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        // ❌ ปิดระบบควบคุมรถ
        if (carNitroSystem != null)
        {
            carNitroSystem.enabled = false;
        }

        // 🏆💾 เซฟระยะทาง + High Score
        if (carDistance != null)
        {
            carDistance.SaveHighScore();   // เซฟสถิติ
            carDistance.enabled = false;   // หยุดนับระยะ
        }

        // 📊 แสดงระยะตอน Game Over
        if (gameUIManager != null && carDistance != null)
        {
            gameUIManager.ShowDistance(carDistance.GetDistance());
        }

        // 🧾 แสดง Game Over UI
        if (gameUIManager != null)
        {
            gameUIManager.GameOver();
        }

        Debug.Log("GAME OVER");
    }
}