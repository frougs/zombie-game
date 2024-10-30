using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject destination;
    public bool canTeleport;
    private bool teleporting;
    private GameObject player;

    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        Debug.Log("interacted with");
        teleporting = true;
        player = interactionCon.gameObject;
    }
    public void Update(){
        if(teleporting){
            player.transform.position = destination.transform.position;
        }
    }
    IEnumerator WaitBeforeEnd(){
        yield return new WaitForSeconds(5);
        teleporting = false;
    }

}
