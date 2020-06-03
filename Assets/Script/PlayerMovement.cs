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

    public bool haveCarbine = false;
    public bool havePistol = false;
    public bool haveRifle = false;

    public GameObject Pistol;
    public GameObject M4_Carbine;
    public GameObject L96_Rifle;

    public GameObject crosshair;

    private Gun gunL96_Rifle;

    private void Start()
    {
        gunL96_Rifle = L96_Rifle.GetComponent<Gun>();
        deathCam.SetActive(false);
        playerSound = GetComponent<AudioSource>();
        crosshair.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.Alpha1) && haveCarbine)
        {
            M4_Carbine.SetActive(true);
            Pistol.SetActive(false);
            L96_Rifle.SetActive(false);
            crosshair.SetActive(true);

            if (gunL96_Rifle.aimed)
            {
                gunL96_Rifle.Scop();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && havePistol)
        {
            Pistol.SetActive(true);
            M4_Carbine.SetActive(false);
            L96_Rifle.SetActive(false);
            crosshair.SetActive(true);            

            if (gunL96_Rifle.aimed)
            {
                gunL96_Rifle.Scop();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && haveRifle)
        {
            L96_Rifle.SetActive(true);
            Pistol.SetActive(false);
            M4_Carbine.SetActive(false);
            crosshair.SetActive(false);

            if (gunL96_Rifle.aimed)
            {
                gunL96_Rifle.Scop();
            }

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

        if (other.gameObject.tag == "OnGroundPistol")
        {
            if (havePistol == false)
            {
                havePistol = true;
                Pistol.SetActive(true);
                M4_Carbine.SetActive(false);
                L96_Rifle.SetActive(false);
                crosshair.SetActive(true);
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
                crosshair.SetActive(true);
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
                crosshair.SetActive(false);
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
