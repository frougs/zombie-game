using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class WeaponController : MonoBehaviour
{
    [HideInInspector] public InputAction attack;
    [HideInInspector] public InputAction reload;
    [HideInInspector] public PlayerInput _pInput;
    public float reloadSpeedAugment;
    private void Start(){
        _pInput = GetComponent<PlayerInput>();
        attack = _pInput.actions["Attack"];
        reload  = _pInput.actions["Reload"];

    }

    private void Update(){
        if(PauseMenuSingleton.instance.GetComponent<PauseController>().isPaused == false){
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
            if(reload.triggered){
                try{
                    var child = this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject;
                    IShootable shootable = child.GetComponent<IShootable>();
                    if(shootable != null){
                        //Debug.Log("Child Found, attempting shot");
                        //shootable.Reload();
                        if(child.GetComponent<BaseGun>() != null){
                            if( child.GetComponent<BaseGun>().fullAmmo != true){
                                child.GetComponent<BaseGun>().Reload();
                            }
                            else{
                                //Maybe add error sound
                            }
                        }
                    }
                }
                catch(Exception e){
                    //Debug.Log("No Child");
                }            
            }
        }
    }
}
