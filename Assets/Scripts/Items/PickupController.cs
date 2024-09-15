using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Alteruna;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// NOTE FOR FUTURE, MAYBE HAVE RIGID BODY AND TRANSFORM SYNCHRONIZATION BE ENABLED/DISABLED WHEN PICKED UP OR DROPPED TO PREVENT DESYNC ISSUES WHEN WEAPONS ARE DROPPED
/// </summary>
[RequireComponent(typeof(RarityManager))]
public class PickupController : MonoBehaviour, IInteractable
{
    public bool startHeld;
    public GameObject item;
    public Rigidbody rb;
    public BoxCollider thisColl;
    public BoxCollider objColl;
    public float dropForce;
    //public GameObject popupText;
    public bool currentlyHeld;
    public int currentHolderIndex = 5;
    private GameObject currentGunRoot;
    public UnityEvent triggered;
    public UnityEvent dropped;
    public int itemID;
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip pickupClip;
    [SerializeField] AudioClip dropClip;
    [HideInInspector] public UIContainer uiStuff;

    
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
            soundSource.PlayOneShot(pickupClip);
            triggered?.Invoke();
            interactionCon.holdingItem = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            //currentHolderIndex = ExtractNum(interactionCon.GetComponent<Alteruna.Avatar>().ToString());
            currentHolderIndex = 1;
            //interactionCon.currentItemID = itemID;
            interactionCon.currentHeldID = itemID;
            
            //BroadcastRemoteMethod(nameof(RemoteUpdateParent), interactionCon.GetComponent<Alteruna.Avatar>().ToString());

        }
        if(startHeld){
            startHeld = false;
        }
        UpdateItemUIOnPickup();
    }
    public void Drop(GameObject cam, InteractionController interactionCon){
        //Debug.Log("Dropping: " +this.gameObject.name);
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
        //interactionCon.currentItemID = 0;
        interactionCon.currentHeldID = 0;
        currentHolderIndex = 5;
        soundSource.PlayOneShot(dropClip);
        dropped?.Invoke();
        if(item.GetComponent<BaseGun>() != null){
            item.GetComponent<BaseGun>().StopReload();
        }
        UpdateItemUIOnDrop();
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
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
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
            //var user = Multiplayer.GetUser(currentHolderIndex);
            //Debug.Log("Current holder not 5");
            var userObj = FindObjectsOfType<ThirdPersonController>();
            foreach(ThirdPersonController players in userObj){
                //Debug.Log("Currently on player: " +players.gameObject.name);
                //Debug.Log(players.ToString() +" : " +user);
                // if(players.ToString().Contains(user)){
                    //Debug.Log("User found: " +players.gameObject.name);
                    //Debug.Log(players.gameObject.name);
                    var holderGunRoot = players.gameObject.GetComponent<InteractionController>().gunRoot;
                    item.transform.position = holderGunRoot.transform.position;
                    item.transform.rotation = holderGunRoot.transform.rotation;
                    item.transform.SetParent(holderGunRoot.transform);
                    //Debug.Log(holderGunRoot.name);
                    
                //}
            
            }
        }
        if(startHeld){
            uiStuff.UpdateItemText(GetComponent<RarityManager>().itemName, GetComponent<RarityManager>().itemColor);
        }
    }
    private void Start(){
        if(startHeld){
            StartGameHeld();
        }
        if(soundSource == null){
            soundSource = GetComponentInChildren<AudioSource>();
        }
    }
    private void StartGameHeld(){
        var player = FindObjectOfType<InteractionController>();
        Interacted(player.gunRoot, player);
    }
    public void UpdateItemUIOnPickup(){
        //uiStuff.UpdateItemText(itemName, itemColor);
        if(GetComponent<RarityManager>() != null){
            uiStuff.UpdateItemText(GetComponent<RarityManager>().itemName, GetComponent<RarityManager>().itemColor);
        }
    }
    public void UpdateItemUIOnDrop(){
        uiStuff.UpdateItemText("Rock", GetComponent<RarityManager>().commonColor);
    }
    


}
