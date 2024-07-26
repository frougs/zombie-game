using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    }
    private void FixedUpdate(){
        if(currentHealth <= 0){
            Death();
        }
        if(currentHealth != maxHealth && !dead){
            healthText.gameObject.SetActive(true);
            healthText.text = currentHealth.ToString() + " | " +maxHealth.ToString();
        }
    }
    private void Death(){
        healthText.gameObject.SetActive(false);
        dead = true;
        eRend.material = deadMat;
        StartCoroutine(RegenTimer());

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
    }
}
