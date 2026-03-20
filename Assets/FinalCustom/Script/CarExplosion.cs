using UnityEngine;
public class PlayerExplosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public GameObject carModel; // 🚗 ตัวโมเดลรถ
    public void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        // ❌ ปิดแค่ตัวรถ ไม่ใช่ทั้ง Player
        if (carModel != null)
            carModel.SetActive(false);
    }
}