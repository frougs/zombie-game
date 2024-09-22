using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlreadyActiveDamageParticle : MonoBehaviour
{
    [SerializeField] public string enemyID;
    //private Vector3 startPOS;
    [SerializeField] GameObject damageParticle;
    private float damage;
    private void Start(){
        //startPOS = transform.position;
    }
    public void ResetParticle(float dmg, Vector3 location){
        //Debug.Log("Spawning Reset particle...");
        var dmgParticle = Instantiate(damageParticle, location, Quaternion.identity);
        var newDamage = damage += dmg;
        //Debug.Log("UPDATED DAMAGE: " + newDamage);
        dmgParticle.GetComponent<CFXR_ParticleText>().text = newDamage.ToString();
        dmgParticle.GetComponent<AlreadyActiveDamageParticle>().enemyID = enemyID;
        dmgParticle.GetComponent<AlreadyActiveDamageParticle>().damage = newDamage;
        dmgParticle.GetComponent<CFXR_ParticleText>().UpdateText();
        Destroy(this.gameObject);

    }

    private void FixedUpdate(){
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + Time.deltaTime, this.transform.position.z);
    }
    public void NewParticle(float dmg){
        damage = dmg;
        this.GetComponent<CFXR_ParticleText>().text = damage.ToString();
        this.GetComponent<CFXR_ParticleText>().UpdateText();
    }
}
