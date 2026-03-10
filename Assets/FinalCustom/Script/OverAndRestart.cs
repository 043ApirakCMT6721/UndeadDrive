using UnityEngine;
using UnityEngine.SceneManagement;

public class CarFlipGameOver : MonoBehaviour
{
    public float flipAngle = 60f;        // องศาที่ถือว่าคว่ำ
    public float flipTimeLimit = 5f;     // เวลาที่ต้องคว่ำค้าง (วินาที)

    private float flipTimer = 0f;
    private bool isGameOver = false;

    void Update()
    {
        // กด R เพื่อเริ่มใหม่
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (isGameOver) return;

        float angle = Vector3.Angle(transform.up, Vector3.up);

        if (angle > flipAngle)
        {
            flipTimer += Time.deltaTime;

            if (flipTimer >= flipTimeLimit)
            {
                GameOver();
            }
        }
        else
        {
            // รถกลับมาตั้งตรง → รีเซ็ตเวลา
            flipTimer = 0f;
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! (Flipped 5 seconds) Press R to Restart");

        // ถ้าใช้ Scene GameOver ให้เปิดบรรทัดนี้
        // SceneManager.LoadScene("GameOver");
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
