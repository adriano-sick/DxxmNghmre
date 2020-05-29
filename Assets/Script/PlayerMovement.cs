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
    bool isGrounded;

    public float health = 100f;

    public bool isAlive = true;
    public GameObject deathCam;

    public AudioClip gotMagSound;
    private AudioSource playerSound;

    private void Start()
    {
        deathCam.SetActive(false);
        playerSound = GetComponent<AudioSource>();
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


    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "FlameThrowerCollider")
        {
            TakeDamage(mummyDamage);
        }

        if (other.gameObject.tag == "pistolMagazine")
        {
            Gun gun = GetComponentInChildren<Gun>();
            gun.pisMag += 1;
            playerSound.PlayOneShot(gotMagSound);
            Destroy(other.gameObject);
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
