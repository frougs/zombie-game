using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class Dummy : MonoBehaviour, IDamagable
{
    [SerializeField] float maxHealth;
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
    private GameObject target;
    public bool pastBarricade;
    [SerializeField] float attackDelay;
    [SerializeField] float attackDistance;
    [SerializeField] float attackDamage;
    private bool currentlyAttacking;

    public void Damaged(float damage){
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
        nav.enabled = false;
    }
    private void FixedUpdate(){
        if(currentHealth <= 0){
            Death();
        }
        if(currentHealth != maxHealth && !dead){
            healthText.gameObject.SetActive(true);
            healthText.text = currentHealth.ToString() + " | " +maxHealth.ToString();
        }
        if(startChase){
            EnableEnemy();
            startChase = false;
            chasing = true;
        }
        if(chasing){
            nav.SetDestination(target.transform.position);
        }
        if(player == null){
            player = FindObjectOfType<ThirdPersonController>().gameObject;
        }
        if(pastBarricade){
            target = player;
        }
        if(target != null){
            if(Vector3.Distance(this.transform.position, target.transform.position) <= attackDistance && currentlyAttacking == false){
                StartCoroutine(DamageTimer());
            }
        }
    }
    private void Death(){
        healthText.gameObject.SetActive(false);
        dead = true;
        eRend.material = deadMat;
        StartCoroutine(RegenTimer());
        chasing = false;
        nav.SetDestination(this.transform.position);

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
        Debug.Log("Attacking");
        currentlyAttacking = false;
        IDamagable attackTarget = target.GetComponent<IDamagable>();
        if(attackTarget != null){
            attackTarget.Damaged(attackDamage);
        }
    }
}
