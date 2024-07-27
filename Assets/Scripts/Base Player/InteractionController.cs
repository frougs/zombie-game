using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    public bool holdingItem = false;
    private List<GameObject> overlaps = new List<GameObject>();
    private Alteruna.Avatar _avatar;
    [HideInInspector] public InputAction interact;
    [HideInInspector] public InputAction drop;
    [HideInInspector] public PlayerInput _pInput;
    [SerializeField] public GameObject gunRoot;
    [SerializeField] private float interactRange;
    [SerializeField] private float pickupDelay;
    private UIContainer uiStuff;
    private void Start(){
        _avatar = GetComponent<Alteruna.Avatar>();
        if (!_avatar.IsMe) return;
        _pInput = GetComponent<PlayerInput>();
        interact = _pInput.actions["Interact"];
        drop = _pInput.actions["DropItem"];
    }
    private void Update(){
        if (!_avatar.IsMe) return;
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
        if(interact.triggered){
        }
        //Picks up the item
        if(interact.triggered){
            if(Physics.Raycast(this.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.position, this.GetComponent<ThirdPersonController>().CinemachineCameraTarget.transform.forward, out RaycastHit hitData, interactRange)){
                IInteractable interactable  = hitData.transform.gameObject.GetComponentInChildren<IInteractable>();
                if(interactable != null && holdingItem == false){
                    InteractItem(interactable);
                    StartCoroutine(InteractDelay());
                }
                else if(interactable != null && holdingItem){
                    DropItem();
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
    }
    public void InteractItem(IInteractable interactable){

        interactable.Interacted(gunRoot, this);
    }
    public void DropItem(){
         var weapon = gunRoot.transform.GetChild(0);
            foreach(Transform obj in weapon.transform){
                if(obj.GetComponent<PickupController>()!=null){
                    obj.GetComponent<PickupController>().Drop(this.GetComponent<ThirdPersonController>().CinemachineCameraTarget, this);
                }
            }
    }
    //Stores currently overlapped items
    private void OnTriggerEnter(Collider other){
        if (!_avatar.IsMe) return;
        if(!overlaps.Contains(other.gameObject)){
            overlaps.Add(other.gameObject);
        }
    }
    //Removes overlapped items when out of range
    private void OnTriggerExit(Collider other){
        if (!_avatar.IsMe) return;
        if(overlaps.Contains(other.gameObject)){
            overlaps.Remove(other.gameObject);
        }
    }
    IEnumerator InteractDelay(){
        yield return new WaitForSeconds(pickupDelay);
    }
}
