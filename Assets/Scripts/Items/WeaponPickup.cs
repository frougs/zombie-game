using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    [SerializeField] float dropForce;
    [SerializeField] GameObject item;
    public void Interacted(GameObject player){
        var gunRoot = player.GetComponent<WeaponController>().gunRoot;
        //Debug.Log(gunRoot);
        if(gunRoot != null){
            this.GetComponent<BoxCollider>().enabled = false;
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.GetComponent<BoxCollider>().enabled = false;
            item.transform.position = gunRoot.transform.position;
            item.transform.rotation = gunRoot.transform.rotation;
            item.transform.SetParent(gunRoot.transform);
            this.GetComponent<ItemInteractionPopup>().Held();
        }
    }
    public void Dropped(GameObject player){
        this.GetComponent<BoxCollider>().enabled = true;
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.GetComponent<BoxCollider>().enabled = true;
        item.transform.SetParent(null);
        item.GetComponent<Rigidbody>().AddForce(player.transform.forward * dropForce, ForceMode.Impulse);
    }
}
