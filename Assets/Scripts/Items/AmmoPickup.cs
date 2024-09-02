using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : FloatingPickup
{
    [SerializeField] int ammoAmount;
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.GetComponent<ThirdPersonController>() != null){
            if(other.gameObject.GetComponent<InteractionController>().currentlyHeld.GetComponent<BaseGun>().currentReserveAmmo < other.gameObject.GetComponent<InteractionController>().currentlyHeld.GetComponent<BaseGun>().maxReserveAmmo){
                other.gameObject.GetComponent<InteractionController>().currentlyHeld.GetComponent<BaseGun>().currentReserveAmmo += ammoAmount;
                Destroy(this.gameObject);
            }
        }
    }
}
