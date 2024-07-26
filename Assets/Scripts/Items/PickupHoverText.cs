using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System;

public class PickupHoverText : MonoBehaviour
{
    /*[SerializeField] private TextMeshPro hoverText;
    private void OnTriggerStay(Collider other){
        if(other.gameObject.GetComponent<ThirdPersonController>() != null){
            Debug.Log("Player entered interaction zone");
        }
    }*/
     [SerializeField] GameObject pickupPopup;
    [SerializeField] TextMeshPro pickupText;
    [SerializeField] Text getText;
    private string displayString;
    [SerializeField] private InputActionReference m_Action;
    private bool active;
    private bool currentlyHeld;
    [SerializeField] float hoverHeight;
    private void OnTriggerEnter(Collider other){
        try{
        var isMe = other.GetComponent<ThirdPersonController>();
        if(isMe != null && !currentlyHeld){
            pickupPopup.SetActive(true);
            var bindingString = displayString;
            pickupText.text = "Pickup: [ " + getText.text +" ]";
            active = true;
        }
        }
        catch(Exception e){
            //Debug.Log("Thing doesnt have the component skullemoji");
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.GetComponent<ThirdPersonController>()){
            pickupPopup.SetActive(false);
            active = false;
        }
    }
    private void FixedUpdate(){
        if(active){
            var parent = GetComponent<Collider>().transform.root;
            pickupText.gameObject.transform.position = new Vector3(parent.position.x, parent.position.y + hoverHeight, parent.position.z);
        }
    }
    public void Held(){
        pickupPopup.SetActive(false);
        active = false;
        currentlyHeld = true;
    }
    public void Dropped(){
        currentlyHeld = false;
    }
    public void GetDisplayString(string bindingText, string uhhh, string whatthesigma){
        displayString = bindingText;
    }

}
