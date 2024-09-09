using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerkUpgradeScript : MonoBehaviour
{
    [SerializeField] string perkKey;
    [SerializeField] GameObject[] unlockBlocks;
    //[SerializeField] int unlock1Price;
    [SerializeField] int unlockStepIncrease;
    private int upgradeNum;
    [SerializeField] TextMeshProUGUI[] priceTexts;

    private void Start(){
        if(PlayerPrefs.HasKey(perkKey)){
            upgradeNum = PlayerPrefs.GetInt(perkKey);
            UpdateMenu(upgradeNum);
        }
        else{
            PlayerPrefs.SetInt(perkKey, 0);
            UpdateMenu(0);
        }
        UpdatePrices();
    }

    public void Unlock(int upgrade){
        if(PlayerPrefs.HasKey("UpgradeTokens")){
            if(PlayerPrefs.GetInt("UpgradeTokens") >= upgrade * unlockStepIncrease){
                PlayerPrefs.SetInt("UpgradeTokens", PlayerPrefs.GetInt("UpgradeTokens") - upgrade * unlockStepIncrease);
                PlayerPrefs.SetInt(perkKey, upgrade);
                UpdateMenu(PlayerPrefs.GetInt(perkKey));
            }
        }
    }
    public void UpdateMenu(int upgradeNum){
        for(int i=0; i < upgradeNum; i++){
            unlockBlocks[i].SetActive(false);
        }
    }
    private void UpdatePrices(){
        for(int i=0; i<priceTexts.Length;i++){
            priceTexts[i].text = ((i + 1) * unlockStepIncrease).ToString();
        }
    }
    public void ResetUpgrades(){
        PlayerPrefs.SetInt(perkKey, 0);
        UpdateMenu(0);
        foreach(GameObject block in unlockBlocks){
            block.SetActive(true);
        }
    }
}
