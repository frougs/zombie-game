using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MysteryCrateSelectedItem : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject buyItem;
    [SerializeField] bool gunMode;
    [SerializeField] bool ammoMode;
    [SerializeField] GameObject spawnPOS;
    [SerializeField] GameObject examplePOS;
    [SerializeField] int forSaleID;
    [SerializeField] TextMeshPro pickupText;
    [SerializeField] Text getText;
    [HideInInspector] public MysteryCrate crate;

    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
            if(gunMode){
                Instantiate(buyItem, spawnPOS.transform.position, Quaternion.identity);
                if(crate != null){
                    crate.CloseBox();
                }
                Destroy(this.gameObject);
                }
            
            if(ammoMode){
                    interactionCon.currentlyHeld.GetComponent<BaseGun>().currentReserveAmmo = interactionCon.currentlyHeld.GetComponent<BaseGun>().maxReserveAmmo;
                    interactionCon.currentlyHeld.GetComponent<BaseGun>().currentAmmo = interactionCon.currentlyHeld.GetComponent<BaseGun>().maxAmmo;
                    if(crate != null){
                        crate.CloseBox();
                    }
                    Destroy(this.gameObject);
            }
    }
    public void Update(){
        pickupText.text = "Pickup: [ " + getText.text +" ]";
        if(FindObjectOfType<InteractionController>().currentHeldID == forSaleID){
            ammoMode = true;
            gunMode = false;
        }
        else{
            gunMode = true;
            ammoMode = false;  
        }
    }
    private void Start(){
        Instantiate(buyItem.GetComponent<BaseGun>().buyModel, examplePOS.transform.position, examplePOS.transform.rotation, examplePOS.transform);
        forSaleID = buyItem.GetComponentInChildren<PickupController>().itemID;
    }
}
