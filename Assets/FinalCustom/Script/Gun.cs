using UnityEngine;
public class Gun : MonoBehaviour
{
    public Camera cam;
    public float range = 100f;
    public float fireRate = 10f;
    public int maxAmmo = 30;
    public int currentAmmo;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSound;   // 🔊 เสียงปืน
                                   // ยิงกระสุนโมเดล
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float bulletForce = 2000f;
    float nextTimeToFire = 0f;
    void Start()
    {
        currentAmmo = maxAmmo;
        // 🔇 กันเสียงเล่นตอนเริ่มเกม
        if (gunSound != null)
        {
            gunSound.Stop();
            gunSound.playOnAwake = false;
        }
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
        currentAmmo--;
        // 🔥 เอฟเฟคปากกระบอก
        if (muzzleFlash != null)
            muzzleFlash.Play();
        // 🔊 เสียงปืน (แก้ยิงรัวแล้วเสียงหาย)
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
                    rb.AddForce(point.forward * bulletForce);
                }
            }
        }
        // 🎯 ยิงแบบ Raycast
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
}