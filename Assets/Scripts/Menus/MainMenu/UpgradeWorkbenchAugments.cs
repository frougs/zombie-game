using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeWorkbenchAugments : MonoBehaviour
{
    [SerializeField] string augmentKey;
    [SerializeField] GameObject[] unlockBlocks;
    [SerializeField] int augmentAmount;
    [SerializeField] TextMeshProUGUI[] amounttexts;
    private int upgradeNum;
    [SerializeField] int maxAugmentAmount;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] int price;
    private void Start(){
        if(PlayerPrefs.HasKey(augmentKey)){
            upgradeNum = PlayerPrefs.GetInt(augmentKey);
            UpdateMenu(upgradeNum);
        }
        else{
            PlayerPrefs.SetInt(augmentKey, 0);
            UpdateMenu(0);
        }
        UpdateAmount();
        UpdatePrice();
    }
    private void UpdateAmount(){
        foreach(TextMeshProUGUI text in amounttexts){
            text.text = "+" +augmentAmount.ToString() + "%";
        }
    }
    private void UpdatePrice(){
        priceText.text = price.ToString();
    }
    public void Unlock(int upgrade){
        if(PlayerPrefs.HasKey("UpgradeTokens")){
            if(PlayerPrefs.GetInt("UpgradeTokens") >= price){
                PlayerPrefs.SetInt("UpgradeTokens", PlayerPrefs.GetInt("UpgradeTokens") - price );
                PlayerPrefs.SetInt(augmentKey, PlayerPrefs.GetInt(augmentKey) + upgrade);
                UpdateMenu(PlayerPrefs.GetInt(augmentKey));
            }
        }
    }
    public void UpdateMenu(int upgradeNum){
        for(int i=0; i < upgradeNum; i++){
            unlockBlocks[i].SetActive(false);
        }
    }
    public void ResetUpgrades(){
        PlayerPrefs.SetInt(augmentKey, 0);
        UpdateMenu(0);
        foreach(GameObject block in unlockBlocks){
            block.SetActive(true);
        }
    }
    private void Update(){
        if(PlayerPrefs.GetInt(augmentKey) > maxAugmentAmount){
            PlayerPrefs.SetInt(augmentKey, maxAugmentAmount);
        }
    }
}
