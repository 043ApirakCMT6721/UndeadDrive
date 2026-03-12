using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public float distanceTravelled = 0f;

    Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        distanceTravelled += distance;

        lastPosition = transform.position;
    }

    public float GetDistance()
    {
        return distanceTravelled;
    }
}