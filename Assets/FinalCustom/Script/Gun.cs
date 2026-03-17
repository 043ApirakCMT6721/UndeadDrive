using UnityEngine;

public class Gun : MonoBehaviour
{
    public Camera cam;
    public float range = 100f;
    public float fireRate = 10f;
    public int maxAmmo = 30;
    public int currentAmmo;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSound;   // เสียงปืน

// เพิ่มสำหรับยิงกระสุนโมเดล
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float bulletForce = 2000f;

    float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;

        // กันไม่ให้เสียงเล่นตอนเริ่มเกม
        if (gunSound != null)
            gunSound.Stop();
    }

    void Update()
    {
        if (currentAmmo <= 0)
            return;

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Debug.Log("Shoot!");
        currentAmmo--;

        // เอฟเฟคปากกระบอกปืน
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // เล่นเสียงเฉพาะตอนยิง
        if (gunSound != null)
            gunSound.PlayOneShot(gunSound.clip);

        // เพิ่ม: ยิงโมเดลกระสุนออกจากปืน
        if (bulletPrefab != null && firePoints != null)
            foreach (Transform point in firePoints)
            {
                GameObject bullet = Instantiate(bulletPrefab, point.position, point.rotation);

                Rigidbody rb = bullet.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddForce(point.forward * bulletForce, ForceMode.Impulse);
                }

                Destroy(bullet, 5f);
            }

        RaycastHit hit;

        Debug.DrawRay(cam.transform.position, cam.transform.forward * range, Color.red, 2f);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            ZombieLookPlayer zombie = hit.transform.GetComponentInParent<ZombieLookPlayer>();

            if (zombie != null)
            {
                zombie.Die();
            }
        }
    }

}
