using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [HideInInspector] public bool holdingItem;
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
        if(interact.triggered && holdingItem == false){
            foreach(GameObject go in overlaps){
                IInteractable interactable = go.GetComponent<IInteractable>();
                if(interactable != null){
                    interactable.Interacted(gunRoot, this);
                }
            }
        }
        if(drop.triggered && holdingItem == true){
            var weapon = gunRoot.transform.GetChild(0);
            foreach(Transform obj in weapon.transform){
                if(obj.GetComponent<PickupController>()!=null){
                    obj.GetComponent<PickupController>().Drop(this.GetComponent<ThirdPersonController>().CinemachineCameraTarget, this);
                }
            }
        }
    }
}
