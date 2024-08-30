using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseGun : MonoBehaviour, IShootable
{
    [SerializeField] float damage;
    [SerializeField] float fireRate;
    [SerializeField] public int maxAmmo;
    [SerializeField] public int maxReserveAmmo;
    [SerializeField] public int currentReserveAmmo;
    [SerializeField] float reloadSpeed;
    [SerializeField] float range;
    [SerializeField] float critMultiplier;
    [SerializeField] public int currentAmmo;
    [SerializeField] public int ammoPrice;
    [SerializeField] public int gunPrice;
    public bool canShoot = true;
    private bool hasAmmo = true;
    //private GameObject progressBar;
    private UIContainer uiStuff;
    public bool held;
    private Image progressBar;
    public float reloadProgress;
    private float rProgress;
    [HideInInspector] public bool reloading;
    [HideInInspector] public bool fullAmmo;
    [SerializeField] private AudioClip gunshot;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private int pointsPerHit;
    private ScoreSystem scoreSystem;
    private bool hasReserveAmmo;
    [SerializeField] public GameObject buyModel;
    public LayerMask IgnoreLayer;
    private Rage rageScript;
    float modifiedDamage;

    private void Start(){
        currentAmmo = maxAmmo;
        currentReserveAmmo = maxReserveAmmo;
        soundSource = this.GetComponent<AudioSource>();
    }
    public void Shot(GameObject shooter){
        if(reloading == false){
            if(canShoot && hasAmmo){
                if(rageScript.doubledamage){
                    modifiedDamage = damage * 2f;
                }
                else{
                    modifiedDamage = damage;
                }
                currentAmmo -= 1;
                if(soundSource != null){
                    soundSource.PlayOneShot(gunshot);
                }
                if(Physics.Raycast(shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward, out RaycastHit hitData, Mathf.Infinity,  ~IgnoreLayer)){
                    IDamagable damagable = hitData.transform.gameObject.GetComponent<IDamagable>();
                    //NonCrit hit
                    if(damagable != null && hitData.transform.gameObject.tag != "CriticalSpot"){
                        damagable.Damaged(modifiedDamage, shooter);
                        scoreSystem.AddToScore(pointsPerHit);
                        canShoot = false;
                        rageScript.Hit();
                    }
                    //Crit hit
                    else if(damagable != null && hitData.transform.gameObject.tag == "CriticalSpot"){
                        damagable.Damaged(modifiedDamage * critMultiplier, shooter);
                        scoreSystem.AddToScore((int)(pointsPerHit * critMultiplier));
                        canShoot = false;
                        rageScript.CritHit();
                    }
                    else{
                        //Debug.LogWarning("Damagable not found");
                    }
                }
                StartCoroutine(ShotDelay());
            }
            else if(canShoot && !hasAmmo && hasReserveAmmo){
                Reload();
            }
            else if(currentAmmo == 0 && currentReserveAmmo == 0){
                //idk play click sound??
                Debug.LogWarning("*CLICK* no ammo!");
            }
        }
    }
    IEnumerator ShotDelay(){
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
    private void Update(){
        if(currentReserveAmmo <= 0){
            currentReserveAmmo = 0;
            hasReserveAmmo = false;
        }
        else{
            hasReserveAmmo = true;
        }
        if(scoreSystem == null){
            scoreSystem = FindObjectOfType<ScoreSystem>();
        }
        if(currentAmmo <= 0){
            hasAmmo = false;
        }
        else{
            hasAmmo = true;
        }
        held = GetComponentInChildren<PickupController>().currentlyHeld;
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
        else{
            progressBar = uiStuff.progressBar;
        }
        if(held){
            uiStuff.UpdateAmmo(currentAmmo, currentReserveAmmo);
        }
        if(reloading){
            if(rProgress < reloadSpeed){
                rProgress += Time.deltaTime;
                progressBar.fillAmount = rProgress/reloadSpeed;
            }
        }
        else{
            rProgress = 0;
        }
        if(currentAmmo == maxAmmo){
            fullAmmo = true;
        }
        else{
            fullAmmo = false;
        }
        if(rageScript == null){
            rageScript = FindObjectOfType<Rage>();
        }
    }
    public void Reload(){
        if(currentReserveAmmo > 0){
            StartCoroutine(ReloadTimer());
        }
    }
    IEnumerator ReloadTimer(){
        reloading = true;
        progressBar.gameObject.SetActive(true);
        yield return new WaitForSeconds(reloadSpeed);
        progressBar.gameObject.SetActive(false);
        var savedAmmo = currentReserveAmmo;
        currentReserveAmmo += (currentAmmo - maxAmmo);
        if(currentReserveAmmo >=  maxAmmo){
            currentAmmo = maxAmmo;
        }
        else{
            currentAmmo = savedAmmo;
        }
        progressBar.fillAmount = 0f;
        reloadProgress = 0f;
        reloading = false;
    }
}
