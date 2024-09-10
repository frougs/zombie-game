using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InteractionController : MonoBehaviour
{
    public bool holdingItem = false;
    private List<GameObject> overlaps = new List<GameObject>();
    //private Alteruna.Avatar _avatar;
    [HideInInspector] public InputAction interact;
    [HideInInspector] public InputAction drop;
    [HideInInspector] public PlayerInput _pInput;
    [SerializeField] public GameObject gunRoot;
    [SerializeField] private float interactRange;
    [SerializeField] private float pickupDelay;
    private UIContainer uiStuff;
    //public int currentItemID = 0;
    [SerializeField] public GameObject currentlyHeld;
    [SerializeField] public int currentHeldID;
    public bool canInteract = true;
    private void Start(){
        // _avatar = GetComponent<Alteruna.Avatar>();
        // if (!_avatar.IsMe) return;
        _pInput = GetComponent<PlayerInput>();
        interact = _pInput.actions["Interact"];
        drop = _pInput.actions["DropItem"];
    }
    private void Update(){
        // if (!_avatar.IsMe) return;
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
        if(interact.triggered){
        }
        //Picks up the item
        if(interact.triggered && canInteract){
            if(Physics.Raycast(this.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, this.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward, out RaycastHit hitData, interactRange)){
                IInteractable interactable  = hitData.transform.gameObject.GetComponentInChildren<IInteractable>();
                if(interactable != null && holdingItem == false){
                    InteractItem(interactable);
                    StartCoroutine(InteractDelay());
                }
                else if(interactable != null && holdingItem){
                    //Debug.Log("Already holding something, trying to drop it");
                    if(hitData.transform.gameObject.GetComponent<BaseGun>() != null || hitData.transform.gameObject.GetComponentInChildren<PickupController>() != null){
                        DropItem();
                    }
                    StartCoroutine(InteractDelay());
                    InteractItem(interactable);
                }
                else{
                    //Put something here for error interact sound
                    Debug.Log("Not Interactable Cuh");
                }
            }
        }
        //Drops the item
        if(drop.triggered && holdingItem == true){
            DropItem();
        }
        if(!holdingItem){
            uiStuff.ClearAmmo();
        }
        if(gunRoot != null){
            try{
                if(gunRoot.transform.GetChild(0).gameObject != null){
                    currentlyHeld = gunRoot.transform.GetChild(0).gameObject;
                }
            }
            catch(Exception e){

            }
        }
        
    }
    public void InteractItem(IInteractable interactable){

        interactable.Interacted(gunRoot, this);
    }
    public void DropItem(){
        //var weapon = gunRoot.transform.GetChild(0);
        var weapon = currentlyHeld;
            foreach(Transform obj in weapon.transform){
                if(obj.GetComponent<PickupController>()!=null){
                    obj.GetComponent<PickupController>().Drop(this.GetComponent<ThirdPersonController>().CinemachineCameraTarget, this);
                }
            }
        currentlyHeld = null;
    }
    //Stores currently overlapped items
    private void OnTriggerEnter(Collider other){
        // if (!_avatar.IsMe) return;
        if(!overlaps.Contains(other.gameObject)){
            overlaps.Add(other.gameObject);
        }
    }
    //Removes overlapped items when out of range
    private void OnTriggerExit(Collider other){
        // if (!_avatar.IsMe) return;
        if(overlaps.Contains(other.gameObject)){
            overlaps.Remove(other.gameObject);
        }
    }
    IEnumerator InteractDelay(){
        canInteract = false;
        yield return new WaitForSeconds(pickupDelay);
        canInteract = true;
    }
}
