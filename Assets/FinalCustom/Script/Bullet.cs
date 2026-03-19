using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 80f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    void Update()
    {
        if (rb == null)
        {
            return;
        }
        rb.linearVelocity = transform.forward * speed;

    }
}