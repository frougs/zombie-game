using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Cinemachine;

public class WeaponController : MonoBehaviour
{
    [HideInInspector] public InputAction attack;
    [HideInInspector] public InputAction reload;
    [HideInInspector] public InputAction aim;
    [HideInInspector] public PlayerInput _pInput;
    [SerializeField] GameObject rock;
    [SerializeField] float rockSpeed;
    [SerializeField] AudioClip rockThrownSound;
    [SerializeField] public AudioSource soundSource;
    [SerializeField] int rockScorePerHit;
    [SerializeField] float rockDamage;
    [SerializeField] float rockFirerate;
    [SerializeField] float rockCritMultiplier;
    private bool canThrowRock = true;
    public float reloadSpeedAugment;
    public bool instaKillActive = false;
    [SerializeField] float adsFOVAdditive;
    [SerializeField] float adsTransitionTime;
    public FOVController cam;
    
    private void Start(){
        cam = FindObjectOfType<FOVController>();
        _pInput = GetComponent<PlayerInput>();
        attack = _pInput.actions["Attack"];
        reload  = _pInput.actions["Reload"];
        aim = _pInput.actions["Aim"];
        if(PlayerPrefs.HasKey("RockDMG")){
            if(PlayerPrefs.GetInt("RockDMG") != 0){
                rockDamage += (rockDamage * ((PlayerPrefs.GetInt("RockDMG") * 10) *.01f));
            }
        }
        if(PlayerPrefs.HasKey("RockPoints")){
            if(PlayerPrefs.GetInt("RockPoints") != 0){
                rockScorePerHit += (int)(rockScorePerHit * ((PlayerPrefs.GetInt("RockPoints") * 15) *.01f)) +1;
            }
        }
        if(PlayerPrefs.HasKey("RockFR")){
            if(PlayerPrefs.GetInt("RockFR") != 0){
                rockFirerate -= (rockFirerate * ((PlayerPrefs.GetInt("RockFR") * 10) *.01f));
            }
        }

    }

    private void Update(){
        if(PauseMenuSingleton.instance.GetComponent<PauseController>().isPaused == false){
            if(aim.IsPressed()){
                //ADS logic
            }
            else{
                //Return to default Logic
            }
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
                    ThrowRock();
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
            if(aim.IsPressed()){
                //Debug.Log("Aim Pressed");
                //Add logic for changing the FOV controller
                this.gameObject.GetComponent<ThirdPersonController>().sprintingBlocked = true;
                var adsFOV = PlayerPrefs.GetFloat("FOV") - adsFOVAdditive;
                cam.FOVChange(adsFOV, adsTransitionTime);
            }
            else{
                //Add logic for reverting the FOV controller
                this.gameObject.GetComponent<ThirdPersonController>().sprintingBlocked = false;

                cam.canRevert = true;
            }
        }
    }
    private void ThrowRock(){
        //Debug.Log("Throwing rock");
        if(canThrowRock){
            StartCoroutine(ShotDelay());
            GameObject launchedProj = Instantiate(rock, GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, Quaternion.identity);
            Rigidbody rb = launchedProj.GetComponent<Rigidbody>();
            if(!instaKillActive){
                launchedProj.GetComponent<ThrownRock>().AssignVariables(rockDamage, this.gameObject, rockScorePerHit, rockCritMultiplier);
            }
            else{
                            launchedProj.GetComponent<ThrownRock>().AssignVariables(Mathf.Infinity, this.gameObject, rockScorePerHit, rockCritMultiplier);
            }
            if(rb != null){
                            rb.velocity =  GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward * rockSpeed;
                            soundSource.PlayOneShot(rockThrownSound);
            }
        }
    }
    public IEnumerator ShotDelay(){
        canThrowRock = false;
        yield return new WaitForSeconds(rockFirerate);
        canThrowRock = true;
    }

    
}
