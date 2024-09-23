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
    public List<string> upgrades = new List<string>();
    private Coroutine reloadRoutine;
    public bool InstaKillActive;
    public bool infiniteAmmo;

    private void Start(){
        currentAmmo = maxAmmo;
        currentReserveAmmo = maxReserveAmmo;
        if(soundSource == null){
            soundSource = this.GetComponentInChildren<AudioSource>();
        }
        weaponCon = FindObjectOfType<WeaponController>();
    }
    public virtual void Shot(GameObject shooter){
        //Debug.Log("Shot Triggered");
        if(shooter.GetComponent<ThirdPersonController>() != null){
            player = shooter;
        }
        if(reloading == false){
            if(canShoot && hasAmmo){
                try{
                if(rageScript.doubledamage){
                    modifiedDamage = damage * 2f;
                }
                else{
                    modifiedDamage = damage;
                }
                }
                catch(Exception e){}
                if(!infiniteAmmo){
                    currentAmmo -= 1;
                }
                if(soundSource != null){
                    soundSource.PlayOneShot(gunshot);
                            if(particlesOBJ != null){
                                //particles.toggle = true;
                                //particles.ParticleBurst();
                                Instantiate(particlesOBJ, particleSpawnPOS.transform);
                            }
                }
                if(Physics.Raycast(shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward, out RaycastHit hitData, range,  ~IgnoreLayer)){
                    //Debug.Log("Shooting raycast");
                    //Instantiate(impactParticle, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                    //Vector3 towardsPlayer = (player.transform.position - impParticle.transform.position).normalized;
                    //Debug.Log(hitData.transform.position);

                    IDamagable damagable = hitData.transform.gameObject.GetComponent<IDamagable>();
                    //NonCrit hit
                    //Debug.Log("hit: " +hitData.transform.gameObject.name);
                    if(hitData.transform.gameObject != player){
                        float damageAmount = 0f;
                        if(damagable != null && hitData.transform.gameObject.tag != "CriticalSpot"){
                            // try{
                                
                            // }
                            // catch(Exception e){
                            //     Debug.LogWarning(e.ToString());
                            // }
                            //var dmgParticles = Instantiate(damageNumberParticles, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                            if(symbiosisScript != null){
                                if(symbiosisScript.lifeSteal && player != null){
                                    player.GetComponent<PlayerHealth>().currentHealth += modifiedDamage * symbiosisScript.lifeStealPercent;
                                }
                            }
                            if(InstaKillActive == false){
                                damagable.Damaged(modifiedDamage, shooter, hitData.point);
                                //dmgParticles.GetComponent<CFXR_ParticleText>().text = modifiedDamage.ToString();
                                damageAmount = modifiedDamage;
                            }
                            else{
                                damagable.Damaged(Mathf.Infinity, shooter, hitData.point);
                                //dmgParticles.GetComponent<CFXR_ParticleText>().text = Mathf.Infinity.ToString();
                                damageAmount = Mathf.Infinity;
                            }
                            if(soundSource != null){
                                soundSource.PlayOneShot(defaultHit);
                            }
                            //dmgParticles.GetComponent<CFXR_ParticleText>().UpdateText();
                            scoreSystem.AddToScore(pointsPerHit);
                            canShoot = false;
                            rageScript.Hit();
                        }
                        //Crit hit
                        else if(damagable != null && hitData.transform.gameObject.tag == "CriticalSpot"){
                            // try{
                            //     var dmgParticles = Instantiate(damageNumberParticles, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                            // }
                            // catch(Exception e){
                            //     Debug.LogWarning(e.ToString());
                            // }
                            //var dmgParticles = Instantiate(damageNumberParticles, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                            if(symbiosisScript != null){
                                if(symbiosisScript.lifeSteal && player != null){
                                    player.GetComponent<PlayerHealth>().currentHealth += modifiedDamage * symbiosisScript.lifeStealPercent;
                                }
                            }
                            if(soundSource != null){
                                soundSource.PlayOneShot(criticalHit);
                            }
                            if(InstaKillActive == false){
                                //dmgParticles.GetComponent<CFXR_ParticleText>().text = (modifiedDamage * critMultiplier).ToString();
                                damageAmount = modifiedDamage * critMultiplier;
                                damagable.Damaged(modifiedDamage * critMultiplier, shooter, hitData.point);
                            }
                            else{
                                damagable.Damaged(Mathf.Infinity, shooter, hitData.point);
                                //dmgParticles.GetComponent<CFXR_ParticleText>().text = Mathf.Infinity.ToString();
                                damageAmount = Mathf.Infinity;
                            }
                            //dmgParticles.GetComponent<CFXR_ParticleText>().UpdateText();
                            scoreSystem.AddToScore((int)(pointsPerHit * critMultiplier));
                            canShoot = false;
                            rageScript.CritHit();
                        }
                        else{
                            Instantiate(impactParticle, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                            //Debug.LogWarning("Damagable not found");
                        }
                        if(damagable != null){
                            UpdateParticle(hitData, damageAmount);
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
    private void UpdateParticle(RaycastHit hitData, float damageAmount){
        try{
        damageAmount = Mathf.Floor(damageAmount);
        bool foundCurrentParticle = false;
        var activeDamageNumbers = FindObjectsOfType<AlreadyActiveDamageParticle>();
        var damageableIDGenerator = hitData.transform.gameObject.GetComponentInParent<DamageableIDGenerator>();
        if(damageableIDGenerator == null){
            damageableIDGenerator = hitData.transform.gameObject.GetComponent<DamageableIDGenerator>();
        }
        //Debug.Log(damageableIDGenerator.ID);
        if(activeDamageNumbers != null && activeDamageNumbers.Length > 0){
            foreach (var particle in activeDamageNumbers){
                if (damageableIDGenerator != null && particle.GetComponent<AlreadyActiveDamageParticle>().enemyID == damageableIDGenerator.ID){
                    //Reset the particle
                    foundCurrentParticle = true;
                    particle.GetComponent<AlreadyActiveDamageParticle>().ResetParticle(damageAmount, hitData.point);
                    //Debug.Log("Found Currently Active Particle");
                    break;
                }
            }
        }
        if(!foundCurrentParticle){
            //Debug.Log("No currently active particle found.. Making one and assigning values");
            //Spawn New particle and assign the ID
            var newDmgParticles = Instantiate(damageNumberParticles, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
            newDmgParticles.GetComponent<AlreadyActiveDamageParticle>().NewParticle(damageAmount);
            newDmgParticles.GetComponent<AlreadyActiveDamageParticle>().enemyID = damageableIDGenerator.ID;
        }
        }
        catch(Exception e){
            Debug.LogWarning(e.ToString());
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
    public void StopReload(){
        if(reloadRoutine != null){
            StopCoroutine(reloadRoutine);
            progressBar.fillAmount = 0f;
            reloadProgress = 0f;
            reloading = false;
            progressBar.gameObject.SetActive(false);
            
        }
    }
    private void Update(){
        if(held == false){
            InstaKillActive = false;
            infiniteAmmo = false;
        }
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
        if(currentReserveAmmo > maxReserveAmmo){
            currentReserveAmmo = maxReserveAmmo;
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
            reloadRoutine = StartCoroutine(ReloadTimer());
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
        if(currentReserveAmmo >=  maxAmmo){
            currentReserveAmmo = currentReserveAmmo - (maxAmmo - currentAmmo);
            currentAmmo = maxAmmo;
        }
        else{
            currentReserveAmmo = 0;
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
