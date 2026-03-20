using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 80f;
    Rigidbody rb;

    // ในสคริปต์ Bullet
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed; // ใส่แรงครั้งเดียวจบ
        Destroy(gameObject, 3f);
    }

    // ลบ void Update() ของกระสุนทิ้งไปเลย

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}