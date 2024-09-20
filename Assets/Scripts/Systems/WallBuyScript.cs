using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WallBuyScript : Purchasable, IInteractable
{
    [SerializeField] GameObject buyItem;
    //[SerializeField] int itemPrice;
    [SerializeField] bool gunMode;
    [SerializeField] bool ammoMode;
    [SerializeField] GameObject spawnPOS;
    [SerializeField] TextMeshPro priceText;
    [SerializeField] TextMeshPro itemName;
    [SerializeField] GameObject examplePOS;
    [SerializeField] int forSaleID;
    [SerializeField] public AudioSource soundSource;
    [SerializeField] public AudioClip errorPurchase;
    [SerializeField] public AudioClip purchase;

    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        if(gunMode){
            var scoreSystem = interactionCon.GetComponent<ScoreSystem>();
            if(scoreSystem.score >= price){
            scoreSystem.SubtractScore(price);
            Instantiate(buyItem, spawnPOS.transform.position, Quaternion.identity);
            }
            else{
                soundSource.PlayOneShot(errorPurchase);
            }
        }
        if(ammoMode){
            var scoreSystem = interactionCon.GetComponent<ScoreSystem>();
            if(scoreSystem.score >= buyItem.GetComponent<BaseGun>().ammoPrice){
                scoreSystem.SubtractScore(price);
                interactionCon.currentlyHeld.GetComponent<BaseGun>().currentReserveAmmo = interactionCon.currentlyHeld.GetComponent<BaseGun>().maxReserveAmmo;
                interactionCon.currentlyHeld.GetComponent<BaseGun>().currentAmmo = interactionCon.currentlyHeld.GetComponent<BaseGun>().maxAmmo;
            }
            else{
                soundSource.PlayOneShot(errorPurchase);
            }
        }

    }
    public void Update(){
        if(FindObjectOfType<InteractionController>().currentHeldID == forSaleID){
            //price = buyItem.GetComponent<BaseGun>().ammoPrice;
 
            price = discountApplied ? buyItem.GetComponent<BaseGun>().ammoPrice - (int)(buyItem.GetComponent<BaseGun>().ammoPrice * discountAmount) : buyItem.GetComponent<BaseGun>().ammoPrice;
            
            ammoMode = true;
            gunMode = false;
        }
        else{
            //price = buyItem.GetComponent<BaseGun>().gunPrice;
            price = discountApplied ? buyItem.GetComponent<BaseGun>().gunPrice - (int)(buyItem.GetComponent<BaseGun>().gunPrice * discountAmount) : buyItem.GetComponent<BaseGun>().gunPrice;
            gunMode = true;
            ammoMode = false;  
        }
        // else if(FindObjectOfType<InteractionController>().currentlyHeld != buyItem || FindObjectOfType<InteractionController>().holdingItem == false){
        //     itemPrice = buyItem.GetComponent<BaseGun>().gunPrice;
        //     gunMode = true;
        //     ammoMode = false;
        // }
        priceText.text = "$" + price.ToString();
        if(!ammoMode){
            itemName.text = buyItem.GetComponentInChildren<RarityManager>().itemName;
            itemName.color = buyItem.GetComponentInChildren<RarityManager>().GetItemColor();
        }
        else{
            itemName.text = "Ammo";
            itemName.color = Color.white;
        }
    }
    private void Start(){
        Instantiate(buyItem.GetComponent<BaseGun>().buyModel, examplePOS.transform.position, examplePOS.transform.rotation, examplePOS.transform);
        forSaleID = buyItem.GetComponentInChildren<PickupController>().itemID;
        if(soundSource == null){
            soundSource = this.GetComponentInChildren<AudioSource>();
        }
    }
}
