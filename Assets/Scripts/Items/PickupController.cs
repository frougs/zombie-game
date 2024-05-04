using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class PickupController : AttributesSync, IInteractable
{
    public GameObject item;
    public Rigidbody rb;
    public BoxCollider thisColl;
    public BoxCollider objColl;
    public float dropForce;
    //public GameObject popupText;
    [SynchronizableField] bool currentlyHeld;
    private GameObject currentGunRoot;
    //private float storedMass;
    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        //BroadcastRemoteMethod("Interacted", gunRoot, interactionCon);
        UpdateStatus(false);
        currentGunRoot = gunRoot;
        /*rb.velocity = Vector3.zero;
        storedMass = rb.mass;
        rb.mass = 0f;*/
        
        item.transform.position = gunRoot.transform.position;
        item.transform.rotation = gunRoot.transform.rotation;
        item.transform.SetParent(gunRoot.transform);
        currentlyHeld = true;
        interactionCon.holdingItem = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //If this works end up passing interactionCon back the item assigned to this as a gameobject, so when I set up shooting it can call the shoot function on that script instead of looking for child object
        
    }
    public void Drop(GameObject cam, InteractionController interactionCon){
        //BroadcastRemoteMethod("Drop", cam, interactionCon);
        UpdateStatus(true);
        /*rb.mass = storedMass;
        storedMass = 0f;*/
        currentlyHeld = false;
        currentGunRoot = null;
        rb.constraints = RigidbodyConstraints.None;
        interactionCon.holdingItem = false;
        item.transform.SetParent(null);
        var camPOS = cam.transform.position + cam.transform.forward * 0.75f;
        item.transform.position = camPOS;
        item.GetComponent<Rigidbody>().AddForce(CameraSingleton.instance.transform.forward * dropForce, ForceMode.Impulse);
    }
    private void UpdateStatus(bool toggle){
        //BroadcastRemoteMethod("UpdateStatus", toggle);
        objColl.enabled = toggle;
        thisColl.enabled = toggle;
        rb.isKinematic = !toggle;
        rb.useGravity = toggle;
        //popupText.SetActive(toggle);
    }


    private void Update(){
        if(currentlyHeld){
            UpdateStatus(false);
        }
        else{
             item.transform.SetParent(null);
             UpdateStatus(true);
        }

        
    }
}
