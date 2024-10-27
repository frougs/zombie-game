using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JitterBug : PerkBase
{
    private JitterBugController controller;
    [SerializeField] int maxChain;
    [SerializeField] float chainDamage;
    [SerializeField] float chainRange;
    [SerializeField] float lightningDuration;
    public override void DefaultPerk(){
        //Add chain lightning logic here
        controller = player.GetComponent<JitterBugController>();
        controller.maxChains = maxChain;
        controller.damage = chainDamage;
        controller.perkPurchased = true;
        controller.chainRange = chainRange;
        controller.lightningDuration = lightningDuration;
    }
    public override void PerkUpgrade1(){
        //Reload buff logic here
        controller.reloadBuffActive = true;
    }
    public override void PerkUpgrade2(){
        //Rock Upgrade Logic here
        controller.UpgradeRock();
    }
    public override void PerkUpgrade3(){
        //Enemy explosion logic here
        controller.SubscribeToEnemyDeath();
    }
        public override void GetUpgradeLevel(){
        if(PlayerPrefs.HasKey("JitterBug")){
            upgradeNum = PlayerPrefs.GetInt("JitterBug");
        }
        else{
            PlayerPrefs.SetInt("JitterBug", 0);
        }
        
    }

}
