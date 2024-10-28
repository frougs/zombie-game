using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class JitterBugController : MonoBehaviour
{
    [Header("General Stuffs")]
    public bool perkPurchased;
    public bool reloadBuffActive;
    [Header("Chain Lightning Stuff")]
    [HideInInspector] public int maxChains;
    [HideInInspector] public float damage;
    [HideInInspector] public float chainRange;
    [HideInInspector] public float lightningDuration;
    [HideInInspector] public float reloadBuffAmount;
    [HideInInspector] public float reloadBuffDuration;
    [SerializeField] GameObject reloadBuffParticles;
    public GameObject lightningPrefab; // Prefab for the lightning effect
    // private LineRenderer lineRenderer;
    // public GameObject lightningParticles;

    public void StartChainLighting(GameObject initialHit){
        if(perkPurchased){
            //Debug.Log("Trying to do lightning shit");
            CreateLightning(initialHit);
        }
    }
    public void CreateLightning(GameObject initialHit){
        var visualLightning = Instantiate(lightningPrefab, initialHit.transform.position, Quaternion.identity);
        visualLightning.GetComponent<ChainLightning>().maxChains = maxChains;
        visualLightning.GetComponent<ChainLightning>().damage = damage;
        visualLightning.GetComponent<ChainLightning>().chainRange = chainRange;
        visualLightning.GetComponent<ChainLightning>().lightningDuration = lightningDuration;
        visualLightning.GetComponent<ChainLightning>().player = this.gameObject;
        visualLightning.GetComponent<ChainLightning>().StartLightning(initialHit);

    }
    public void Update(){
        if(this.GetComponent<InteractionController>().currentlyHeld != null){
            if(reloadBuffActive && this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject.GetComponent<BaseGun>() != null){
                this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject.GetComponent<BaseGun>().reloadBuffActive = true;
                this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject.GetComponent<BaseGun>().reloadBuffAmount = reloadBuffAmount;
                this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject.GetComponent<BaseGun>().reloadBuffDuration = reloadBuffDuration;
                this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject.GetComponent<BaseGun>().reloadBuffParticles = reloadBuffParticles;
            }
        }
    }
}
