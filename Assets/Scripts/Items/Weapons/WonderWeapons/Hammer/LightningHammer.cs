using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightningHammer : BaseGun
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
                    GameObject launchedProj = Instantiate(projectile, shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, Quaternion.LookRotation(-1 * (shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position - transform.position)));
                    launchedProj.GetComponent<LightningHammerImpact>().hammer = this;
                    launchedProj.GetComponent<LightningHammerImpact>().player = player;
                    Rigidbody rb = launchedProj.GetComponent<Rigidbody>();
                    if(rb != null){
                        rb.velocity =  shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward * projectileSpeed;
                    }
                }




                // //This stuff gonna change for hammer but I still need to copy some of it!
                // if(Physics.Raycast(shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, shooter.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward, out RaycastHit hitData, range,  ~IgnoreLayer)){
                    
                //     //Instantiate(impactParticle, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                //     //Vector3 towardsPlayer = (player.transform.position - impParticle.transform.position).normalized;
                //     //Debug.Log(hitData.transform.position);

                //     IDamagable damagable = hitData.transform.gameObject.GetComponent<IDamagable>();
                //     //NonCrit hit
                //     //Debug.Log("hit: " +hitData.transform.gameObject.name);
                //     if(hitData.transform.gameObject != player){
                //         if(damagable != null && hitData.transform.gameObject.tag != "CriticalSpot"){
                //             try{
                //                 var dmgParticles = Instantiate(damageNumberParticles, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                //                 dmgParticles.GetComponent<CFXR_ParticleText>().text = modifiedDamage.ToString();
                //                 dmgParticles.GetComponent<CFXR_ParticleText>().UpdateText();
                //             }
                //             catch(Exception e){
                //                 Debug.LogWarning(e.ToString());
                //             }
                //             if(symbiosisScript != null){
                //                 if(symbiosisScript.lifeSteal && player != null){
                //                     player.GetComponent<PlayerHealth>().currentHealth += modifiedDamage * symbiosisScript.lifeStealPercent;
                //                 }
                //             }
                //             damagable.Damaged(modifiedDamage, shooter, hitData.point);
                //             if(soundSource != null){
                //                 soundSource.PlayOneShot(defaultHit);
                //             }
                //             scoreSystem.AddToScore(pointsPerHit);
                //             canShoot = false;
                //             rageScript.Hit();
                //         }
                //         //Crit hit
                //         else if(damagable != null && hitData.transform.gameObject.tag == "CriticalSpot"){
                //             try{
                //                 var dmgParticles = Instantiate(damageNumberParticles, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                //                 dmgParticles.GetComponent<CFXR_ParticleText>().text = (modifiedDamage * critMultiplier).ToString();
                //                 dmgParticles.GetComponent<CFXR_ParticleText>().UpdateText();
                //             }
                //             catch(Exception e){
                //                 Debug.LogWarning(e.ToString());
                //             }
                //             if(symbiosisScript != null){
                //                 if(symbiosisScript.lifeSteal && player != null){
                //                     player.GetComponent<PlayerHealth>().currentHealth += modifiedDamage * symbiosisScript.lifeStealPercent;
                //                 }
                //             }
                //             if(soundSource != null){
                //                 soundSource.PlayOneShot(criticalHit);
                //             }
                //             damagable.Damaged(modifiedDamage * critMultiplier, shooter, hitData.point);
                //             scoreSystem.AddToScore((int)(pointsPerHit * critMultiplier));
                //             canShoot = false;
                //             rageScript.CritHit();
                //         }
                //         else{
                //             Instantiate(impactParticle, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
                //             //Debug.LogWarning("Damagable not found");
                //         }
                //     }
                // }





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
    public void Hit(List<GameObject> damagables){
        if(damagables.Count > 0){
            //Debug.Log("HitSomething hopefullyonce!!");
            if(soundSource != null){
                soundSource.PlayOneShot(criticalHit);
            }
            if(symbiosisScript != null){
                if(symbiosisScript.lifeSteal && player != null){
                    player.GetComponent<PlayerHealth>().currentHealth += modifiedDamage * symbiosisScript.lifeStealPercent;
                }
            }
            scoreSystem.AddToScore((int)(pointsPerHit * critMultiplier));
            canShoot = false;
            rageScript.CritHit();
        }
        foreach(GameObject damagable in damagables){
            IDamagable damagableOBJ = damagable.GetComponent<IDamagable>();
            if(damagableOBJ != null){
                if(damagable != player && damagable.GetComponent<BarrierScript>() == null){   


                    // if(soundSource != null){
                    //     soundSource.PlayOneShot(criticalHit);
                    // }
                    if(!InstaKillActive){
                        damagableOBJ.Damaged(modifiedDamage * critMultiplier, player, damagable.transform.position);
                    }
                    else{
                        damagableOBJ.Damaged(Mathf.Infinity, player, damagable.transform.position);
                    }
                }
            }
        }
        // else{
        //     Instantiate(impactParticle, hitData.point, Quaternion.LookRotation((player.transform.position - hitData.point).normalized));
        //         //Debug.LogWarning("Damagable not found");
        // }

    }
    public void PlayImpactSound(Vector3 pos){
        impactAudioSource.transform.position = pos;
        impactAudioSource.GetComponent<AudioSource>().PlayOneShot(impactSound);
    }   
}
