using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public float health = 50f;
    private buttonControl_script enemyAnim;

    private NavMeshAgent meshNav;
    public GameObject player;
    public float followDistance = 20.0f;
    public float attackDistance = 10.0f;
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



    // Start is called before the first frame update
    void Start()
    {

        meshNav = GetComponent<NavMeshAgent>();
        enemySound = GetComponent<AudioSource>();
        enemyAnim = GetComponent<buttonControl_script>();
        
        if (gameObject.name == "mummy@idle01")
        {
            enemyAnim.Idle();
        }

    }

    // Update is called once per frame
    void Update()
    {
        enemyAnim.Idle();
        
        if(GameObject.Find("First Person Player") != null)
        {
            if (meshNav.enabled)
            {
                float dist = Vector3.Distance(player.transform.position, this.transform.position);
                bool shoot = false;
                bool follow = (dist < followDistance);

                if (follow)
                {
                    float random = Random.Range(0.0f, 1.0f);
                    if (random > (1.0f - attackProbability) && dist < attackDistance)
                    {
                        ShootEvent();
                        

                    }
                }

                if (follow)
                {
                    meshNav.SetDestination(player.transform.position);
                    enemyAnim.Run();

                    //enemySound.PlayOneShot(followSound, 1f);
                }
            }
        }
        

        
    }

    public void ShootEvent()
    {
        if (GameObject.Find("WFX_FlameThrower Big(Clone)") == null)
        {
            GameObject temp = Instantiate(fire, gameObject.transform.position + fireOffset, transform.rotation) as GameObject;
            temp.transform.SetParent(transform);
        }

        if (enemySound != null)
        {
            enemySound.PlayOneShot(GunSound);
        }

       /* float random = Random.Range(0.0f, 1.0f);

        bool isHit = random > 1.0f - hitAccuracy;

        PlayerMovement playerScript = GetComponent<PlayerMovement>();
        

        if (isHit)
        {
            playerScript.TakeDamage(damagePoints);
        }*/
    }
        
    public void TakeDamage (float amount)
    {
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
            enemyAnim.Dance();
        }
        
        Destroy(gameObject, 0.2f);
    }
}
