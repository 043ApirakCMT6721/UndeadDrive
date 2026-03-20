using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
public class GameUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    [Header("Distance UI")]
    public TextMeshProUGUI finalDistanceText;
    [Header("Sounds")]
    public AudioSource gameOverSound;
    [Header("Explosion")] // 💥 เพิ่มตรงนี้
    public PlayerExplosion playerExplosion;
    bool isPaused = false;
    bool isGameOver = false; // 🔥 กันเรียกซ้ำ
    void Start()
    {
        Time.timeScale = 0f;
        if (startPanel != null)
            startPanel.SetActive(true);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1f)
            {
                PauseGame();
            }
            else if (isPaused)
            {
                ContinueGame();
            }
        }
    }
    // ▶️ Start Game
    public void StartGame()
    {
        if (startPanel != null)
            startPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    // ⏸ Pause
    public void PauseGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    // ▶️ Continue
    public void ContinueGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    // 💀 Game Over
    public void GameOver()
    {
        if (isGameOver) return; // 🔥 กันซ้ำ
        isGameOver = true;
        StartCoroutine(GameOverDelay());
    }
    // ⏳ ดีเลย์ + 💥 ระเบิด
    IEnumerator GameOverDelay()
    {
        // 💥 ระเบิดทันทีตอนตาย
        if (playerExplosion != null)
            playerExplosion.Explode();
        // ⏳ หน่วงแบบไม่โดน TimeScale
        yield return new WaitForSecondsRealtime(1.7f);
        // 💀 ค่อยขึ้น Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (gameOverSound != null)
            gameOverSound.Play();
        Time.timeScale = 0f;
    }
    // แสดงระยะทาง
    public void ShowDistance(float distance)
    {
        if (finalDistanceText != null)
        {
            finalDistanceText.text = "Distance: " + Mathf.FloorToInt(distance) + " m";
        }
    }
    // 🔁 Restart
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // ❌ Quit
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}