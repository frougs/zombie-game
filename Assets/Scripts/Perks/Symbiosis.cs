using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbiosis : PerkBase
{
    [SerializeField] GameObject ammoDropObj;
    [SerializeField] float discountAmount;
    [SerializeField] public bool lifeSteal;
    [SerializeField] public float lifeStealPercent;
    private Purchasable[] allPurchasables;

    public void EnemyDeath(GameObject enemyPos){
        if(hasPerk){
            Instantiate(ammoDropObj, enemyPos.transform.position, Quaternion.identity);
        }
    }
    public override void PerkUpgrade1(){
        //Lifesteal
        lifeSteal = true;
    }
    public override void PerkUpgrade2(){
        //Instarepair barriers
        var allBarriers = FindObjectsOfType<BarrierScript>();
        foreach(BarrierScript barrier in allBarriers){
            barrier.progressPerInteract = barrier.barrierCapacity;
        }
    }
    public override void PerkUpgrade3(){
        //Discounts
        allPurchasables = FindObjectsOfType<Purchasable>();
        foreach(Purchasable buyable in allPurchasables){
            buyable.price = buyable.price - (int)(buyable.price * discountAmount);
            buyable.discountApplied = true;
            buyable.discountAmount = discountAmount;
        }
    }
}
