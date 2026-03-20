using UnityEngine;
using System.Collections;
public class PlayerExplosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public GameObject carModel;
    public AudioSource explosionSound; // 🔊 เพิ่มตรงนี้
    public void Explode()
    {
        // 💥 เอฟเฟก
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        // 🔊 เสียงระเบิด
        if (explosionSound != null)
        {
            explosionSound.Play();
        }
        // 🚗 ซ่อนรถ
        if (carModel != null)
            carModel.SetActive(false);
    }
}