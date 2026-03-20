using UnityEngine;
using System.Collections;
using TMPro; // 👈 เพิ่มตัวนี้เพื่อใช้ TextMeshPro

public class Gun : MonoBehaviour
{
    public Camera cam;
    public float range = 100f;
    public float fireRate = 25f;
    public int maxAmmo = 60; // 👈 เปลี่ยนเป็น 60 ตามที่ต้องการ
    public int currentAmmo;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSound;

    [Header("UI Settings")]
    public TextMeshProUGUI ammoText; // 👈 ลาก Object AmmoText มาใส่ในช่องนี้ใน Inspector

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float bulletForce = 2000f;

    public float reloadTime = 1.5f;
    bool isReloading = false;
    float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI(); // 👈 อัปเดต UI ตอนเริ่มเกม

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

        
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI(); // 👈 อัปเดต UI ทุกครั้งที่ยิง

        if (muzzleFlash != null)
            muzzleFlash.Play();

        // ระบบเสียง
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

        // ระบบยิงกระสุน
        if (bulletPrefab != null && firePoints != null)
        {
            foreach (Transform point in firePoints)
            {
                GameObject bullet = Instantiate(bulletPrefab, point.position, point.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Rigidbody carRb = GetComponentInParent<Rigidbody>();
                    rb.linearVelocity = point.forward * bulletForce + (carRb != null ? carRb.linearVelocity : Vector3.zero);
                }

                // Ignore Collision
                Collider bulletCol = bullet.GetComponent<Collider>();
                Collider[] carCols = GetComponentsInParent<Collider>();
                foreach (Collider col in carCols)
                    if (bulletCol != null && col != null) Physics.IgnoreCollision(bulletCol, col);
            }
        }

        
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            ZombieLookPlayer zombie = hit.transform.GetComponentInParent<ZombieLookPlayer>();
            if (zombie != null) zombie.Die();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // ตรงนี้สามารถใส่เสียงรีโหลดเพิ่มได้
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        UpdateAmmoUI(); // 👈 อัปเดต UI หลังรีโหลดเสร็จ
        isReloading = false;
        Debug.Log("Reloaded!");
    }

    // ฟังก์ชันสำหรับอัปเดตข้อความบนหน้าจอ
    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }
}