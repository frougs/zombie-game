using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

    public float regenerationDelay = 5f; 
    public float regenerationRate = 1f; 
    private bool isRegenerating = false;

    private void Start()
    {
        currentHealth = maxHealth;
        if (uiStuff == null)
        {
            uiStuff = FindObjectOfType<UIContainer>();
        }
    }

    public void Damaged(float damage, GameObject attacker)
    {
        if (!dead)
        {
            currentHealth -= damage;
            StopCoroutine("RegenerateHealth"); 
            isRegenerating = false; 

            if (currentHealth <= 0)
            {
                Death();
            }
            else if(weaponXEnabled == true)
            {
                StartCoroutine(RegenerateHealthAfterDelay());
            }
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
            StartCoroutine(RegenerateHealthAfterDelay());
        }
        uiStuff.UpdateHealth((int)currentHealth);
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
            uiStuff.UpdateHealth((int)currentHealth);
            yield return null; 
        }

        isRegenerating = false;
    }
}