using UnityEngine;
using TMPro;

public class CarDistance : MonoBehaviour
{
    public Transform car;
    public TextMeshProUGUI distanceText;

    [Header("High Score UI")]
    public TextMeshProUGUI highScoreText;

    private Vector3 startPosition;
    private float distance;

    private float highScore;

    void Start()
    {
        if (car != null)
        {
            startPosition = car.position;
        }

        // โหลด High Score
        highScore = PlayerPrefs.GetFloat("HighScore", 0);

        UpdateHighScoreUI();
    }

    void Update()
    {
        if (car == null) return;

        distance = Vector3.Distance(startPosition, car.position);

        distanceText.text = "Distance: " + Mathf.FloorToInt(distance) + " m";
    }

    public float GetDistance()
    {
        return distance;
    }

    public void SaveHighScore()
    {
        // ถ้าระยะใหม่มากกว่า → เซฟ
        if (distance > highScore)
        {
            highScore = distance;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();

            Debug.Log("NEW HIGH SCORE: " + highScore);
        }

        UpdateHighScoreUI();
    }

    void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "Best: " + Mathf.FloorToInt(highScore) + " m";
        }
    }
}