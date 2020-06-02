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

    public float fireRate = 15f;
    private float nextTimeToFire = 0f;

    public AudioClip gunShoot;
    public AudioClip noAmmo;
    public AudioClip reload;
    private AudioSource gunSound;
    public float SFXVol = 1f;
    public float volFix = 5f;
    public GameManager gameManager;
    public bool isReloading = false;

    public Camera mainCamera;
    private float defaultFOV;
    public float scoppedFOV = 15.0f;
    public GameObject scop;
    public bool aimed = false;
    public float scoppedMS = 10;
    private float defaultMS;
    private MouseLook mouseLook;

   void Start()
   {
        gunSound = GetComponent<AudioSource>();
        mouseLook = GetComponentInParent<MouseLook>();
        
   }
    // Update is called once per frame
    void Update()
    {


        if (Time.time >= nextTimeToFire && ammo > 0 && !isReloading)
        {
            if (Input.GetButton("Fire1") && gameObject.tag == "Automatic")
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                ammo -= 1;
                StartCoroutine(Shoot());
            }

            if (Input.GetButtonDown("Fire1") && gameObject.tag == "SemiAuto")
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                ammo -= 1;
                StartCoroutine(Shoot());
            }

            if (Input.GetButtonDown("Fire1") && gameObject.tag == "Rifle")
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                ammo -= 1;
                StartCoroutine(Shoot());
                
            }

            
        }

        if (Input.GetButtonDown("Fire1") && ammo == 0 && !isReloading)
        {
            gunSound.PlayOneShot(noAmmo, SFXVol * volFix);
        }



        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());

        }

        if (Input.GetButtonDown("Fire2"))
        {
            Scop();   

        }

    }


    IEnumerator Shoot()
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

            yield return new WaitForSeconds(0.5f);
            if (aimed)
            {
                Scop();
            }

        }

    }

    IEnumerator Reload()
    {        

        PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
        if (gameObject.tag == "SemiAuto" && playerMovement.pistolMag > 0 && maxAmmo > ammo)
        {
            if (aimed)
            {
                Scop();
            }

            isReloading = true;
            gunSound.PlayOneShot(reload, SFXVol * volFix);
            ammo = 0;
            yield return new WaitForSeconds(reloadCooldown);
            ammo += (maxAmmo - ammo);
            playerMovement.pistolMag -= 1;
            isReloading = false;

        }

        if (gameObject.tag == "Automatic" && playerMovement.carbineMag > 0 && maxAmmo > ammo)
        {
            if (aimed)
            {
                Scop();
            }

            isReloading = true;
            gunSound.PlayOneShot(reload, SFXVol * volFix);
            ammo = 0;
            yield return new WaitForSeconds(reloadCooldown);
            ammo += (maxAmmo - ammo);
            playerMovement.carbineMag -= 1;
            isReloading = false;
        }

        if (gameObject.tag == "Rifle" && playerMovement.rifleMag > 0 && maxAmmo > ammo)
        {
            if (aimed)
            {
                Scop();
            }

            isReloading = true;
            gunSound.PlayOneShot(reload, SFXVol * volFix);
            ammo = 0;
            yield return new WaitForSeconds(reloadCooldown);
            ammo += (maxAmmo - ammo);
            playerMovement.rifleMag -= 1;
            isReloading = false;
        }

    }

    
    IEnumerator Flashlight()
    {
        gunFlashLight.SetActive(true);
        yield return new WaitForSeconds(gunFlashTime);
        gunFlashLight.SetActive(false);
    }

    void Scop()
    {
        if (gameObject.name == "L96_Rifle")
        {
            if (!aimed)
            {
                scop.SetActive(true);
                aimed = true;
                defaultFOV = mainCamera.fieldOfView;
                mainCamera.fieldOfView = scoppedFOV;
                defaultMS = mouseLook.mouseSensitivity;
                mouseLook.mouseSensitivity = scoppedMS;
            }

            else if (aimed)
            {
                scop.SetActive(false);
                aimed = false;
                mainCamera.fieldOfView = defaultFOV;
                mouseLook.mouseSensitivity = defaultMS;
            }
        }

        
    }

}
