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
}
