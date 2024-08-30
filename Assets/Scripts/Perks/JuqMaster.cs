using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuqMaster : PerkBase
{
    public int healthBoost;
    public int level1HealthBoost;
    public double chanceToIgnoreHit;
    public float damageNegationAmount;
    public override void DefaultPerk(){
        player.GetComponent<PlayerHealth>().maxHealth += healthBoost;
        player.GetComponent<PlayerHealth>().currentHealth += healthBoost;
    }
    public override void PerkUpgrade1(){
        player.GetComponent<PlayerHealth>().maxHealth += level1HealthBoost;
        player.GetComponent<PlayerHealth>().currentHealth += level1HealthBoost;
    }
    public override void PerkUpgrade2(){
        player.GetComponent<PlayerHealth>().chanceToIgnoreHit = chanceToIgnoreHit;
    }
    public override void PerkUpgrade3(){
        player.GetComponent<PlayerHealth>().damageNegationAmount = damageNegationAmount;
    }
}
