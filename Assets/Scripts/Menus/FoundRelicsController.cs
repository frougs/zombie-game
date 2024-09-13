using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoundRelicsController : MonoBehaviour
{
    [SerializeField] GameObject relic;
    [SerializeField] Image itemPreview;
    public RarityManager itemInfo;
    [SerializeField] Button itemButton;
    [Header("Page population stuff")]
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] Image itemImageFull;

    private void Start(){
        itemInfo = relic.GetComponentInChildren<RarityManager>();
    }

    private void OnEnable(){
        UpdateItem();
    }
    public void OpenPage(){
        itemName.text = itemInfo.itemName;
        itemDescription.text = itemInfo.itemDescription;
        itemImageFull.sprite = itemInfo.unlockedIMG;
    }
    public void UpdateItem(){
        if(PlayerPrefs.HasKey(itemInfo.itemName) == false || PlayerPrefs.GetInt(itemInfo.itemName) == 0){
            //Item has not yet been collected, make button non-interactable and display item hidden image
            itemButton.interactable = false;
            itemPreview.sprite = itemInfo.hiddenIMG;
        }
        else{
            //Item has been collected make button interactable and display unobscured item image
            itemButton.interactable = true;
            itemPreview.sprite = itemInfo.unlockedIMG;
        }
    }
    public void FixedUpdate(){
        UpdateItem();
    }

}
