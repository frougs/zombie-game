using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstaKillPickup : MonoBehaviour
{
    private void Update(){
        try{
            this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject.GetComponent<BaseGun>().InstaKillActive = true;
        }
        catch(Exception e){}
        this.GetComponent<WeaponController>().instaKillActive = true;

    }
}
