using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickRevive : PerkBase
{
    public bool firstPurchased = false;
    
    public override void Interacted(GameObject gunRoot, InteractionController interactionCon){
        player = interactionCon.gameObject;
        if(powered && !firstPurchased){
            firstPurchased = true;
            ActivatePerk();
            soundSource.PlayOneShot(purchase);
        }
        else if(hasPerk == false){
            var scoreSystem = interactionCon.GetComponent<ScoreSystem>();
            if(scoreSystem.score >= price){
                scoreSystem.SubtractScore(price);
                ActivatePerk();
                soundSource.PlayOneShot(purchase);
            }
            else{
                soundSource.PlayOneShot(errorPurchase);
            }
        }
    }
    public override void DefaultPerk(){
        player.GetComponent<PlayerHealth>().weaponXEnabled = true;
        player.GetComponent<PlayerHealth>().weaponXLevel = 1;
        player.GetComponent<PlayerHealth>().extraLives += 1;
        player.GetComponent<PlayerHealth>().StartRegenWhenPurchased();
    }
    public override void PerkUpgrade1(){
        player.GetComponent<PlayerHealth>().weaponXLevel = 2;
        player.GetComponent<PlayerHealth>().regenerationDelay =  player.GetComponent<PlayerHealth>().regenerationDelay / 2f;
    }
    public override void PerkUpgrade2(){
        player.GetComponent<PlayerHealth>().weaponXLevel = 3;
        player.GetComponent<PlayerHealth>().regenerationRate += 2.5f;
        player.GetComponent<PlayerHealth>().extraLifeBoost += 15;
    }
    public override void PerkUpgrade3(){
        player.GetComponent<PlayerHealth>().weaponXLevel = 4;
        player.GetComponent<PlayerHealth>().extraLives += 1;
    }
    public override void GetUpgradeLevel(){
        if(PlayerPrefs.HasKey("WeaponX")){
            upgradeNum = PlayerPrefs.GetInt("WeaponX");
        }
        else{
            PlayerPrefs.SetInt("WeaponX", 0);
        }
        
    }
    
}
