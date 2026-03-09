using UnityEngine;
using System.Collections.Generic; // ใช้ List เก็บถนน

public class RoadGenerator : MonoBehaviour
{
    [Header("การตั้งค่าถนน")]
    public GameObject[] roadPrefabs;
    public Transform playerTransform; // ลากตัวผู้เล่นมาใส่

    public int numberOfRoads = 10;
    public float roadLength = 10f;
    public float safeDistance = 20f; // ระยะห่างก่อนลบถนน

    private Vector3 nextSpawnPoint;
    private List<GameObject> activeRoads = new List<GameObject>(); // เก็บถนนที่สร้างแล้ว

    void Start()
    {
        nextSpawnPoint = transform.position;

        // สร้างถนนเริ่มต้น
        for (int i = 0; i < numberOfRoads; i++)
        {
            SpawnRoad();
        }
    }

    void Update()
    {
        // 1. สร้างถนนเพิ่มเมื่อผู้เล่นใกล้ปลายถนน
        if (playerTransform.position.z > nextSpawnPoint.z - (numberOfRoads * roadLength))
        {
            SpawnRoad();
        }

        // 2. ลบถนนเก่าที่อยู่ไกลผู้เล่น
        if (activeRoads.Count > 0)
        {
            float distanceToOldRoad =
                playerTransform.position.z - activeRoads[0].transform.position.z;

            if (distanceToOldRoad > roadLength * 2)
            {
                Destroy(activeRoads[0]);
                activeRoads.RemoveAt(0);
            }
        }
    }

    public void SpawnRoad()
    {
        int randomIndex = Random.Range(0, roadPrefabs.Length);
        GameObject roadToSpawn = roadPrefabs[randomIndex];

        GameObject go = Instantiate(
            roadToSpawn,
            nextSpawnPoint,
            Quaternion.identity
        );

        activeRoads.Add(go);

        nextSpawnPoint += new Vector3(0, 0, roadLength);
    }

    private void DeleteOldRoad()
    {
        if (activeRoads.Count > 0)
        {
            Destroy(activeRoads[0]);
            activeRoads.RemoveAt(0);
        }
    }
}