using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public Transform player;
    public float speed = 6f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }
}