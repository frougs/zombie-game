using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseGun : MonoBehaviour, IShootable
{
    [SerializeField] public float damage;
    [SerializeField] public float fireRate;
    [SerializeField] public int maxAmmo;
    [SerializeField] public int maxReserveAmmo;
    [SerializeField] public int currentReserveAmmo;
    [SerializeField] public float reloadSpeed;
    [SerializeField] public float range;
    [SerializeField] public float critMultiplier;
    [SerializeField] public int currentAmmo;
    [SerializeField] public int ammoPrice;
    [SerializeField] public int gunPrice;
    public bool canShoot = true;
    [HideInInspector] public bool hasAmmo = true;
    //private GameObject progressBar;
    [HideInInspector] public UIContainer uiStuff;
    public bool held;
    [HideInInspector] public Image progressBar;
    public float reloadProgress;
    [HideInInspector] public float rProgress;
    [HideInInspector] public bool reloading;
    [HideInInspector] public bool fullAmmo;
    [SerializeField] public AudioClip gunshot;
    [SerializeField] public AudioClip noAmmo;
    [SerializeField] public AudioClip reloadSound;
    [SerializeField] public AudioClip defaultHit;
    [SerializeField] public AudioClip criticalHit;
    public bool canPlayNoAmmo = true;
    public Coroutine playAmmoSoundCoroutine;
    [SerializeField] public AudioSource soundSource;
    [SerializeField] public int pointsPerHit;
    [HideInInspector] public ScoreSystem scoreSystem;
    [HideInInspector] public bool hasReserveAmmo;
    [SerializeField] public GameObject buyModel;
    public LayerMask IgnoreLayer;
    [HideInInspector] public Rage rageScript;
    [HideInInspector] public Symbiosis symbiosisScript;
    public float modifiedDamage;
    public WeaponController weaponCon;
    public float reloadSpeedAugment;
    [HideInInspector] public GameObject player;
    [SerializeField] public GameObject particleSpawnPOS;
    [SerializeField] public GameObject particlesOBJ;
    [SerializeField] public GameObject impactParticle;
    [SerializeField] public GameObject damageNumberParticles;
    

    private void Start(){
        currentAmmo = maxAmmo;
        currentReserveAmmo = maxReserveAmmo;
        if(soundSource == null){
            soundSource = this.GetComponentInChildren<AudioSource>();
        }
        weaponCon = FindObjectOfType<WeaponController>();
    }
    public virtual void Shot(GameObject shooter){
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
                if(Physics.Raycast(shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward, out RaycastHit hitData, range,  ~IgnoreLayer)){
                    
                    //Instantiate(impactParticle, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                    //Vector3 towardsPlayer = (player.transform.position - impParticle.transform.position).normalized;
                    //Debug.Log(hitData.transform.position);

                    IDamagable damagable = hitData.transform.gameObject.GetComponent<IDamagable>();
                    //NonCrit hit
                    //Debug.Log("hit: " +hitData.transform.gameObject.name);
                    if(hitData.transform.gameObject != player){
                        if(damagable != null && hitData.transform.gameObject.tag != "CriticalSpot"){
                            try{
                                var dmgParticles = Instantiate(damageNumberParticles, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                                dmgParticles.GetComponent<CFXR_ParticleText>().text = modifiedDamage.ToString();
                                dmgParticles.GetComponent<CFXR_ParticleText>().UpdateText();
                            }
                            catch(Exception e){
                                Debug.LogWarning(e.ToString());
                            }
                            if(symbiosisScript != null){
                                if(symbiosisScript.lifeSteal && player != null){
                                    player.GetComponent<PlayerHealth>().currentHealth += modifiedDamage * symbiosisScript.lifeStealPercent;
                                }
                            }
                            damagable.Damaged(modifiedDamage, shooter, hitData.point);
                            if(soundSource != null){
                                soundSource.PlayOneShot(defaultHit);
                            }
                            scoreSystem.AddToScore(pointsPerHit);
                            canShoot = false;
                            rageScript.Hit();
                        }
                        //Crit hit
                        else if(damagable != null && hitData.transform.gameObject.tag == "CriticalSpot"){
                            try{
                                var dmgParticles = Instantiate(damageNumberParticles, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                                dmgParticles.GetComponent<CFXR_ParticleText>().text = (modifiedDamage * critMultiplier).ToString();
                                dmgParticles.GetComponent<CFXR_ParticleText>().UpdateText();
                            }
                            catch(Exception e){
                                Debug.LogWarning(e.ToString());
                            }
                            if(symbiosisScript != null){
                                if(symbiosisScript.lifeSteal && player != null){
                                    player.GetComponent<PlayerHealth>().currentHealth += modifiedDamage * symbiosisScript.lifeStealPercent;
                                }
                            }
                            if(soundSource != null){
                                soundSource.PlayOneShot(criticalHit);
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
                //Debug.LogWarning("*CLICK* no ammo!");
                if(canPlayNoAmmo){
                    if(playAmmoSoundCoroutine != null){
                        StopCoroutine(playAmmoSoundCoroutine);
                    }
                    playAmmoSoundCoroutine = StartCoroutine(NoAmmoSound());
                    //soundSource.PlayOneShot(noAmmo);
                }
                
            }
        }
    }
    public IEnumerator NoAmmoSound(){
        canPlayNoAmmo = false;
        soundSource.PlayOneShot(noAmmo);
        yield return new WaitForSeconds(fireRate);
        canPlayNoAmmo = true;
    }
    public IEnumerator ShotDelay(){
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
        if(soundSource != null){
            soundSource.PlayOneShot(reloadSound);
        }
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
    public void RefillAmmo(){
        currentAmmo = maxAmmo;
        currentReserveAmmo = maxReserveAmmo;
    }
}
