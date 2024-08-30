using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorSystem : Purchasable, IInteractable
{
    //[SerializeField] int doorPrice;
    [SerializeField] GameObject[] connectedDoors;
    [SerializeField] TextMeshPro[] costText;
    [SerializeField] SpawnerScript[] spawners;

    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        var scoreSystem = interactionCon.GetComponent<ScoreSystem>();
        if(scoreSystem.score >= price){
            scoreSystem.SubtractScore(price);
            RemoveDoors();
        }
        else{
            //Once I add a notification system, display an error and play an error sound idk lol!! xD
            Debug.LogWarning("Too Broke to Buy Door!!! HAHAHA!!");
        }
    }
    private void RemoveDoors(){
        //Maybe change to a different system later? lol
        Destroy(this.gameObject);
        foreach (GameObject door in connectedDoors){
            Destroy(door);
        }
        if(spawners != null){
            foreach(SpawnerScript spawner in spawners){
                spawner.roomActive = true;
            }
        }
    }
    public void UpdateCostText(){
        foreach(TextMeshPro text in costText){
            text.text = "$"+price;
        }
    }
    private void Start(){
        UpdateCostText();
    }
    private void Update(){
        UpdateCostText();
    }

}
