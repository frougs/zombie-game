using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAOE : MonoBehaviour
{
    [SerializeField] LightningHammerImpact hammer;
    private List<GameObject> damagables = new List<GameObject>();
    private bool sendListOnce = true;
    private void OnTriggerEnter(Collider obj){
        //hammer.DamageablesInRange(obj.gameObject);
        if(obj.GetComponent<IDamagable>() != null && obj.GetComponent<ThirdPersonController>() == null && obj.GetComponent<BarrierScript>() == null){
            damagables.Add(obj.gameObject);
        }
    }
    private void Update(){
        if(hammer.impact && sendListOnce == true){
            sendListOnce = false;
            StartCoroutine(SendList());
        }
    }
    private IEnumerator SendList(){
        //Debug.Log("SendingList...");
        yield return new WaitForSeconds(hammer.minProjectileLifetime);
        //Debug.Log("List Sent!: " +damagables.Count);
        this.GetComponent<SphereCollider>().enabled = false;
        hammer.hammer.Hit(damagables);
        Destroy(hammer.gameObject);
    }
}
