using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponController : MonoBehaviour
{
    public GameObject gunRoot;
    private bool holdingWeapon;
    private void Update(){
        if(GetComponent<InteractionController>().drop.triggered && GetComponent<Alteruna.Avatar>().IsMe){
            Debug.Log("Drop Pressed");
            var weapon = gunRoot.transform.GetChild(0);
            foreach(Transform obj in weapon.transform){
                if(obj.GetComponent<WeaponPickup>()!=null){
                    obj.GetComponent<WeaponPickup>().Dropped(/*this.gameObject*/);
                }
            }
        }
    }
    //Eventually Add firing mechanic below using interfaces, find object childed under gunroot that implements an IShootable function or something and call its shootfunction
}
