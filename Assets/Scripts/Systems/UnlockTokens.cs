using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTokens : MonoBehaviour
{

    public void AddUpgradeToken(int amount){
        PlayerPrefs.SetInt("UpgradeTokens", PlayerPrefs.GetInt("UpgradeTokens") + amount);

    }
    public void DeleteTokens(){
        PlayerPrefs.DeleteKey("UpgradeTokens");
    }
}
