using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 80f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;

        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}