using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    public int maxHealth = 100;
    [HideInInspector] float currentHealth;
    private bool dead;
    public UnityEvent died;
    private UIContainer uiStuff;
    private void Start(){
        currentHealth = maxHealth;
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
    }
    public void Damaged(float damage, GameObject attacker){
        if(!dead){
            currentHealth -= damage;
        }
    }
    private void Update(){
        if(currentHealth <= 0){
            Death();
        }
        else{
            dead = false;
        }
        uiStuff.UpdateHealth((int)currentHealth);
    }
    private void Death(){
        Debug.Log("BLEGHHHH IM DEADED");
        dead = true;
        died?.Invoke();
    }
}
