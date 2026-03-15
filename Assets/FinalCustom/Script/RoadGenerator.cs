using UnityEngine;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour
{
    [Header("Road Settings")]
    public GameObject[] roadPrefabs;
    public Transform playerTransform;

    public int numberOfRoads = 10;
    public float roadLength = 10f;

    private Vector3 nextSpawnPoint;
    private List<GameObject> activeRoads = new List<GameObject>();


    [Header("Zombie Spawn")]
    public GameObject[] zombiePrefabs;
    public int maxZombies = 3;

    public float roadHalfWidth = 4f;
    public float sideOffset = 12f;

    private List<GameObject> zombies = new List<GameObject>();


    [Header("Obstacle Spawn")]
    public GameObject[] obstaclePrefabs;
    public int maxObstacles = 4;

    public float obstacleCheckRadius = 2.5f;

    private List<GameObject> obstacles = new List<GameObject>();


    void Start()
    {
        nextSpawnPoint = transform.position;

        SetupFog();

        for (int i = 0; i < numberOfRoads; i++)
        {
            SpawnRoad();
        }
    }


    void Update()
    {
        if (playerTransform.position.z > nextSpawnPoint.z - (numberOfRoads * roadLength))
        {
            SpawnRoad();
        }

        DeleteOldRoad();
        DeleteFarObjects();
    }


    void SpawnRoad()
    {
        int randomIndex = Random.Range(0, roadPrefabs.Length);
        GameObject roadPrefab = roadPrefabs[randomIndex];

        GameObject road = Instantiate(roadPrefab, nextSpawnPoint, Quaternion.identity);

        activeRoads.Add(road);

        SpawnZombies();
        SpawnObstacles();

        nextSpawnPoint += new Vector3(0, 0, roadLength);
    }


    void SpawnZombies()
    {
        int zombieCount = Random.Range(0, maxZombies + 1);

        for (int i = 0; i < zombieCount; i++)
        {
            Vector3 pos = GetSideSpawnPosition();

            GameObject zombiePrefab =
                zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];

            GameObject zombie = Instantiate(zombiePrefab, pos, Quaternion.identity);

            zombie.transform.rotation =
                Quaternion.Euler(0, Random.Range(0, 360), 0);

            zombies.Add(zombie);
        }
    }


    void SpawnObstacles()
    {
        int obstacleCount = Random.Range(1, maxObstacles + 1);

        int attempts = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 pos;
            bool validPosition = false;

            while (!validPosition && attempts < 20)
            {
                attempts++;

                float x;

                int zone = Random.Range(0, 3);

                if (zone == 0)
                    x = Random.Range(-sideOffset, -roadHalfWidth);
                else if (zone == 1)
                    x = Random.Range(-roadHalfWidth, roadHalfWidth);
                else
                    x = Random.Range(roadHalfWidth, sideOffset);

                float z = Random.Range(-roadLength / 2, roadLength / 2);

                pos = nextSpawnPoint + new Vector3(x, 0, z);

                if (!Physics.CheckSphere(pos, obstacleCheckRadius))
                {
                    GameObject obstaclePrefab =
                        obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

                    GameObject obstacle =
                        Instantiate(obstaclePrefab, pos, Quaternion.identity);

                    obstacle.transform.rotation =
                        Quaternion.Euler(0, Random.Range(0, 360), 0);

                    obstacles.Add(obstacle);

                    validPosition = true;
                }
            }
        }
    }


    Vector3 GetSideSpawnPosition()
    {
        float x;

        if (Random.value > 0.5f)
            x = Random.Range(-sideOffset, -roadHalfWidth);
        else
            x = Random.Range(roadHalfWidth, sideOffset);

        float z = Random.Range(-roadLength / 2, roadLength / 2);

        return nextSpawnPoint + new Vector3(x, 0, z);
    }


    void DeleteOldRoad()
    {
        if (activeRoads.Count == 0) return;

        float distance =
            playerTransform.position.z - activeRoads[0].transform.position.z;

        if (distance > roadLength * 2)
        {
            Destroy(activeRoads[0]);
            activeRoads.RemoveAt(0);
        }
    }


    void DeleteFarObjects()
    {
        DeleteList(zombies);
        DeleteList(obstacles);
    }


    void DeleteList(List<GameObject> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null)
            {
                list.RemoveAt(i);
                continue;
            }

            float distance =
                playerTransform.position.z - list[i].transform.position.z;

            if (distance > 50f)
            {
                Destroy(list[i]);
                list.RemoveAt(i);
            }
        }
    }


    void SetupFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.gray;

        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 30f;
        RenderSettings.fogEndDistance = 120f;
    }
}