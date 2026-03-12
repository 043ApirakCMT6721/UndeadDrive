using UnityEngine;
using UnityEngine.UI;

public class CarHealthUI : MonoBehaviour
{
    public Slider healthSlider;
    public CarHealth carHealth;

    public Image fillImage;

    void Start()
    {
        healthSlider.maxValue = carHealth.maxHealth;
    }

    void Update()
    {
        float health = carHealth.currentHealth;

        healthSlider.value = health;

        float percent = health / carHealth.maxHealth;

        if (percent > 0.6f)
        {
            fillImage.color = Color.green;
        }
        else if (percent > 0.3f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.red;
        }
    }
}