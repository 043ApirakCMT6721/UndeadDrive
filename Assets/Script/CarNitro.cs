using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarNitroSystem : MonoBehaviour
{
    [Header("Speed Settings")]
    public float acceleration = 15f;
    public float maxForwardSpeed = 20f;
    public float maxReverseSpeed = 10f;
    public float turnSpeed = 120f;
    public float naturalDeceleration = 10f;

    [Header("Nitro Settings")]
    public float nitro = 0f;
    public float maxNitro = 100f;
    public float nitroGainMultiplier = 2f;
    public float nitroUseRate = 25f;
    public float boostMultiplier = 1.8f;
    public float minNitroToBoost = 30f;

    [Header("Ground Check")]
    public float groundCheckDistance = 1.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("UI")]
    public Slider nitroSlider;
    public TextMeshProUGUI speedText;

    private float currentSpeed = 0f;
    private Vector3 lastPosition;
    private float currentSpeedKmh;
    private bool isBoosting = false;   // ✅ ตัวควบคุมบูสจริง

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        CheckGround();

        float moveInput = 0f;

        // 🚗 เร่งได้เฉพาะตอนติดพื้น
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
                moveInput = 1f;

            if (Input.GetKey(KeyCode.S))
                moveInput = -1f;

            currentSpeed += moveInput * acceleration * Time.deltaTime;
        }

        // 🧲 ลดความเร็วอัตโนมัติ
        if (moveInput == 0)
        {
            if (currentSpeed > 0)
                currentSpeed -= naturalDeceleration * Time.deltaTime;
            else if (currentSpeed < 0)
                currentSpeed += naturalDeceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxReverseSpeed, maxForwardSpeed);

        if (Mathf.Abs(currentSpeed) < 0.1f)
            currentSpeed = 0f;

        // 🚀 เริ่ม Boost (ต้องมีขั้นต่ำ 30)
        if (Input.GetKeyDown(KeyCode.LeftShift)
            && nitro >= minNitroToBoost
            && currentSpeed > 0
            && isGrounded)
        {
            isBoosting = true;
        }

        // หยุด Boost
        if (Input.GetKeyUp(KeyCode.LeftShift)
            || nitro <= 0
            || !isGrounded)
        {
            isBoosting = false;
        }

        float finalSpeed = currentSpeed;

        if (isBoosting)
        {
            finalSpeed *= boostMultiplier;
            nitro -= nitroUseRate * Time.deltaTime;
        }

        // เคลื่อนที่
        transform.Translate(Vector3.forward * finalSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, lastPosition);

        // 🔢 คำนวณความเร็ว km/h
        if (Time.deltaTime > 0)
        {
            float speedMS = distance / Time.deltaTime;
            currentSpeedKmh = speedMS * 3.6f;
        }

        if (speedText != null)
            speedText.text = Mathf.RoundToInt(currentSpeedKmh) + " km/h";

        // 🔋 เพิ่ม Nitro เฉพาะตอนติดพื้น และไม่บูส
        if (!isBoosting && Mathf.Abs(currentSpeed) > 0.1f && isGrounded)
        {
            nitro += distance * nitroGainMultiplier;
        }

        nitro = Mathf.Clamp(nitro, 0, maxNitro);

        // ↩️ เลี้ยวได้เฉพาะตอนติดพื้น
        if (Mathf.Abs(currentSpeed) > 0.1f && isGrounded)
        {
            float turn = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);
        }

        if (nitroSlider != null)
            nitroSlider.value = nitro / maxNitro;

        lastPosition = transform.position;
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );

        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
    }
}
