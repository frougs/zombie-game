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
    [SerializeField] float reloadBuffAmount;
    [SerializeField] float reloadBuffDuration;
    [SerializeField] GameObject upgradedRock;
    [Header("Death Field Stuffs")]
    [SerializeField] GameObject deathField;
    [SerializeField] float fieldDuration;
    [SerializeField] float fieldDamage;
    [SerializeField] float fieldFirerate;
    bool spawnFieldOnDeath;
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
        controller.reloadBuffAmount = reloadBuffAmount;
        controller.reloadBuffDuration = reloadBuffDuration;
    }
    public override void PerkUpgrade2(){
        //Rock Upgrade Logic here
        controller.GetComponent<WeaponController>().rock = upgradedRock;
        controller.GetComponent<WeaponController>().rockUpgradeActive = true;
    }
    public override void PerkUpgrade3(){
        //Enemy explosion logic here
        spawnFieldOnDeath = true;
    }
        public override void GetUpgradeLevel(){
        if(PlayerPrefs.HasKey("JitterBug")){
            upgradeNum = PlayerPrefs.GetInt("JitterBug");
        }
        else{
            PlayerPrefs.SetInt("JitterBug", 0);
        }
        
    }
    public void SpawnField(Vector3 pos){
        var deathFieldSpawned = Instantiate(deathField, pos, Quaternion.identity);
        deathFieldSpawned.GetComponent<JitterbugExplosion>().player = player;
        deathFieldSpawned.GetComponent<JitterbugExplosion>().fieldDuration = fieldDuration;
        deathFieldSpawned.GetComponent<JitterbugExplosion>().fieldDamage = fieldDamage;
        deathFieldSpawned.GetComponent<JitterbugExplosion>().fieldDamageInterval = fieldFirerate;
    }

}
