using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public GameObject weaponCamera;

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
    public GameObject scop;
    public bool aimed = false;    
    private float defaultMS;
    private MouseLook mouseLook;
    private float defaultSpeed;
    public float scoppedSpeed = 4.5f;
    public float scoppedMS = 10;
    public float scoppedFOV = 30.0f;
    public float maxFOV = 5.0f;
    public float minFOV = 30.0f;
    private PlayerMovement playerMovement;
    public float shootPrecision;
    private float defaultshootPrecision;
    public GameObject crosshair;
    private Animator anim;
    public float aimTime;
    private float precisionIncrease = 0.25f;


   void Start()
   {
        gunSound = GetComponent<AudioSource>();
        mouseLook = GetComponentInParent<MouseLook>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        anim = GetComponent<Animator>();
   }

  void OnEnable()
  {
        isReloading = false;
  }

   
    // Update is called once per frame
    void Update()
    {
        if (aimed)
        {
            if (Input.mouseScrollDelta.y > 0 && mainCamera.fieldOfView >= maxFOV)
            {
                mainCamera.fieldOfView -= 1;
            }

            if (Input.mouseScrollDelta.y < 0 && mainCamera.fieldOfView <= minFOV)
            {
                mainCamera.fieldOfView += 1;
            }
        }

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

            if (Input.GetButtonDown("Fire1") && gameObject.tag == "Repetition")
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                ammo -= 1;
                StartCoroutine(Shoot());
                StartCoroutine(Shoot());
                StartCoroutine(Shoot());
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

        if (Input.GetButtonDown("Fire2") && !isReloading)
        {
            StartCoroutine(Scop()); 
        }

    }


    IEnumerator Shoot()
    {
        StartCoroutine(Flashlight());
        anim.SetTrigger("Shot");        

        if (gameObject.name == "870_Shotgun" && !gunSound.isPlaying)
        {
            gunSound.PlayOneShot(gunShoot, SFXVol);
        }

        else if (gameObject.name == "Pistol" || gameObject.name == "M4_Carbine" || gameObject.name == "L96_Rifle")
        {
            gunSound.PlayOneShot(gunShoot, SFXVol);
        }        

        if (!muzzleFlash.isPlaying)
        {
            muzzleFlash.Play();
        }

        float offsetY = Random.Range(-shootPrecision, shootPrecision);
        float offsetX = Random.Range(-shootPrecision, shootPrecision);
        Vector3 direction = fpsCam.transform.forward + new Vector3(offsetX, offsetY, offsetX);

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range))
        {
            //Debug.Log(hit.transform.name + fpsCam.transform.forward + direction);

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
            if (aimed && gameObject.name == "L96_Rifle")
            {
                StartCoroutine(Scop());
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
                StartCoroutine(Scop());
            }

            isReloading = true;
            gunSound.PlayOneShot(reload, SFXVol * volFix);            
            ammo = 0;
            crosshair.SetActive(false);
            anim.SetTrigger("Reload");
            yield return new WaitForSeconds(reloadCooldown);
            crosshair.SetActive(true);
            ammo += (maxAmmo - ammo);
            playerMovement.pistolMag -= 1;
            isReloading = false;

        }

        if (gameObject.tag == "Automatic" && playerMovement.carbineMag > 0 && maxAmmo > ammo)
        {
            if (aimed)
            {
                StartCoroutine(Scop());
            }

            isReloading = true;
            gunSound.PlayOneShot(reload, SFXVol * volFix);
            ammo = 0;
            crosshair.SetActive(false);
            anim.SetTrigger("Reload");
            yield return new WaitForSeconds(reloadCooldown);
            crosshair.SetActive(true);
            ammo += (maxAmmo - ammo);
            playerMovement.carbineMag -= 1;
            isReloading = false;
        }

        if (gameObject.tag == "Rifle" && playerMovement.rifleMag > 0 && maxAmmo > ammo)
        {
            if (aimed)
            {
                StartCoroutine(Scop());
            }

            isReloading = true;
            gunSound.PlayOneShot(reload, SFXVol * volFix);
            ammo = 0;
            anim.SetTrigger("Reload");
            yield return new WaitForSeconds(reloadCooldown);
            ammo += (maxAmmo - ammo);
            playerMovement.rifleMag -= 1;
            isReloading = false;
        }

        if (gameObject.tag == "Repetition" && playerMovement.shotgunMag > 0 && maxAmmo > ammo)
        {
            if (aimed)
            {
                StartCoroutine(Scop());
            }

            isReloading = true;
            gunSound.PlayOneShot(reload, SFXVol * volFix);
            ammo = 0;
            anim.SetTrigger("Reload");
            crosshair.SetActive(false);
            yield return new WaitForSeconds(reloadCooldown);
            crosshair.SetActive(true);
            ammo += (maxAmmo - ammo);
            playerMovement.shotgunMag -= 1;
            isReloading = false;
        }

    }

    
    IEnumerator Flashlight()
    {
        gunFlashLight.SetActive(true);
        yield return new WaitForSeconds(gunFlashTime);
        gunFlashLight.SetActive(false);
    }

    public IEnumerator Scop()
    {
        PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
        
        if (gameObject.name == "L96_Rifle")
        {
            if (!aimed)
            {
                anim.SetBool("Scoped", true);
                yield return new WaitForSeconds(aimTime);
                scop.SetActive(true);
                aimed = true;                
                defaultFOV = mainCamera.fieldOfView;
                mainCamera.fieldOfView = scoppedFOV;
                defaultMS = mouseLook.mouseSensitivity;
                mouseLook.mouseSensitivity = scoppedMS;
                weaponCamera.SetActive(false);
                defaultshootPrecision = shootPrecision;
                shootPrecision *= precisionIncrease;
                defaultSpeed = playerMovement.speed;
                playerMovement.speed = scoppedSpeed;
            }

            else if (aimed)
            {
                scop.SetActive(false);
                aimed = false;
                anim.SetBool("Scoped", false);
                mainCamera.fieldOfView = defaultFOV;
                mouseLook.mouseSensitivity = defaultMS;
                weaponCamera.SetActive(true);
                shootPrecision = defaultshootPrecision;
                playerMovement.speed = defaultSpeed;
            }
        }

        if (gameObject.name != "L96_Rifle")
        {
            

            if (!aimed)
            {
                aimed = true;                
                anim.SetBool("Scoped", true);
                defaultFOV = mainCamera.fieldOfView;
                mainCamera.fieldOfView = scoppedFOV;
                defaultMS = mouseLook.mouseSensitivity;
                mouseLook.mouseSensitivity = scoppedMS;
                defaultshootPrecision = shootPrecision;
                shootPrecision *= precisionIncrease;
                defaultSpeed = playerMovement.speed;
                playerMovement.speed = scoppedSpeed;
            }

            else if (aimed)
            {
                aimed = false;
                anim.SetBool("Scoped", false);
                mainCamera.fieldOfView = defaultFOV;
                mouseLook.mouseSensitivity = defaultMS;
                shootPrecision = defaultshootPrecision;
                playerMovement.speed = defaultSpeed;
            }
        }

        
    }

}
