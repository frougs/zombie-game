using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTokens : MonoBehaviour
{
    [SerializeField] AudioClip tokenEarnedSound;
    public void AddUpgradeToken(int amount){
        PlayerPrefs.SetInt("UpgradeTokens", PlayerPrefs.GetInt("UpgradeTokens") + amount);
        GetComponent<UIContainer>().AddedTokenAnimation();
        this.GetComponent<AudioSource>().PlayOneShot(tokenEarnedSound);

    }
    public void DeleteTokens(){
        PlayerPrefs.DeleteKey("UpgradeTokens");
    }
}
