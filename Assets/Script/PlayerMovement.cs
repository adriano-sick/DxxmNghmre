using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float mummyDamage = 10.0f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    private bool isGrounded;

    public float health = 100f;

    public bool isAlive = true;
    public GameObject deathCam;

    public AudioClip gotMagSound;
    private AudioSource playerSound;

    public int pistolMag;
    public int carbineMag;
    public int rifleMag;
    public int shotgunMag;

    public bool haveCarbine = false;
    public bool havePistol = false;
    public bool haveRifle = false;
    public bool haveShotgun = false;

    public GameObject Pistol;
    public GameObject M4_Carbine;
    public GameObject L96_Rifle;
    public GameObject Shotgun;

    private Gun gunL96_Rifle;
    private Gun gunPistol;
    private Gun gunM4_Carbine;
    private Gun gunShotgun;
    private Gun gun;
    private float changeTime;
    public float changeCooldown;

    private void Start()
    {
        gunL96_Rifle = L96_Rifle.GetComponent<Gun>();
        gunPistol = Pistol.GetComponent<Gun>();
        gunM4_Carbine = M4_Carbine.GetComponent<Gun>();
        gunShotgun = Shotgun.GetComponent<Gun>();
        deathCam.SetActive(false);
        playerSound = GetComponent<AudioSource>();
        gunL96_Rifle.crosshair.SetActive(false);
        gunPistol.crosshair.SetActive(false);
        gunM4_Carbine.crosshair.SetActive(false);
        gunShotgun.crosshair.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        

        if (isAlive)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && haveCarbine && M4_Carbine.activeSelf == false && Time.time > changeTime + changeCooldown)
        {
            if (gunL96_Rifle.aimed || gunPistol.aimed || gunShotgun.aimed)
            {
                gun = GetComponentInChildren<Gun>();
                StartCoroutine(gun.Scop());
            }

            GetComponentInChildren<Gun>().gunFlashLight.SetActive(false);
            
            Pistol.SetActive(false);
            L96_Rifle.SetActive(false);
            Shotgun.SetActive(false);
            M4_Carbine.SetActive(true);

            gunM4_Carbine.crosshair.SetActive(true);
            gunL96_Rifle.crosshair.SetActive(false);
            gunPistol.crosshair.SetActive(false);
            gunShotgun.crosshair.SetActive(false);

            changeTime = Time.time;
            
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && havePistol && Pistol.activeSelf == false && Time.time > changeTime + changeCooldown)
        {

            if (gunL96_Rifle.aimed || gunM4_Carbine.aimed || gunShotgun.aimed)
            {
                gun = GetComponentInChildren<Gun>();
                StartCoroutine(gun.Scop());
            }

            GetComponentInChildren<Gun>().gunFlashLight.SetActive(false);
            
            M4_Carbine.SetActive(false);
            L96_Rifle.SetActive(false);
            Shotgun.SetActive(false);
            Pistol.SetActive(true);

            gunPistol.crosshair.SetActive(true);
            gunL96_Rifle.crosshair.SetActive(false);
            gunM4_Carbine.crosshair.SetActive(false);
            gunShotgun.crosshair.SetActive(false);

            changeTime = Time.time;

        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && haveShotgun && Shotgun.activeSelf == false && Time.time > changeTime + changeCooldown)
        {
            if (gunL96_Rifle.aimed || gunM4_Carbine.aimed || gunPistol.aimed)
            {
                gun = GetComponentInChildren<Gun>();
                StartCoroutine(gun.Scop());
            }

            GetComponentInChildren<Gun>().gunFlashLight.SetActive(false);
            
            Pistol.SetActive(false);
            M4_Carbine.SetActive(false);
            L96_Rifle.SetActive(false);
            Shotgun.SetActive(true);

            gunShotgun.crosshair.SetActive(true);
            gunL96_Rifle.crosshair.SetActive(false);
            gunM4_Carbine.crosshair.SetActive(false);
            gunPistol.crosshair.SetActive(false);

            changeTime = Time.time;

        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && haveRifle && L96_Rifle.activeSelf == false && Time.time > changeTime + changeCooldown)
        {
            if (gunM4_Carbine.aimed || gunPistol.aimed || gunShotgun.aimed)
            {
                gun = GetComponentInChildren<Gun>();
                StartCoroutine(gun.Scop());
            }

            GetComponentInChildren<Gun>().gunFlashLight.SetActive(false);
            
            Pistol.SetActive(false);
            M4_Carbine.SetActive(false);
            Shotgun.SetActive(false);
            L96_Rifle.SetActive(true);

            gunShotgun.crosshair.SetActive(false);
            gunL96_Rifle.crosshair.SetActive(false);
            gunM4_Carbine.crosshair.SetActive(false);
            gunPistol.crosshair.SetActive(false);


            if (gunM4_Carbine.aimed || gunPistol.aimed || gunShotgun.aimed)
            {
                gun = GetComponentInChildren<Gun>();
                StartCoroutine(gun.Scop());
            }
            changeTime = Time.time;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.name == "FlameThrowerCollider")
        {
            TakeDamage(mummyDamage);
        }

        if (other.gameObject.tag == "PistolMagazine")
        {
            pistolMag += 1;
            playerSound.PlayOneShot(gotMagSound);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "CarbineMagazine")
        {
            carbineMag += 1;
            playerSound.PlayOneShot(gotMagSound);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "RifleMagazine")
        {
            rifleMag += 1;
            playerSound.PlayOneShot(gotMagSound);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "ShotgunMagazine")
        {
            shotgunMag += 1;
            playerSound.PlayOneShot(gotMagSound);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "OnGroundPistol")
        {
            if (havePistol == false)
            {
                havePistol = true;
                Pistol.SetActive(true);
                M4_Carbine.SetActive(false);
                L96_Rifle.SetActive(false);
                Shotgun.SetActive(false);
                gunPistol.crosshair.SetActive(true);
                gunM4_Carbine.crosshair.SetActive(false);
                gunL96_Rifle.crosshair.SetActive(false);
                gunShotgun.crosshair.SetActive(false);
                playerSound.PlayOneShot(gotMagSound);
                Destroy(other.gameObject);
            }

            else if (havePistol == true)
            {
                pistolMag += 1;
                playerSound.PlayOneShot(gotMagSound);
                Destroy(other.gameObject);
            }
            

        }

        if (other.gameObject.tag == "OnGroundCarbine")
        {
            if (haveCarbine == false)
            {
                haveCarbine = true;
                M4_Carbine.SetActive(true);
                Pistol.SetActive(false);
                L96_Rifle.SetActive(false);
                Shotgun.SetActive(false);
                gunM4_Carbine.crosshair.SetActive(true);
                gunL96_Rifle.crosshair.SetActive(false);
                gunPistol.crosshair.SetActive(false);
                gunShotgun.crosshair.SetActive(false);
                playerSound.PlayOneShot(gotMagSound);
                Destroy(other.gameObject);
            }

            else if (haveCarbine == true)
            {
                carbineMag += 1;
                playerSound.PlayOneShot(gotMagSound);
                Destroy(other.gameObject);
            }

        }

        if (other.gameObject.tag == "OnGroundRifle")
        {
            if (haveRifle == false)
            {
                haveRifle = true;
                L96_Rifle.SetActive(true);
                M4_Carbine.SetActive(false);
                Pistol.SetActive(false);
                Shotgun.SetActive(false);
                gunL96_Rifle.crosshair.SetActive(false);
                gunM4_Carbine.crosshair.SetActive(false);
                gunPistol.crosshair.SetActive(false);
                gunShotgun.crosshair.SetActive(false);
                playerSound.PlayOneShot(gotMagSound);
                Destroy(other.gameObject);
            }

            else if (haveRifle == true)
            {
                rifleMag += 1;
                playerSound.PlayOneShot(gotMagSound);
                Destroy(other.gameObject);
            }


        }

        if (other.gameObject.tag == "OnGroundShotgun")
        {
            if (haveShotgun == false)
            {
                haveShotgun = true;
                Shotgun.SetActive(true);
                M4_Carbine.SetActive(false);
                Pistol.SetActive(false);
                L96_Rifle.SetActive(false);
                gunShotgun.crosshair.SetActive(true);
                gunM4_Carbine.crosshair.SetActive(false);
                gunL96_Rifle.crosshair.SetActive(false);
                gunPistol.crosshair.SetActive(false);
                playerSound.PlayOneShot(gotMagSound);
                Destroy(other.gameObject);
            }

            else if (haveShotgun == true)
            {
                shotgunMag += 1;
                playerSound.PlayOneShot(gotMagSound);
                Destroy(other.gameObject);
            }

        }

    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }

    }

    void Die()
    {
        Debug.Log("You Die!!!");
        isAlive = false;
        deathCam.SetActive(true);
        GameObject.Find("Canvas").SetActive(false);
        Destroy(gameObject, 0f);
    }
}
