using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDetection : MonoBehaviour
{
    public int roomNumber;
    [SerializeField] SpawnerScript[] spawners;
    [SerializeField] public BarrierScript[] barriers;
    private void OnTriggerStay(Collider other){
        if(other.gameObject.GetComponent<ThirdPersonController>() != null){
            foreach(SpawnerScript spawner in spawners){
                spawner.playerActive = true;
            }
        }
    }
    private void OnTriggerExit(Collider other){
         if(other.gameObject.GetComponent<ThirdPersonController>() != null){
            foreach(SpawnerScript spawner in spawners){
                spawner.playerActive = false;
            }
        }
    }
}
