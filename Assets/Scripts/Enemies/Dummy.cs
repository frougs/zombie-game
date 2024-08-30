using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class Dummy : MonoBehaviour, IDamagable
{
    [SerializeField] public float maxHealth;
    [SerializeField] float regenDelay;
    [SerializeField] TextMeshPro healthText;
    [SerializeField] float currentHealth;
    private bool dead;
    [SerializeField] Material deadMat;
    [SerializeField] Material aliveMat;
    private Renderer eRend;
    private NavMeshAgent nav;
    public bool startChase;
    private bool chasing;
    public GameObject player;
    [SerializeField] private GameObject target;
    public bool pastBarricade;
    [SerializeField] float attackDelay;
    [SerializeField] float attackDistance;
    [SerializeField] float attackDamage;
    private bool currentlyAttacking;
    [SerializeField] int killBonus;
    [SerializeField] public int spawnedRoom;
    private bool barrierDown;
    private GameObject targetBarrier;
    private bool barrierDelay;

    public void Damaged(float damage, GameObject attacker){
        //Debug.Log("OUCHIE!!!! I WAS DAMAGED FOR: " + damage);
        if(!dead){
            currentHealth -= damage;
        }
    }
    private void Start(){
        healthText.text = maxHealth.ToString() + " | " +maxHealth.ToString();
        eRend = GetComponent<Renderer>();
        currentHealth = maxHealth;
        nav = GetComponent<NavMeshAgent>();
        //nav.enabled = false;
        targetBarrier = FindOptimalBarrier();

    }
    private void FixedUpdate(){
        if(currentHealth <= 0){
            Death();
        }
        if(currentHealth != maxHealth && !dead){
            healthText.gameObject.SetActive(true);
            healthText.text = currentHealth.ToString() + " | " +maxHealth.ToString();
        }
        /*if(startChase){
            EnableEnemy();
            startChase = false;
            chasing = true;
        }*/
        if(chasing){
            if(target != null && nav != null){
                nav.SetDestination(target.transform.position);
            }
        }
        if(player == null){
            player = FindObjectOfType<ThirdPersonController>().gameObject;
        }
        /*if(pastBarricade){
            chasing = true;
            target = player;
        }
        else{
            chasing = true;
            target = FindOptimalBarrier();
        }*/
        if(target != player){
            chasing = true;
            //this.target = FindOptimalBarrier();
            this.target = targetBarrier;
            if(targetBarrier.GetComponent<BarrierScript>() != null){
                if(targetBarrier.GetComponent<BarrierScript>().barrierActive == false){
                    target = player;
                    StartCoroutine(BarrierBreakDelay());
                }
            }
        }
        else{
            chasing = true;
            target = player;
        }

        if(target != null){
            if(Vector3.Distance(this.transform.position, target.transform.position) <= attackDistance && currentlyAttacking == false && barrierDelay == false){
                StartCoroutine(DamageTimer());
            }
        }
    }
    private void Death(){
        FindObjectOfType<ScoreSystem>().AddToScore(killBonus);
        FindObjectOfType<RoundsScript>().remainingSpawnCount -= 1;
        FindObjectOfType<RoundsScript>().currentAlive -= 1;
        Destroy(this.gameObject);
        // healthText.gameObject.SetActive(false);
        // dead = true;
        // eRend.material = deadMat;
        // StartCoroutine(RegenTimer());
        // chasing = false;
        // nav.SetDestination(this.transform.position);

    }
    IEnumerator RegenTimer(){
        while(currentHealth < maxHealth){
            currentHealth += 0.5f * Time.deltaTime;
            //yield return null;
        }
        yield return new WaitForSeconds(regenDelay);
        currentHealth = maxHealth;
        eRend.material = aliveMat;
        dead = false;
        chasing = true;
    }
    private void EnableEnemy(){
        nav.enabled = true;

    }

    IEnumerator DamageTimer(){
        currentlyAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        //Debug.Log("Attacking");
        currentlyAttacking = false;
        if(Vector3.Distance(this.transform.position, target.transform.position) <= attackDistance){
            IDamagable attackTarget = target.GetComponent<IDamagable>();
            if(attackTarget != null){
                attackTarget.Damaged(attackDamage, this.gameObject);
            }
        }
    }
    IEnumerator BarrierBreakDelay(){
        barrierDelay = true;
        yield return new WaitForSeconds(attackDelay);
        barrierDelay = false;
    }
    // private GameObject FindOptimalBarrier(){
    //     var rooms = FindObjectsOfType<RoomDetection>();
    //     foreach(RoomDetection room in rooms){
    //         if(room.roomNumber == spawnedRoom){
    //             var barriers = room.barriers;
    //             float closestBarrier = Mathf.Infinity;
    //             foreach(BarrierScript barrier in barriers){
    //                 float distance = Vector3.Distance(this.transform.position, barrier.gameObject.transform.position);
    //                 if(distance < closestBarrier){
    //                     closestBarrier = distance;
    //                     if(barrier.barrierActive == true){
    //                         return barrier.gameObject;
    //                     }
    //                     else{
    //                         break;
    //                     }
    //                 }
    //             }
    //         }
    //     }
    //     return player;
    // }
    private GameObject FindOptimalBarrier()
    {
        var rooms = FindObjectsOfType<RoomDetection>();
        GameObject closestBarrier = null;
        float closestDistance = Mathf.Infinity;

        foreach (RoomDetection room in rooms)
        {
            if (room.roomNumber == spawnedRoom)
            {
                var barriers = room.barriers;

                foreach (BarrierScript barrier in barriers)
                {
                    if (barrier.barrierActive)
                    {
                        float distance = Vector3.Distance(this.transform.position, barrier.gameObject.transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestBarrier = barrier.gameObject;
                        }
                    }
                }
            }
        }

        
        return closestBarrier != null ? closestBarrier : player;
        //return closestBarrier;
    }
}
