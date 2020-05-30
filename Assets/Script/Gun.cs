using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    public ParticleSystem muzzleFlash;
    public GameObject gunFlashLight;
    public float gunFlashTime = 0.15f;
    public GameObject impactEffect;
    public float impactForce = 300f;
    public int ammo = 0;
    public int maxAmmo;
    public float reloadCooldown = 3f;

    //SOLVE MULTIPLE VALUES FOR MAGS IN DIFFERENT INSTANCES - MAYBE CREATE A GameManager SCRIPT?

    public float fireRate = 15f;
    private float nextTimeToFire = 0f;

    public AudioClip gunShoot;
    public AudioClip noAmmo;
    public AudioClip reload;
    private AudioSource gunSound;
    public float SFXVol = 1f;
    public float volFix = 5f;
    public GameManager gameManager;

   void Start()
   {
        gunSound = GetComponent<AudioSource>();                        
   }
    // Update is called once per frame
    void Update()
    {


        if (Time.time >= nextTimeToFire && ammo > 0)
        {
            if (Input.GetButton("Fire1") && gameObject.tag == "Automatic")
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                ammo -= 1;
                Shoot();
            }

            if (Input.GetButtonDown("Fire1") && gameObject.tag == "SemiAuto")
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                ammo -= 1;
                Shoot();
            }

        }

        if (ammo == 0 && Input.GetButtonDown("Fire1"))
        {
            gunSound.PlayOneShot(noAmmo, SFXVol * volFix);
        }
            
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

    void Shoot()
    {
        if (!muzzleFlash.isPlaying)
        {
            muzzleFlash.Play();
        }

        StartCoroutine(Flashlight());
        gunSound.PlayOneShot(gunShoot, SFXVol);

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);

            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }

    }

    IEnumerator Reload()
    {
            PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();

            if (gameObject.tag == "SemiAuto" && playerMovement.pistolMag > 0 && maxAmmo > ammo)
            {
                gunSound.PlayOneShot(reload, SFXVol * volFix);
                ammo = 0;
                yield return new WaitForSeconds(reloadCooldown);
                ammo += (maxAmmo - ammo);
                playerMovement.pistolMag -= 1;
                
            }

            if (gameObject.tag == "Automatic" && playerMovement.carbineMag > 0 && maxAmmo > ammo)
            {
                gunSound.PlayOneShot(reload, SFXVol * volFix);
                ammo = 0;
                yield return new WaitForSeconds(reloadCooldown);
                ammo += (maxAmmo - ammo);
                playerMovement.carbineMag -= 1;
            }


        }

    }
    IEnumerator Flashlight()
    {
        gunFlashLight.SetActive(true);
        yield return new WaitForSeconds(gunFlashTime);
        gunFlashLight.SetActive(false);
    }



}
