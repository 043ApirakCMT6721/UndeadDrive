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

    [Header("Camera FOV")]
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float boostFOV = 80f;
    public float fovSpeed = 5f;

    [Header("UI")]
    public Slider nitroSlider;
    public Image nitroFillImage;   // 🔥 ลาก Fill Image มาใส่
    public TextMeshProUGUI speedText;

    [Header("Visual Effect")]
    public float glowSpeed = 4f;

    private float currentSpeed = 0f;
    private Vector3 lastPosition;
    private float currentSpeedKmh;
    private bool isBoosting = false;
    private float glowTimer = 0f;
    public float GetSpeed()
    {
        return currentSpeedKmh;
    }

    void Start()
    {
        lastPosition = transform.position;

        if (playerCamera != null)
            playerCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.W))
            moveInput = 1f;

        if (Input.GetKey(KeyCode.S))
            moveInput = -1f;

        currentSpeed += moveInput * acceleration * Time.deltaTime;

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

        // 🚀 เริ่ม Boost
        if (Input.GetKeyDown(KeyCode.LeftShift)
            && nitro >= minNitroToBoost
            && currentSpeed > 0)
        {
            isBoosting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)
            || nitro <= 0)
        {
            isBoosting = false;
        }

        float finalSpeed = currentSpeed;

        if (isBoosting)
        {
            finalSpeed *= boostMultiplier;
            nitro -= nitroUseRate * Time.deltaTime;
        }

        transform.Translate(Vector3.forward * finalSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, lastPosition);

        if (Time.deltaTime > 0)
        {
            float speedMS = distance / Time.deltaTime;
            currentSpeedKmh = speedMS * 3.6f;
        }

        if (speedText != null)
            speedText.text = Mathf.RoundToInt(currentSpeedKmh) + " km/h";

        if (!isBoosting && Mathf.Abs(currentSpeed) > 0.1f)
        {
            nitro += distance * nitroGainMultiplier;
        }

        nitro = Mathf.Clamp(nitro, 0, maxNitro);

        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turn = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);
        }

        // 🎨 NITRO UI EFFECT
        if (nitroSlider != null)
        {
            float nitroPercent = nitro / maxNitro;
            nitroSlider.value = nitroPercent;

            if (nitroFillImage != null)
            {
                Color normalColor = Color.Lerp(Color.red, Color.cyan, nitroPercent);

                if (isBoosting)
                {
                    glowTimer += Time.deltaTime * glowSpeed;
                    float glow = 1f + Mathf.Sin(glowTimer) * 0.4f;
                    nitroFillImage.color = normalColor * glow;
                }
                else if (nitro >= minNitroToBoost)
                {
                    glowTimer += Time.deltaTime * 2f;
                    float pulse = 0.85f + Mathf.Sin(glowTimer) * 0.15f;
                    nitroFillImage.color = normalColor * pulse;
                }
                else
                {
                    nitroFillImage.color = normalColor;
                }
            }
        }

        // 🎥 Smooth FOV
        if (playerCamera != null)
        {
            float targetFOV = isBoosting ? boostFOV : normalFOV;
            playerCamera.fieldOfView = Mathf.Lerp(
                playerCamera.fieldOfView,
                targetFOV,
                Time.deltaTime * fovSpeed
            );
        }

        lastPosition = transform.position;
    }
}