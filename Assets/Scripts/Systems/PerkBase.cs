using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkBase : Purchasable, IInteractable
{
    [SerializeField] public bool powered;
    //[SerializeField] ThirdPersonController player;
    [SerializeField] Light light;
    [SerializeField] Texture2D perkIcon;
    [SerializeField] public string perkName;
    [SerializeField] public string perkDescription;
    [SerializeField] public int upgradeNum = 0;
    //[SerializeField] public int price;
    [SerializeField] public GameObject player;
    [SerializeField] public float popupDuration;
    [SerializeField] public bool hasPerk;
    public UIContainer uiStuff;
    [SerializeField] public TextMeshPro priceText;
    [SerializeField] public GameObject needPowerObj;
    [SerializeField] public AudioSource soundSource;
    [SerializeField] public AudioClip errorPurchase;
    [SerializeField] public AudioClip purchase;

    public virtual void Update(){
        // if(player == null){
        //     player = FindObjectOfType<ThirdPersonController>();
        // }
        if(powered){
            light.enabled = true;
            needPowerObj.SetActive(false);
        }
        else if(powered == false){
            light.enabled = false;
            needPowerObj.SetActive(true);
        }
        priceText.text = "$" + price.ToString();
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
                soundSource.PlayOneShot(purchase);
            }
        }
        else{
            soundSource.PlayOneShot(errorPurchase);
        }
    }

    public virtual void ActivatePerk(){
        FindObjectOfType<PopupTextScript>().Message(perkName, perkDescription, popupDuration);
        hasPerk = true;
        AddPerkToUI();
        if(upgradeNum >= 0){
            //Base perk stuff here
            DefaultPerk();
            if(upgradeNum >= 1){
                //Upgrade number 1 stuff
                PerkUpgrade1();
                if(upgradeNum >= 2){
                    //Upgrade number 2 stuff
                    PerkUpgrade2();
                    if(upgradeNum >= 3){
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
        soundSource = this.GetComponent<AudioSource>();
    }
    private void AddPerkToUI(){
        uiStuff.AddPerk(perkIcon);
    }
}
