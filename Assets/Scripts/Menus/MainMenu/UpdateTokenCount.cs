using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateTokenCount : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tokenText;
    void Update()
    {
        tokenText.text = PlayerPrefs.GetInt("UpgradeTokens").ToString();
    }
    public void AddToken(){
        PlayerPrefs.SetInt("UpgradeTokens", PlayerPrefs.GetInt("UpgradeTokens") + 1);
    }
    public void ResetTokens(){
        PlayerPrefs.SetInt("UpgradeTokens", 0);
    }
}
