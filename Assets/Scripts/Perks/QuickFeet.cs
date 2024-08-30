using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFeet : PerkBase
{
    [SerializeField] float burstSpeedAmount;
    [SerializeField] float burstDuration;
    [SerializeField] float burstTransitionTime;
    [Header("Upgrade Stuff")]
    [SerializeField] float level1Duration;
    [SerializeField] float upgradedWalkSpeed;
    [SerializeField] float reloadSpeedAugment;
    public override void DefaultPerk(){
        player.GetComponent<ThirdPersonController>().quickFeet = true;
        player.GetComponent<ThirdPersonController>().burstSpeedAmount = burstSpeedAmount;
        player.GetComponent<ThirdPersonController>().burstDuration = burstDuration;
        player.GetComponent<ThirdPersonController>().burstTransitionTime = burstTransitionTime;
        
    }
    public override void PerkUpgrade1(){
        player.GetComponent<ThirdPersonController>().burstDuration = level1Duration;
    }
    public override void PerkUpgrade2(){
        player.GetComponent<ThirdPersonController>().MoveSpeed = upgradedWalkSpeed;
    }
    public override void PerkUpgrade3(){
        player.GetComponent<ThirdPersonController>().SprintSpeed = burstSpeedAmount;
        FindObjectOfType<WeaponController>().reloadSpeedAugment = reloadSpeedAugment;
    }
}
