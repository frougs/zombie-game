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
    [SynchronizableField] public bool currentlyHeld;
    [SynchronizableField] public int currentHolderIndex = 5;
    private GameObject currentGunRoot;
    //private float storedMass;
    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        if(currentlyHeld == false){
            //BroadcastRemoteMethod("Interacted", gunRoot, interactionCon);
            UpdateStatus(false);
            currentGunRoot = gunRoot;
            /*rb.velocity = Vector3.zero;
            storedMass = rb.mass;
            rb.mass = 0f;*/
            //item.transform.position = gunRoot.transform.position;
            //item.transform.rotation = gunRoot.transform.rotation;
            //item.transform.SetParent(gunRoot.transform);
            currentlyHeld = true;
            interactionCon.holdingItem = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            currentHolderIndex = ExtractNum(interactionCon.GetComponent<Alteruna.Avatar>().ToString());
            //BroadcastRemoteMethod(nameof(RemoteUpdateParent), interactionCon.GetComponent<Alteruna.Avatar>().ToString());

        }

        
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
        currentHolderIndex = 5;
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
    private int ExtractNum(string input)
    {
        int number = 0;
        int startIndex = input.IndexOf("Index: ");
        if (startIndex != -1)
        {
            startIndex += "Index: ".Length;
            int endIndex = input.IndexOf(")", startIndex);
            if (endIndex != -1)
            {
                string numberString = input.Substring(startIndex, endIndex - startIndex);
                if (int.TryParse(numberString, out number))
                {
                    return number;
                }
                else
                {
                    Debug.LogError("Failed to parse number.");
                }
            }
            else
            {
                Debug.LogError("Closing parenthesis not found.");
            }
        }
        else
        {
            Debug.LogError("Index not found in input string.");
        }
        return number;
    }

    private void FixedUpdate(){
        if(!currentlyHeld && currentHolderIndex != 5){
            currentHolderIndex = 5;
        }
        if(currentHolderIndex != 5 && item.transform.parent == null){
            var user = Multiplayer.GetUser(currentHolderIndex);
            //Debug.Log("Current holder not 5");
            var userObj = FindObjectsOfType<Alteruna.Avatar>();
            foreach(Alteruna.Avatar players in userObj){
                //Debug.Log("Currently on player: " +players.gameObject.name);
                //Debug.Log(players.ToString() +" : " +user);
                if(players.ToString().Contains(user)){
                    //Debug.Log("User found: " +players.gameObject.name);
                    var holderGunRoot = players.gameObject.GetComponent<InteractionController>().gunRoot;
                    item.transform.position = holderGunRoot.transform.position;
                    item.transform.rotation = holderGunRoot.transform.rotation;
                    item.transform.SetParent(holderGunRoot.transform);
                    //Debug.Log(holderGunRoot.name);
                    
                }
            }
        }
        
        
    }

    /*[SynchronizableMethod]
    public void RemoteUpdateParent(string user){
        Debug.Log(user);
        var remotePlayer = GameObject.Find(user).transform;
        var remoteGunRoot = remotePlayer.transform.Find("gunRoot");
        item.transform.SetParent(remoteGunRoot);
        /*var remoteGunRoot = u.gameObject.transform.Find("gunRoot");
        item.transform.SetParent(remoteGunRoot);
    }   */
}