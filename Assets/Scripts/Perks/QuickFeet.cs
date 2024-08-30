using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFeet : PerkBase
{
    [SerializeField] float burstSpeedAmount;
    [SerializeField] float burstDuration;
    [SerializeField] float burstTransitionTime;
    public override void DefaultPerk(){
        player.GetComponent<ThirdPersonController>().quickFeet = true;
        player.GetComponent<ThirdPersonController>().burstSpeedAmount = burstSpeedAmount;
        player.GetComponent<ThirdPersonController>().burstDuration = burstDuration;
        player.GetComponent<ThirdPersonController>().burstTransitionTime = burstTransitionTime;
        
    }
}
