using UnityEngine;
using UnityEngine.SceneManagement;
public class GameUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
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
    // ปุ่ม Start Game
    public void StartGame()
    {
        if (startPanel != null)
            startPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    // Pause เกม
    public void PauseGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    // เล่นต่อ
    public void ContinueGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    // Game Over
    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (gameOverSound != null)
            gameOverSound.Play();
        Time.timeScale = 0f;
    }
    // Restart เกม
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Quit เกม
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}