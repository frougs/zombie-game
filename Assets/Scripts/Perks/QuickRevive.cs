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
        }
        else if(hasPerk == false){
            var scoreSystem = interactionCon.GetComponent<ScoreSystem>();
            if(scoreSystem.score >= price){
                scoreSystem.SubtractScore(price);
                ActivatePerk();
            }
        }
    }
    public override void DefaultPerk(){
        player.GetComponent<PlayerHealth>().weaponXEnabled = true;
        player.GetComponent<PlayerHealth>().weaponXLevel = 1;
    }
    public override void PerkUpgrade1(){
        player.GetComponent<PlayerHealth>().weaponXLevel = 2;
    }
    public override void PerkUpgrade2(){
        player.GetComponent<PlayerHealth>().weaponXLevel = 3;
    }
    public override void PerkUpgrade3(){
        player.GetComponent<PlayerHealth>().weaponXLevel = 4;
    }

    
}