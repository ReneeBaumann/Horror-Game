using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Ammo")]
    public int maxAmmoInMag = 10;
    public int currentAmmoInMag;

    [Header("Cooldowns")]
    public float shootCooldown = 0.5f;
    private float shootTimer;

    [Header("Shoot Settings")]
    public float shootRange = 100f;
    public bool canShoot = true;

    [Header("FX")]
    public ParticleSystem muzzleFlash;
    public GameObject muzzleFlashLight;
    public AudioSource shootSound;
    public ParticleSystem impactEffect;
  
    void Start()
    {
        currentAmmoInMag = maxAmmoInMag;
        if (muzzleFlashLight) muzzleFlashLight.SetActive(false);
    }

    void Update()
    {
        currentAmmoInMag = Mathf.Clamp(currentAmmoInMag, 0, maxAmmoInMag);

        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            Shoot();
        }

        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        if (currentAmmoInMag > 0 && shootTimer <= 0f)
        {
            // Play FX
            if (shootSound) shootSound.Play();
            if (muzzleFlash) muzzleFlash.Play();
            if (muzzleFlashLight) muzzleFlashLight.SetActive(true);

            // Raycast to show shooting direction
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootRange))
            {
                if (impactEffect)
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

                // Check if we hit an enemy
                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(25); 
                }
            }


            currentAmmoInMag--;
            shootTimer = shootCooldown;

            StartCoroutine(EndShootFX());
        }
        else
        {
            Debug.Log("Click! (Empty or cooldown)");
        }
    }

    IEnumerator EndShootFX()
    {
        yield return new WaitForSeconds(0.1f);
        if (muzzleFlashLight) muzzleFlashLight.SetActive(false);
    }
}
