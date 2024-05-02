using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    private List<GameObject> overlaps = new List<GameObject>();
    [HideInInspector] public InputAction interact;
    [HideInInspector] public InputAction drop;
    [HideInInspector] public PlayerInput _pInput;
    private Alteruna.Avatar _avatar;
    private void OnTriggerEnter(Collider other){
        if (!_avatar.IsMe)
            return;
        if(!overlaps.Contains(other.gameObject)){
            overlaps.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other){
        if (!_avatar.IsMe)
            return;
        if(overlaps.Contains(other.gameObject)){
            overlaps.Remove(other.gameObject);
        }
    }
    private void Update(){
        if (!_avatar.IsMe)
            return;
        if(interact.triggered){
            //Debug.Log("Attemping interact");
            foreach(GameObject obj in overlaps){
                IInteractable interactable = obj.GetComponent<IInteractable>();
                if(interactable != null){
                    interactable.Interacted(this.gameObject);
                }
            }
        }
    }
    private void Start(){
        _avatar = GetComponent<Alteruna.Avatar>();
        if (!_avatar.IsMe)
            return;
        _pInput = GetComponent<PlayerInput>();
        interact = _pInput.actions["Interact"];
        drop = _pInput.actions["DropItem"];
    }
}
