using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuqMaster : PerkBase
{
    public int healthBoost;
    public float chanceToIgnoreHit;
    public float damageNegationAmount;
    public override void DefaultPerk(){
        player.GetComponent<PlayerHealth>().maxHealth += healthBoost;
        player.GetComponent<PlayerHealth>().currentHealth += healthBoost;
    }
}
