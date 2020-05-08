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

    public float fireRate = 15f;
    private float nextTimeToFire = 0f;

    public AudioClip gunShoot;
    private AudioSource gunSound;
    public float SFXVol = 1f;

   void Start()
    {
        gunSound = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1")  && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
        
    }

    void Shoot()
    {
        muzzleFlash.Play();
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
    IEnumerator Flashlight()
    {
        gunFlashLight.SetActive(true);
        yield return new WaitForSeconds(gunFlashTime);
        gunFlashLight.SetActive(false);
    }



}
