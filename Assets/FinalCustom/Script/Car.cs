using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed = 5f;
    public ParticleSystem smoke;

    void Update()
    {
        float move = Input.GetAxis("Horizontal");

        // ให้รถเคลื่อนที่
        transform.Translate(move * speed * Time.deltaTime, 0, 0);

        // ถ้ารถวิ่ง
        if (move != 0)
        {
            if (!smoke.isPlaying)
                smoke.Play();
        }
        else
        {
            smoke.Stop();
        }
    }
}