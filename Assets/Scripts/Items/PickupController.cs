using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public GameObject item;
    public Rigidbody rb;
    public BoxCollider coll;
    public float dropForce;
    public GameObject popupText;

    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        UpdateStatus(false);
        item.transform.position = gunRoot.transform.position;
        item.transform.rotation = gunRoot.transform.rotation;
        item.transform.SetParent(gunRoot.transform);
        interactionCon.holdingItem = true;
        
    }
    public void Drop(GameObject cam, InteractionController interactionCon){
        UpdateStatus(true);
        interactionCon.holdingItem = false;
        item.transform.SetParent(null);
        var camPOS = cam.transform.position + cam.transform.forward * 0.75f;
        item.transform.position = camPOS;
        item.GetComponent<Rigidbody>().AddForce(CameraSingleton.instance.transform.forward * dropForce, ForceMode.Impulse);
    }
    private void UpdateStatus(bool toggle){
        coll.enabled = toggle;
        rb.isKinematic = !toggle;
        popupText.SetActive(toggle);
    }
}
