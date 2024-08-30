using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkBase : MonoBehaviour, IInteractable
{
    [SerializeField] public bool powered;
    //[SerializeField] ThirdPersonController player;
    [SerializeField] Light light;
    [SerializeField] Texture2D perkIcon;
    [SerializeField] public string perkName;
    [SerializeField] public string perkDescription;
    [SerializeField] int upgradeNum = 0;
    [SerializeField] public int price;
    [SerializeField] public GameObject player;
    [SerializeField] public float popupDuration;
    [SerializeField] public bool hasPerk;
    private UIContainer uiStuff;

    private void Update(){
        // if(player == null){
        //     player = FindObjectOfType<ThirdPersonController>();
        // }
        if(powered){
            light.enabled = true;
        }
        else{
            light.enabled = false;
        }
    }
    public void PoweredOn(){
        powered = true;
    }

    public virtual void Interacted(GameObject gunRoot, InteractionController interactionCon){
        player = interactionCon.gameObject;
        var scoreSystem = interactionCon.GetComponent<ScoreSystem>();
        if(scoreSystem.score >= price){
                scoreSystem.SubtractScore(price);
            if(powered && hasPerk == false){
                ActivatePerk();
            }
        }
    }

    public virtual void ActivatePerk(){
        FindObjectOfType<PopupTextScript>().Message(perkName, perkDescription, popupDuration);
        hasPerk = true;
        AddPerkToUI();
        if(upgradeNum == 0){
            //Base perk stuff here
            DefaultPerk();
            if(upgradeNum == 1){
                //Upgrade number 1 stuff
                PerkUpgrade1();
                if(upgradeNum == 2){
                    //Upgrade number 2 stuff
                    PerkUpgrade2();
                    if(upgradeNum == 3){
                        //Upgrade number 3 stuff
                        PerkUpgrade3();
                    }
                }
            }

        }
    }
    public virtual void DefaultPerk(){

    }
    public virtual void PerkUpgrade1(){

    }
    public virtual void PerkUpgrade2(){

    }
    public virtual void PerkUpgrade3(){

    }
    private void Start(){
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
    }
    private void AddPerkToUI(){
        uiStuff.AddPerk(perkIcon);
    }
}