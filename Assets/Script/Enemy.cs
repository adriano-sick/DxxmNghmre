using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public float health = 50f;
    private buttonControl_script enemyAnim;

    private NavMeshAgent meshNav;
    public GameObject player;
    public float followDistance = 20.0f;
    public float limitDistance = 2.0f;
    public float attackDistance = 6.0f;
    [Range(0.0f, 1.0f)]
    public float attackProbability = 50f;
    [Range(0.0f, 1.0f)]
    public float hitAccuracy = 0.5f;
    public float damagePoints = 2.0f;
    public AudioClip GunSound = null;
    public AudioClip followSound = null;
    private AudioSource enemySound;
    public GameObject fire;
    public Vector3 fireOffset = new Vector3(0, 3, 0);
    

    void Start()
    {
        meshNav = GetComponent<NavMeshAgent>();
        enemySound = GetComponent<AudioSource>();
        enemyAnim = GetComponent<buttonControl_script>();
                
    }

    void Update()
    {

        if (GameObject.Find("First Person Player") != null)
        {
            if (meshNav.enabled)
            {
                float dist = Vector3.Distance(player.transform.position, this.transform.position);
                bool follow = (dist < followDistance);

                if (follow)
                {

                    meshNav.SetDestination(player.transform.position);
                    
                    enemyAnim.Invoke("Run", 0f);

                    float random = Random.Range(0.0f, 1.0f);
                    if (random > (1.0f - attackProbability) && dist < attackDistance)
                    {
                        ShootEvent();

                    }

                }

                else if (!follow)
                {                    
                    if (gameObject.name == "mummy@idle01")
                    {
                        enemyAnim.Invoke("Idle", 0f);
                    }

                }

            }

        }
        

        else if(GameObject.Find("First Person Player") == null)
        {
            enemyAnim.Invoke("Dance", 0f);
        }
        
    }

    public void ShootEvent()
    {
        if(transform.childCount < 3)
        {
           GameObject temp = Instantiate(fire, gameObject.transform.position + fireOffset, transform.rotation) as GameObject;
           temp.transform.SetParent(transform);
        }

        if (enemySound != null && !enemySound.isPlaying)
        {
            enemySound.PlayOneShot(GunSound);
        }
      
    }
        
    public void TakeDamage (float amount)
    {
        enemyAnim.Invoke("TakeDamage", 0f);
        health -= amount;        
        if (health <= 0f)
        {
            Die();
        }

    }

    void Die()
    {
        if(gameObject.name == "mummy@idle01")
        {
            enemyAnim.Invoke("Dance", 0f);
        }
        
        Destroy(gameObject, 0.2f);
    }
}
