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
    private Symbiosis symbiosisScript;
    float modifiedDamage;
    public WeaponController weaponCon;
    public float reloadSpeedAugment;
    private GameObject player;
    [SerializeField] GameObject particleSpawnPOS;
    [SerializeField] GameObject particlesOBJ;
    [SerializeField] GameObject impactParticle;
    

    private void Start(){
        currentAmmo = maxAmmo;
        currentReserveAmmo = maxReserveAmmo;
        soundSource = this.GetComponent<AudioSource>();
        weaponCon = FindObjectOfType<WeaponController>();
    }
    public void Shot(GameObject shooter){
        if(shooter.GetComponent<ThirdPersonController>() != null){
            player = shooter;
        }
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
                            if(particlesOBJ != null){
                                //particles.toggle = true;
                                //particles.ParticleBurst();
                                Instantiate(particlesOBJ, particleSpawnPOS.transform);
                            }
                }
                if(Physics.Raycast(shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward, out RaycastHit hitData, Mathf.Infinity,  ~IgnoreLayer)){
                    
                    //Instantiate(impactParticle, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                    //Vector3 towardsPlayer = (player.transform.position - impParticle.transform.position).normalized;
                    //Debug.Log(hitData.transform.position);

                    IDamagable damagable = hitData.transform.gameObject.GetComponent<IDamagable>();
                    //NonCrit hit
                    //Debug.Log("hit: " +hitData.transform.gameObject.name);
                    if(hitData.transform.gameObject != player){
                        if(damagable != null && hitData.transform.gameObject.tag != "CriticalSpot"){
                            if(symbiosisScript != null){
                                if(symbiosisScript.lifeSteal && player != null){
                                    player.GetComponent<PlayerHealth>().currentHealth += modifiedDamage * symbiosisScript.lifeStealPercent;
                                }
                            }
                            damagable.Damaged(modifiedDamage, shooter, hitData.point);
                            scoreSystem.AddToScore(pointsPerHit);
                            canShoot = false;
                            rageScript.Hit();
                        }
                        //Crit hit
                        else if(damagable != null && hitData.transform.gameObject.tag == "CriticalSpot"){
                            if(symbiosisScript != null){
                                if(symbiosisScript.lifeSteal && player != null){
                                    player.GetComponent<PlayerHealth>().currentHealth += modifiedDamage * symbiosisScript.lifeStealPercent;
                                }
                            }
                            damagable.Damaged(modifiedDamage * critMultiplier, shooter, hitData.point);
                            scoreSystem.AddToScore((int)(pointsPerHit * critMultiplier));
                            canShoot = false;
                            rageScript.CritHit();
                        }
                        else{
                            Instantiate(impactParticle, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                            //Debug.LogWarning("Damagable not found");
                        }
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
                if(reloadSpeedAugment == 0){
                    progressBar.fillAmount = rProgress/reloadSpeed;
                }
                else{
                    progressBar.fillAmount = rProgress/(reloadSpeed * reloadSpeedAugment);
                }
                
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
        /*if(FindObjectOfType<WeaponController>() == null && weaponCon == null){
            weaponCon = FindObjectOfType<WeaponController>();
        }
        else{
            reloadSpeedAugment = weaponCon.reloadSpeedAugment;
        }*/
        if(weaponCon.reloadSpeedAugment!= 0){
            reloadSpeedAugment = weaponCon.reloadSpeedAugment;
        }
        if(symbiosisScript == null){
            symbiosisScript = FindObjectOfType<Symbiosis>();
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
        if(reloadSpeedAugment == 0){
            yield return new WaitForSeconds(reloadSpeed);
        }
        else{
            yield return new WaitForSeconds(reloadSpeed * reloadSpeedAugment);
        }
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
