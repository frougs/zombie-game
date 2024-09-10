using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningHammerImpact : MonoBehaviour
{
    [HideInInspector] public LightningHammer hammer;
    [SerializeField] GameObject areaOfEffect;
    [SerializeField] GameObject impactParticle;
    [HideInInspector] public GameObject player;
    [SerializeField] float maxprojectileLifetime;
    [SerializeField] public float minProjectileLifetime;
    [HideInInspector] public bool impact;
    private bool sendOnce = true;
    /*public void DamageablesInRange(List<GameObject> damagables){
        //hammer.Hit(obj);
        if(sendOnce = true){
            hammer.Hit(damagables);
            sendOnce = false;
        }
        
    }*/
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.GetComponent<ThirdPersonController>() == null){
            impact = true;
            areaOfEffect.SetActive(true);
            Instantiate(impactParticle, this.transform.position, Quaternion.identity);
            hammer.PlayImpactSound(this.transform.position);
            //Destroy(this.gameObject);
        }
    }
    private void Start(){
        //Destroy(this.gameObject, projectileLifetime);
    }
}
