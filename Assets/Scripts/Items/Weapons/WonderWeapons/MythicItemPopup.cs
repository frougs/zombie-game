using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MythicItemPopup : MonoBehaviour
{
    [SerializeField] float popupDuration;
    public void SendPopup(){
        FindObjectOfType<PopupTextScript>().Message(GetComponent<RarityManager>().itemName, GetComponent<RarityManager>().itemFlavorText, popupDuration, GetComponent<RarityManager>().itemColor);
    }
}
