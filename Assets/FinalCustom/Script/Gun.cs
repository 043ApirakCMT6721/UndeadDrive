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
    public AudioSource gunSound;

    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float bulletForce = 800f;

    public float reloadTime = 1.5f;
    bool isReloading = false;

    float nextTimeToFire = 0f;

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (currentAmmo <= 0)
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
        Debug.Log("Shoot!");
        currentAmmo--;

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (gunSound != null)
            gunSound.PlayOneShot(gunSound.clip);

        if (bulletPrefab != null && firePoints != null)
            foreach (Transform point in firePoints)
            {
                GameObject bullet = Instantiate(bulletPrefab, point.position, point.rotation);

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Rigidbody carRb = GetComponentInParent<Rigidbody>();
                    if (carRb != null)
                        rb.linearVelocity = point.forward * bulletForce + carRb.linearVelocity;
                    else
                        rb.linearVelocity = point.forward * bulletForce;
                }

                Collider bulletCol = bullet.GetComponent<Collider>();
                Collider[] carCols = GetComponentsInParent<Collider>();

                foreach (Collider col in carCols)
                {
                    if (bulletCol != null && col != null)
                        Physics.IgnoreCollision(bulletCol, col);
                }
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