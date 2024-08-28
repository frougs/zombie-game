using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIContainer : MonoBehaviour
{
    public GameObject crosshair;
    public Image progressBar;
    public GameObject ammoObj;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI remainingText;

    public void UpdateAmmo(int currentAmmo, int maxAmmo){
        ammoText.text = "Ammo: " + currentAmmo + " | " + maxAmmo.ToString();
    }
    public void ClearAmmo(){
        ammoText.text = "Ammo: 0 | 0";
    }
    public void UpdateHealth(int currentHealth){
        healthText.text = "Health: " +currentHealth;
    }
    public void UpdateScore(int currentScore){
        //Debug.Log("Updating score");
        scoreText.text = "Score: " +currentScore;
    }
    public void UpdateRound(int round){
        roundText.text = "Round: " +round;
    }
    public void UpdateRemaining(int remaining, int total){
        remainingText.text = "Left: " +remaining.ToString() + "/" +total;
    }
}
