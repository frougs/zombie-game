using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    public int maxHealth = 100;
    [HideInInspector] public float currentHealth;
    private bool dead;
    public UnityEvent died;
    private UIContainer uiStuff;
    public bool weaponXEnabled = false;
    public int weaponXLevel;
    public int juqMasterLevel;
    public int extraLives;
    public int extraLifeBoost;
    public bool extraLifeRegen;
    [Header("Weapon X Stuff")]
//EDIT THESE FOR WEAPONX
    public float regenerationDelay = 5f; 
    public float regenerationRate = 1f; 
    private bool isRegenerating = false;
    private Coroutine regen;
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip damagedClip;

    //JuqMaster stuff
    [Header("Juq Master Stuff")]
    public double chanceToIgnoreHit;
    public float damageNegationAmount;
    private void Start()
    {
        currentHealth = maxHealth;
        if (uiStuff == null)
        {
            uiStuff = FindObjectOfType<UIContainer>();
        }
        if(soundSource == null){
            soundSource = GetComponentInChildren<AudioSource>();
        }
    }

    public void Damaged(float damage, GameObject attacker, Vector3 pos)
    {
        if(AttemptDodge() == false){
            if(regen != null){
                StopCoroutine(regen);
            }
            if (!dead)
            {
                currentHealth -= damage - damageNegationAmount;
                isRegenerating = false; 
                soundSource.PlayOneShot(damagedClip);

                if (currentHealth <= 0 && extraLives == 0)
                {
                    Death();
                }
                else if(weaponXEnabled == true)
                {
                    regen = StartCoroutine(RegenerateHealthAfterDelay());
                }
            }
        }
        else{
            //Attack Dodged, add sound effect/visual here later
        }
    }

    private void Update()
    {
        if (currentHealth <= 0 && extraLives == 0 && isRegenerating == false)
        {
            Death();
        }
        else if(extraLives > 0 && currentHealth <= 0){
            extraLives -= 1;
            currentHealth += extraLifeBoost;
            regen = StartCoroutine(RegenerateHealthAfterDelay());
        }
        uiStuff.UpdateHealth((int)currentHealth, (int)maxHealth);

        if(currentHealth/maxHealth <= 0.25f){
            uiStuff.UpdateDamagedOverlay(true, 1f - (currentHealth/maxHealth));
        }
        else{
            uiStuff.UpdateDamagedOverlay(false, 0f);
        }
    }

    private void Death()
    {
        Debug.Log("BLEGHHHH IM DEADED");
        dead = true;
        died?.Invoke();
    }

    private IEnumerator RegenerateHealthAfterDelay()
    {

        yield return new WaitForSeconds(regenerationDelay);

        isRegenerating = true;

        while (currentHealth < maxHealth && isRegenerating)
        {
            currentHealth += regenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            uiStuff.UpdateHealth((int)currentHealth, (int)maxHealth);
            yield return null; 
        }

        isRegenerating = false;
    }
    private bool AttemptDodge(){
        double roll = UnityEngine.Random.value;
        return roll < chanceToIgnoreHit;
    }
    public void StartRegenWhenPurchased(){
        regen = StartCoroutine(RegenerateHealthAfterDelay());
    }
}