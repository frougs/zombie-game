using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [HideInInspector] public bool holdingItem = false;
    private List<GameObject> overlaps = new List<GameObject>();
    private Alteruna.Avatar _avatar;
    [HideInInspector] public InputAction interact;
    [HideInInspector] public InputAction drop;
    [HideInInspector] public PlayerInput _pInput;
    [SerializeField] GameObject gunRoot;
    private void Start(){
        _avatar = GetComponent<Alteruna.Avatar>();
        if (!_avatar.IsMe) return;
        _pInput = GetComponent<PlayerInput>();
        interact = _pInput.actions["Interact"];
        drop = _pInput.actions["DropItem"];
    }
    private void Update(){
        if (!_avatar.IsMe) return;
        if(interact.triggered){
        }
        //Picks up the item
        if(interact.triggered && holdingItem == false){
            foreach(GameObject go in overlaps){
                IInteractable interactable = go.GetComponent<IInteractable>();
                if(interactable != null){
                    interactable.Interacted(gunRoot, this);
                }
            }
        }
        //Drops the item
        if(drop.triggered && holdingItem == true){
            //Debug.Log("Attemping drop");
            var weapon = gunRoot.transform.GetChild(0);
            foreach(Transform obj in weapon.transform){
                if(obj.GetComponent<PickupController>()!=null){
                    obj.GetComponent<PickupController>().Drop(this.GetComponent<ThirdPersonController>().CinemachineCameraTarget, this);
                }
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
}
