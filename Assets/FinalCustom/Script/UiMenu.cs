using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // ✅ เพิ่มอันนี้

public class GameUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    [Header("Distance UI")] // ✅ เพิ่มส่วนนี้
    public TextMeshProUGUI finalDistanceText;

    [Header("Sounds")]
    public AudioSource gameOverSound;

    bool isPaused = false;

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
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverSound != null)
            gameOverSound.Play();

        Time.timeScale = 0f;
    }

    // ✅ ฟังก์ชันนี้แหละที่แก้ error
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