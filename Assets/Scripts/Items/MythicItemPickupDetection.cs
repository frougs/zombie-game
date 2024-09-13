using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MythicItemPickupDetection : MonoBehaviour
{
    public void PickedUp(){
        PlayerPrefs.SetInt(this.GetComponent<RarityManager>().itemName, 1);
    }
}
