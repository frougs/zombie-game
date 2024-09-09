using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : PerkBase
{
    private float rageAmount;
    public bool doubledamage;
    public float ragePerShot;
    public float critMultiplier;
    public float rageDecayRate;
    public float rageDecayPauseDuration;
    private float storedDecayRate;
    private bool raging;
    private bool canDecay = true;
    private Coroutine rageDecayCoroutine;
    [Header("Upgrades")]
    public float ragePerShotUpgrade;
    public float rageDecayRateUpgrade;
    public override void DefaultPerk(){
        raging = true;
    }
    public override void PerkUpgrade1(){
        ragePerShot = ragePerShotUpgrade;
    }
    public override void PerkUpgrade2(){
        rageDecayRate = rageDecayRateUpgrade;
    }
    public override void PerkUpgrade3(){
        canDecay = false;
        rageAmount = 1f;
    }
    public void Hit(){
        rageAmount += ragePerShot;
        if(rageDecayCoroutine != null){
            StopCoroutine(rageDecayCoroutine);
        }
    }
    public void CritHit(){
        rageAmount += ragePerShot * critMultiplier;
        if(rageDecayCoroutine != null){
            StopCoroutine(rageDecayCoroutine);
        }
    }
    private void FixedUpdate(){
        uiStuff.UpdateRageBar(raging, rageAmount, doubledamage);
        if(canDecay){
            rageAmount -= rageDecayRate * Time.fixedDeltaTime;
        }

        if(rageAmount <= 0f){
            rageAmount = 0f;
        }
        if(rageAmount >= 1f){
            rageAmount = 1f;
            doubledamage = true;
            //canDecay = false;
            if(canDecay == true){
                rageDecayCoroutine = StartCoroutine(RageDecayPause());
            }
            
        }
        else{
            doubledamage = false;
        }
    }
    private IEnumerator RageDecayPause(){
        canDecay = false;
        yield return new WaitForSeconds(rageDecayPauseDuration);
        canDecay = true;
    }
    public override void GetUpgradeLevel(){
        if(PlayerPrefs.HasKey("Rage")){
            upgradeNum = PlayerPrefs.GetInt("Rage");
        }
        else{
            PlayerPrefs.SetInt("Rage", 0);
        }
        
    }
}
