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
    public float attackProbability = 100f;
    [Range(0.0f, 1.0f)]
    public float hitAccuracy = 0.5f;
    public float damagePoints = 2.0f;
    public AudioClip GunSound = null;
    public AudioClip followSound = null;
    private AudioSource enemySound;
    public GameObject fire;
    public Vector3 fireOffset = new Vector3(0, 3, 0);
    private Animator anim;

    void Start()
    {
        meshNav = GetComponent<NavMeshAgent>();
        enemySound = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();

        anim.SetBool("isIdle", true);
                
    }

    void Update()
    {
        if (meshNav.velocity == new Vector3(0, 0, 0))
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isRun", false);
            
        }

        else if (meshNav.velocity != new Vector3(0, 0, 0))
        {
            anim.SetBool("isRun", true);
            anim.SetBool("isIdle", false);
        }

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("takeDamage"))
        {
            anim.SetBool("takeDamage", false);
        }


        if (GameObject.Find("First Person Player") != null)
        {
            if (meshNav.enabled)
            {
                float dist = Vector3.Distance(player.transform.position, this.transform.position);
                bool follow = (dist < followDistance);

                if (follow)
                {

                    meshNav.SetDestination(player.transform.position);
                    float random = Random.Range(0.0f, 1.0f);
                    if (random > (1.0f - attackProbability) && dist < attackDistance)
                    {
                        ShootEvent();

                    }                                       

                }
                
            }

        }        

        else if(GameObject.Find("First Person Player") == null)
        {
            
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
        health -= amount;        
        if (health <= 0f)
        {
            Die();
        }

        anim.SetBool("takeDamage", true);

    }

    void Die()
    {
                
        Destroy(gameObject, 0.2f);
    }
}
