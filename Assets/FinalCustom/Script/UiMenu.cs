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
    [Header("Explosion")]
    public PlayerExplosion playerExplosion;
    bool isPaused = false;
    bool isGameOver = false;
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
                PauseGame();
            else if (isPaused)
                ContinueGame();
        }
    }
    public void StartGame()
    {
        if (startPanel != null)
            startPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PauseGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
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
        if (isGameOver) return;
        isGameOver = true;
        StartCoroutine(GameOverDelay());
    }
    // ⏳ ดีเลย์ + 💥 ระเบิด
    IEnumerator GameOverDelay()
    {
        // 💥 เรียกระเบิด (รวมเสียงอยู่ในนี้)
        if (playerExplosion != null)
            playerExplosion.Explode();
        yield return new WaitForSecondsRealtime(1.7f);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (gameOverSound != null)
            gameOverSound.Play();
        Time.timeScale = 0f;
    }
    public void ShowDistance(float distance)
    {
        if (finalDistanceText != null)
        {
            finalDistanceText.text = "Distance: " + Mathf.FloorToInt(distance) + " m";
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}