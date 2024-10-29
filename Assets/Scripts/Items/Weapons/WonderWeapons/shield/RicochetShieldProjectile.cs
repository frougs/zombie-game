using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RicochetShieldProjectile : MonoBehaviour
{
    [SerializeField] float ricochetMaxDistance;
    [SerializeField] float maxBounces;
    [HideInInspector] public RicochetShield shield;
    [SerializeField] float projSpeed;
    [SerializeField] float projMaxLifetime;
    [HideInInspector] public GameObject player;
    public bool hitSomething = false;
    private GameObject currentTarget;
    private bool moveToTarget;
    private bool returningToPlayer;
    [SerializeField] GameObject impactParticles;
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.GetComponent<IDamagable>() != null && other.gameObject.GetComponent<ThirdPersonController>() == null && other.gameObject.GetComponent<BarrierScript>() == null){
            shield.Hit(other.gameObject);
            SendImpactInfo();
        }
    }
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.GetComponent<IDamagable>() != null && collision.gameObject.GetComponent<ThirdPersonController>() == null && collision.gameObject.GetComponent<BarrierScript>() == null){
            shield.Hit(collision.gameObject);
            if(maxBounces > 0){
                Debug.Log("Finding Next target..");
                FindNextTarget();
                SendImpactInfo();
            }
        }
        else{
            ReturnToPlayer();
            SendImpactInfo();
        }
        if(collision.gameObject != null && collision.gameObject.GetComponent<ThirdPersonController>() == null){
            hitSomething = true;
        }
    }

    private void FindNextTarget(){
        this.GetComponent<Rigidbody>().useGravity = false;
        //Debug.Log("Finding Next Target");
        var damageables = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IDamagable>();
        if(damageables == null){
            ReturnToPlayer();
        }
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        foreach(var damageable in damageables){
            GameObject obj = (damageable as MonoBehaviour).gameObject;
            if(obj.gameObject.GetComponent<BarrierScript>() == null && obj.gameObject.GetComponent<ThirdPersonController>() == null){
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                Debug.Log("Damageable: " +obj.name +" Distance: " +distance);
                if(distance < ricochetMaxDistance){
                    if(distance < closestDistance){
                        closestObject = obj;
                        closestDistance = distance;
                    }
                }
            }
        }
        //if(closestObject != null){
            //Debug.Log("Next Target Found: " +closestObject.name +" Distance:" +Vector3.Distance(transform.position, closestObject.transform.position));
            currentTarget = closestObject;
            moveToTarget = true;
        //}
        // else{
        //     //Debug.Log("No valid objects in range, returning to player...");
        //     currentTarget = player;
        // }
        maxBounces -= 1;
    }
    private void ReturnToPlayer(){
        if(currentTarget == null && maxBounces <= 0){
            this.GetComponent<Rigidbody>().useGravity = false;
            //Debug.Log("Returning to player");
            this.GetComponent<Collider>().isTrigger = true;
            //MoveToTarget(player);
            currentTarget = player;
            moveToTarget = true;
            returningToPlayer = true;
        }
    }
    public void Start(){
        Destroy(this.gameObject, projMaxLifetime);
    }
    private void FixedUpdate(){
        if(maxBounces <= 0){
            ReturnToPlayer();
        }
        if(this.GetComponent<Rigidbody>().velocity == Vector3.zero){
            //ReturnToPlayer();
        }
        //Debug.Log("Distance between player: " +Vector3.Distance(transform.position, player.transform.position));
        if(Vector3.Distance(transform.position, player.transform.position) <= 2f && hitSomething){
            //shield.canShoot = true;
            Destroy(this.gameObject);
        }
        if(moveToTarget && currentTarget != null){
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Vector3 direction = (currentTarget.transform.position - this.transform.position).normalized;
            this.GetComponent<Rigidbody>().velocity = direction * projSpeed;
        }
        if(currentTarget == null && returningToPlayer == false && hitSomething){
            FindNextTarget();
        }
    }
    private void SendImpactInfo(){
        shield.PlayImpactSound(this.transform.position);
        Instantiate(impactParticles, this.transform.position, Quaternion.identity);
    }
}
