using UnityEngine;

public class smoke : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        transform.Translate(move * speed * Time.deltaTime, 0, 0);
    }
}