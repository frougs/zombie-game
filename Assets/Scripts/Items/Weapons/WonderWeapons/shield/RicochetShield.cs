using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RicochetShield : BaseGun
{
    [Header("Projectile stuff")]
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;
    [SerializeField] GameObject impactAudioSource;
    [SerializeField] AudioClip impactSound;
    public override void Shot(GameObject shooter){
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
                if(projectile != null){
                    //GameObject launchedProj = Instantiate(projectile, shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, Quaternion.LookRotation(-1 * (shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position - transform.position)));
                    GameObject launchedProj = Instantiate(projectile, transform.position, Quaternion.identity);
                    launchedProj.GetComponent<RicochetShieldProjectile>().shield = this;
                    launchedProj.GetComponent<RicochetShieldProjectile>().player = player;
                    Rigidbody rb = launchedProj.GetComponent<Rigidbody>();
                    if(rb != null){
                        rb.velocity =  shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward * projectileSpeed;
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
    public void Hit(GameObject damagable)
    {
        if(soundSource != null){
            soundSource.PlayOneShot(criticalHit);
        }
        if(symbiosisScript != null){
            if(symbiosisScript.lifeSteal && player != null){
                player.GetComponent<PlayerHealth>().currentHealth += modifiedDamage * symbiosisScript.lifeStealPercent;
            }
        }
        scoreSystem.AddToScore((int)(pointsPerHit * critMultiplier));
        //canShoot = false;
        rageScript.CritHit();

        IDamagable damagableOBJ = damagable.GetComponent<IDamagable>();
        if(damagableOBJ != null){
            if(damagable != player && damagable.GetComponent<BarrierScript>() == null){
                if(!InstaKillActive){
                    damagableOBJ.Damaged(modifiedDamage * critMultiplier, player, damagable.transform.position);
                    UpdateParticle(damagable, modifiedDamage * critMultiplier);
                }
                else{
                    damagableOBJ.Damaged(Mathf.Infinity, player, damagable.transform.position);
                    UpdateParticle(damagable, Mathf.Infinity);
                }
            }
        }
    }
    private void UpdateParticle(GameObject hitData, float damageAmount){
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
                    particle.GetComponent<AlreadyActiveDamageParticle>().ResetParticle(damageAmount, hitData.transform.position);
                    //Debug.Log("Found Currently Active Particle");
                    break;
                }
            }
        }
        if(!foundCurrentParticle){
            //Spawn New particle and assign the ID
            var newDmgParticles = Instantiate(damageNumberParticles, hitData.transform.position, Quaternion.LookRotation((player.transform.position - hitData.transform.position).normalized));
            newDmgParticles.GetComponent<AlreadyActiveDamageParticle>().NewParticle(damageAmount);
            newDmgParticles.GetComponent<AlreadyActiveDamageParticle>().enemyID = damageableIDGenerator.ID;
        }
        }
        catch(Exception e){
            Debug.LogWarning(e.ToString());
        }
    }
    public void PlayImpactSound(Vector3 pos){
        impactAudioSource.transform.position = pos;
        impactAudioSource.GetComponent<AudioSource>().PlayOneShot(impactSound);
    }   

}
