using UnityEngine;
using System.Collections;
using TMPro; // สำหรับ UI กระสุน

public class Gun : MonoBehaviour
{
    public Camera cam;
    public float range = 100f;
    public float fireRate = 25f;
    public int maxAmmo = 60; // ปรับเป็น 60 ตามที่ต้องการ
    public int currentAmmo;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSound;
    public AudioClip reloadSound; // 🔊 ช่องใส่เสียงรีโหลด (เพิ่มใหม่)

    [Header("UI Settings")]
    public TextMeshProUGUI ammoText; // 👈 ลาก Text ในหน้า Inspector มาใส่ช่องนี้

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
        UpdateAmmoUI(); // อัปเดตเลขกระสุนตอนเริ่มเกม

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

        // รีโหลดเมื่อกด R เท่านั้น (ห้ามรีโหลดเอง)
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        // ยิงได้ต่อเมื่อมีกระสุน (currentAmmo > 0)
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI(); // อัปเดตเลขทุกครั้งที่ยิง

        if (muzzleFlash != null)
            muzzleFlash.Play();

        // ระบบเสียงปืน (แบบสุ่ม Pitch ของเพื่อน)
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

        // ระบบยิงกระสุนโมเดล (ของคุณ)
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

                // ป้องกันกระสุนชนรถ
                Collider bulletCol = bullet.GetComponent<Collider>();
                Collider[] carCols = GetComponentsInParent<Collider>();
                foreach (Collider col in carCols)
                {
                    if (bulletCol != null && col != null) Physics.IgnoreCollision(bulletCol, col);
                }
            }
        }

        // ระบบ Raycast ไว้จัดการ Zombie
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

        // 🔊 เล่นเสียงรีโหลด (ถ้าใส่ไฟล์เสียงไว้)
        if (gunSound != null && reloadSound != null)
        {
            gunSound.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        UpdateAmmoUI(); // เติมกระสุนเต็ม 60
        isReloading = false;
        Debug.Log("Reloaded!");
    }

    // ฟังก์ชันอัปเดต UI
    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }
}