using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float stopDistance = 2f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            // หันหน้าเข้าหาผู้เล่น
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            transform.rotation = Quaternion.LookRotation(direction);

            // เดินเข้าไปหา
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}