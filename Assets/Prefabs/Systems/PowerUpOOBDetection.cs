using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpOOBDetection : MonoBehaviour
{
    [SerializeField] GameObject repositionPoint;
    public void OnTriggerEnter(Collider other){
        if(other.gameObject.GetComponent<PowerupBase>() != null){
            other.gameObject.transform.position = repositionPoint.transform.position;
        }
    }
}
