using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JitterbugExplosion : MonoBehaviour
{
    public float fieldDuration;
    public float fieldDamage;
    public float fieldDamageInterval;
    public List<GameObject> inRange = new List<GameObject>();
    public GameObject player;
    public GameObject damageNumberParticles;
    private void Start(){
        Destroy(this.gameObject, fieldDuration);
        player = FindObjectOfType<ThirdPersonController>().gameObject;
        StartCoroutine(DamageDelay());
    }
    private void OnTriggerEnter(Collider other){
        //Debug.Log("Collision Detected with: " +other.gameObject.name);
        IDamagable damagableOBJ = other.gameObject.GetComponent<IDamagable>();
        if(damagableOBJ != null && other.gameObject != player && other.gameObject.GetComponent<BarrierScript>() == null){
            //Debug.Log("Collision is damagable");
            //IDamagable damagableOBJ = other.gameObject.GetComponent<IDamagable>();
            if(!inRange.Contains(other.gameObject)){
                inRange.Add(other.gameObject);
            }
            
        }
        if(other.gameObject.GetComponent<JitterbugExplosion>() != null){
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.gameObject.GetComponent<IDamagable>() != null && other.gameObject.GetComponent<ThirdPersonController>() == null && other.gameObject.GetComponent<BarrierScript>() == null){
            IDamagable damagableOBJ = other.gameObject.GetComponent<IDamagable>();
            if(damagableOBJ != null){
                if(inRange.Contains(other.gameObject)){
                    inRange.Remove(other.gameObject);
                }
            }
        }
    }
    IEnumerator DamageDelay(){
        //Debug.Log("DamageDelayStarted");
        
        yield return new WaitForSeconds(fieldDamageInterval);
        foreach(GameObject obj in inRange){
            if (obj == null) // Skip null entries
            {
                continue;
            }
            obj.GetComponent<IDamagable>().Damaged(fieldDamage, this.gameObject, this.transform.position);
            UpdateParticle(obj, fieldDamage);
            //Debug.Log("Damaging: " +obj.name +" For Damage: " +fieldDamage);
        }
        StartCoroutine(DamageDelay());
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
