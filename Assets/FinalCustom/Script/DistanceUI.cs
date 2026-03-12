using UnityEngine;
using TMPro;

public class DistanceUI : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI distanceText;

    float distance;

    void Start()
    {
        distance = 0f;
    }

    void Update()
    {
        distance = player.position.z;

        distanceText.text = " " + Mathf.FloorToInt(distance) + " m";
    }
}