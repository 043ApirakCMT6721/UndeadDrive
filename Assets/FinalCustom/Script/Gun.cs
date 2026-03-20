using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Camera cam;
    public float range = 100f;
    public float fireRate = 25f;
    public int maxAmmo = 30;
    public int currentAmmo;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSound;   // 🔊 เสียงปืน

    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float bulletForce = 2000f; // ใช้ค่าแรงกระสุนจากเพื่อน

    public float reloadTime = 1.5f;
    bool isReloading = false;
    float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
        // 🔇 กันเสียงเล่นตอนเริ่มเกม (เอามาจาก main)
        if (gunSound != null)
        {
            gunSound.Stop();
            gunSound.playOnAwake = false;
        }
    }

    void Update()
    {
        if (isReloading)
            return;

        // ระบบ Reload (เอามาจาก HEAD)
        if (Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;

        // 🔥 เอฟเฟคปากกระบอก
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // 🔊 ระบบเสียงปืนแบบเทพของเพื่อน (สร้าง Object ชั่วคราวเพื่อให้เสียงไม่ขาด)
        if (gunSound != null && gunSound.clip != null)
        {
            float randomPitch = Random.Range(0.9f, 1.1f);
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = transform.position;
            AudioSource audio = tempAudio.AddComponent<AudioSource>();
            audio.clip = gunSound.clip;
            audio.pitch = randomPitch;
            audio.volume = 0.05f;
            audio.Play();
            Destroy(tempAudio, gunSound.clip.length);
        }

        // 💥 ยิงกระสุนโมเดล
        if (bulletPrefab != null && firePoints != null)
        {
            foreach (Transform point in firePoints)
            {
                GameObject bullet = Instantiate(bulletPrefab, point.position, point.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                
                if (rb != null)
                {
                    // ระบบคำนวณความเร็วรถ (เอามาจาก HEAD)
                    Rigidbody carRb = GetComponentInParent<Rigidbody>();
                    if (carRb != null)
                        rb.linearVelocity = point.forward * bulletForce + carRb.linearVelocity;
                    else
                        rb.linearVelocity = point.forward * bulletForce;
                }

                // ป้องกันกระสุนชนรถตัวเอง (เอามาจาก HEAD)
                Collider bulletCol = bullet.GetComponent<Collider>();
                Collider[] carCols = GetComponentsInParent<Collider>();
                foreach (Collider col in carCols)
                {
                    if (bulletCol != null && col != null)
                        Physics.IgnoreCollision(bulletCol, col);
                }
            }
        }

        // 🎯 ยิงแบบ Raycast (เอาไว้จัดการ Zombie)
        RaycastHit hit;
        Debug.DrawRay(cam.transform.position, cam.transform.forward * range, Color.red, 2f);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            ZombieLookPlayer zombie = hit.transform.GetComponentInParent<ZombieLookPlayer>();
            if (zombie != null)
            {
                zombie.Die();
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded!");
    }
}