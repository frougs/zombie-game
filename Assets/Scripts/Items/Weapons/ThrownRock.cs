using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownRock : MonoBehaviour
{
    [SerializeField] GameObject impactParticles;
    private float rockDamage;
    private GameObject player;
    private int scorePerHit;
    private float critPercent;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip critSound;
        private void OnCollisionEnter(Collision obj){
            //Debug.Log("Rock Hit: " +obj.gameObject.name);
            if(obj.gameObject.GetComponent<IDamagable>() != null && obj.gameObject.GetComponent<ThirdPersonController>() == null && obj.gameObject.GetComponent<BarrierScript>() == null){
                IDamagable damagableOBJ = obj.gameObject.GetComponent<IDamagable>();
                if(damagableOBJ != null){
                    if(obj.gameObject.tag == "CriticalSpot"){
                        damagableOBJ.Damaged(rockDamage * critPercent, player, this.transform.position);
                        player.GetComponent<AudioSource>().PlayOneShot(critSound);
                    }
                    else if(obj.gameObject.tag != "CriticalSpot"){
                        damagableOBJ.Damaged(rockDamage, player, this.transform.position);
                        player.GetComponent<AudioSource>().PlayOneShot(hitSound);
                    }
                    player.GetComponent<ScoreSystem>().AddToScore(scorePerHit);
                    
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
    
}
