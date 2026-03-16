using UnityEngine;

public class Gun : MonoBehaviour
{
    public Camera cam;
    public float range = 100f;
    public float fireRate = 10f;

    public int maxAmmo = 30;
    public int currentAmmo;

    public ParticleSystem muzzleFlash;

    float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
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

        if (muzzleFlash != null)
            muzzleFlash.Play();

        RaycastHit hit;

        // เส้น debug ยิงออกจากกล้อง
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