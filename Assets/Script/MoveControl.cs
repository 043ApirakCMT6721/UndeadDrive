using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;            // ความเร็วปกติ
    public float boostMultiplier = 2f;  // ตัวคูณความเร็วตอนกด Shift
    public float turnSpeed = 120f;      // ความเร็วการเลี้ยว

    void Update()
    {
        // เช็กว่ากด Shift อยู่ไหม
        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = speed * boostMultiplier;
        }

        // เดินหน้า
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }

        // ถอยหลัง
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * currentSpeed * Time.deltaTime);
        }

        // เลี้ยวซ้าย
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -turnSpeed * Time.deltaTime);
        }

        // เลี้ยวขวา
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
        }
    }
}