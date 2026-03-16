using UnityEngine;

public class ZombieLookPlayer : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player);
        }
    }

    // เพิ่มฟังก์ชันตาย
    public void Die()
    {
        Destroy(gameObject);
    }
}