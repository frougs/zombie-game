using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WallBuyScript : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject buyItem;
    [SerializeField] int itemPrice;
    [SerializeField] bool gunMode;
    [SerializeField] bool ammoMode;
    [SerializeField] GameObject spawnPOS;
    [SerializeField] TextMeshPro priceText;
    [SerializeField] GameObject examplePOS;
    [SerializeField] int forSaleID;

    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        if(gunMode){
            var scoreSystem = interactionCon.GetComponent<ScoreSystem>();
            if(scoreSystem.score >= itemPrice){
            scoreSystem.SubtractScore(itemPrice);
            Instantiate(buyItem, spawnPOS.transform.position, Quaternion.identity);
            }
        }
        if(ammoMode){
            var scoreSystem = interactionCon.GetComponent<ScoreSystem>();
            if(scoreSystem.score >= buyItem.GetComponent<BaseGun>().ammoPrice){
                scoreSystem.SubtractScore(itemPrice);
                interactionCon.currentlyHeld.GetComponent<BaseGun>().currentReserveAmmo = interactionCon.currentlyHeld.GetComponent<BaseGun>().maxReserveAmmo;
                interactionCon.currentlyHeld.GetComponent<BaseGun>().currentAmmo = interactionCon.currentlyHeld.GetComponent<BaseGun>().maxAmmo;
            }  
        }

    }
    public void Update(){
        if(FindObjectOfType<InteractionController>().currentHeldID == forSaleID){
            itemPrice = buyItem.GetComponent<BaseGun>().ammoPrice;
            ammoMode = true;
            gunMode = false;
        }
        else{
            itemPrice = buyItem.GetComponent<BaseGun>().gunPrice;
            gunMode = true;
            ammoMode = false;  
        }
        // else if(FindObjectOfType<InteractionController>().currentlyHeld != buyItem || FindObjectOfType<InteractionController>().holdingItem == false){
        //     itemPrice = buyItem.GetComponent<BaseGun>().gunPrice;
        //     gunMode = true;
        //     ammoMode = false;
        // }
        priceText.text = "$" + itemPrice.ToString();
    }
    private void Start(){
        Instantiate(buyItem.GetComponent<BaseGun>().buyModel, examplePOS.transform.position, examplePOS.transform.rotation, examplePOS.transform);
        forSaleID = buyItem.GetComponentInChildren<PickupController>().itemID;
    }
}
