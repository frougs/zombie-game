using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrownRock : MonoBehaviour
{
    [SerializeField] GameObject impactParticles;
    private float rockDamage;
    private GameObject player;
    private int scorePerHit;
    private float critPercent;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip critSound;
    [SerializeField] AudioSource soundSource;
    [SerializeField] GameObject damageNumberParticles;
    private bool hitOnce = false;
        private void OnCollisionEnter(Collision obj){
            //Debug.Log("Rock Hit: " +obj.gameObject.name);
            if(obj.gameObject.GetComponent<IDamagable>() != null && obj.gameObject.GetComponent<ThirdPersonController>() == null && obj.gameObject.GetComponent<BarrierScript>() == null){
                IDamagable damagableOBJ = obj.gameObject.GetComponent<IDamagable>();
                if(damagableOBJ != null && hitOnce == false){
                    hitOnce = true;
                    float damageAmount = 0f;
                    if(obj.gameObject.tag == "CriticalSpot"){
                        damagableOBJ.Damaged(rockDamage * critPercent, player, this.transform.position);
                        damageAmount = rockDamage * critPercent;
                        player.GetComponent<WeaponController>().soundSource.PlayOneShot(critSound);
                    }
                    else if(obj.gameObject.tag != "CriticalSpot"){
                        damagableOBJ.Damaged(rockDamage, player, this.transform.position);
                        damageAmount = rockDamage;
                        player.GetComponent<WeaponController>().soundSource.PlayOneShot(hitSound);
                    }
                    player.GetComponent<ScoreSystem>().AddToScore(scorePerHit);
                    UpdateParticle(obj.gameObject, damageAmount);
                }
            }

            if(impactParticles != null){
                Instantiate(impactParticles, this.transform.position, Quaternion.LookRotation((player.transform.position - this.transform.position).normalized));
            }
            
            Destroy(this.gameObject);
        }
        public void AssignVariables(float damage, GameObject player, int rockScorePerHit, float critAmnt){
            this.rockDamage = damage;
            this.player = player;
            this.scorePerHit = rockScorePerHit;
            this.critPercent = critAmnt;
        }
    private void UpdateParticle(GameObject hitObj, float damageAmount){
        try{
        damageAmount = Mathf.Floor(damageAmount);
        bool foundCurrentParticle = false;
        var activeDamageNumbers = FindObjectsOfType<AlreadyActiveDamageParticle>();
        var damageableIDGenerator = hitObj.GetComponentInParent<DamageableIDGenerator>();
        if(damageableIDGenerator == null){
            damageableIDGenerator = hitObj.GetComponent<DamageableIDGenerator>();
        }
        //Debug.Log(damageableIDGenerator.ID);
        if(activeDamageNumbers != null && activeDamageNumbers.Length > 0){
            foreach (var particle in activeDamageNumbers){
                if (damageableIDGenerator != null && particle.GetComponent<AlreadyActiveDamageParticle>().enemyID == damageableIDGenerator.ID){
                    //Reset the particle
                    foundCurrentParticle = true;
                    particle.GetComponent<AlreadyActiveDamageParticle>().ResetParticle(damageAmount, this.transform.position);
                    //Debug.Log("Found Currently Active Particle");
                    break;
                }
            }
        }
        if(!foundCurrentParticle){
            //Debug.Log("No currently active particle found.. Making one and assigning values");
            //Spawn New particle and assign the ID
            var newDmgParticles = Instantiate(damageNumberParticles, this.transform.position, Quaternion.LookRotation((player.transform.position - this.transform.position).normalized));
            newDmgParticles.GetComponent<AlreadyActiveDamageParticle>().NewParticle(damageAmount);
            newDmgParticles.GetComponent<AlreadyActiveDamageParticle>().enemyID = damageableIDGenerator.ID;
        }
        }
        catch(Exception e){
            Debug.LogWarning(e.ToString());
        }
    }
}
