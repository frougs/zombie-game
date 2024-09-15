using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierRepairPowerup : MonoBehaviour
{
    private void OnEnable(){
        var barriers = FindObjectsOfType<BarrierScript>();
        foreach (var barrier in barriers){
            barrier.currentProgress = barrier.barrierCapacity;
        }
    }
}
