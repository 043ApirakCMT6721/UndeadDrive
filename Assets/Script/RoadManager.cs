using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour
{
    [Header("Road Prefabs")]
    public GameObject roadStraight;
    public GameObject roadLeft;
    public GameObject roadRight;

    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs;
    public float obstacleSpawnChance = 0.5f;

    [Header("General Settings")]
    public Transform player;
    public int startRoadCount = 5;
    public float spawnDistance = 30f;
    public int maxRoads = 8;

    private Transform lastExitPoint;
    private List<GameObject> activeRoads = new List<GameObject>();

    void Start()
    {
        GameObject firstRoad = Instantiate(roadStraight, Vector3.zero, Quaternion.identity);
        activeRoads.Add(firstRoad);

        Transform exit = firstRoad.transform.Find("ExitPoint");

        if (exit == null)
        {
            Debug.LogError("ExitPoint missing in first road!");
            return;
        }

        lastExitPoint = exit;

        for (int i = 0; i < startRoadCount; i++)
        {
            SpawnRoad();
        }
    }

    void Update()
    {
        if (player == null || lastExitPoint == null)
            return;

        if (Vector3.Distance(player.position, lastExitPoint.position) < spawnDistance)
        {
            SpawnRoad();
            RemoveOldRoad();
        }
    }

    void SpawnRoad()
    {
        GameObject randomRoad = GetRandomRoad();

        Quaternion fixedRotation = Quaternion.Euler(
            0,
            lastExitPoint.eulerAngles.y,
            0
        );

        GameObject newRoad = Instantiate(
            randomRoad,
            lastExitPoint.position,
            fixedRotation
        );

        activeRoads.Add(newRoad);

        SpawnObstacles(newRoad);

        Transform exit = newRoad.transform.Find("ExitPoint");

        if (exit == null)
        {
            Debug.LogError("ExitPoint not found!");
            return;
        }

        lastExitPoint = exit;
    }

    GameObject GetRandomRoad()
    {
        int rand = Random.Range(0, 100);

        if (rand < 60) return roadStraight;
        if (rand < 80) return roadLeft;
        return roadRight;
    }

    void SpawnObstacles(GameObject road)
    {
        Transform spawnParent = road.transform.Find("ObstacleSpawnPoints");
        if (spawnParent == null) return;

        foreach (Transform point in spawnParent)
        {
            if (Random.value < obstacleSpawnChance)
            {
                int rand = Random.Range(0, obstaclePrefabs.Length);

                Instantiate(
                    obstaclePrefabs[rand],
                    point.position,
                    point.rotation,
                    road.transform
                );
            }
        }
    }

    void RemoveOldRoad()
    {
        if (activeRoads.Count > maxRoads)
        {
            Destroy(activeRoads[0]);
            activeRoads.RemoveAt(0);
        }
    }
}