using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System;


public class ItemInteractionPopup : MonoBehaviour
{
    [SerializeField] GameObject pickupPopup;
    [SerializeField] TextMeshPro pickupText;
    [SerializeField] Text getText;
    private string displayString;
    [SerializeField] private InputActionReference m_Action;
    [HideInInspector] public bool held;
    private void OnTriggerEnter(Collider other){
        try{
        var isMe = other.GetComponent<ThirdPersonController>()._avatar;
        if(isMe != null){
            if(isMe.IsMe){
                pickupPopup.SetActive(true);
                var bindingString = displayString;
                pickupText.text = "Pickup: [ " + getText.text +" ]";
            }
        }
        }
        catch(Exception e){
            //Debug.Log("Thing doesnt have the component skullemoji");
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.GetComponent<Alteruna.Avatar>().IsMe){
            pickupPopup.SetActive(false);
        }
    }
    public void GetDisplayString(string bindingText, string uhhh, string whatthesigma){
        displayString = bindingText;
    }
    public void Held(){
        pickupPopup.SetActive(false);

    }
}

