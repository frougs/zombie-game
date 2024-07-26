using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class WeaponController : MonoBehaviour
{
    [HideInInspector] public InputAction attack;
    [HideInInspector] public PlayerInput _pInput;
    private void Start(){
        _pInput = GetComponent<PlayerInput>();
        attack = _pInput.actions["Attack"];
    }

    private void Update(){
        if(attack.IsPressed()){
            try{
                var child = this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject;
                IShootable shootable = child.GetComponent<IShootable>();
                if(shootable != null){
                    //Debug.Log("Child Found, attempting shot");
                    shootable.Shot(this.gameObject);
                }
            }
            catch(Exception e){
                //Debug.Log("No Child");
            }

            
        }
    }
}
