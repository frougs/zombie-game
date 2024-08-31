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
    [SerializeField] GameObject perkParent;
    [SerializeField] GameObject rageBar;
    [SerializeField] Image rageBarFill;
    [SerializeField] GameObject doubleDamageObj;
    [SerializeField] TextMeshProUGUI upgradeTokenCount;



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
    public void AddPerk(Texture2D icon){
        GameObject perkObject = new GameObject("Perk_UI");
        perkObject.transform.SetParent(perkParent.transform, false);
        Image image = perkObject.AddComponent<Image>();
        if(icon != null){
            image.sprite = TextureToSprite(icon);
        }
    }
    Sprite TextureToSprite(Texture2D texture){
         return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    public void UpdateRageBar(bool enable, float rageAmnt, bool doubledamage){
        if(enable){
            rageBar.SetActive(true);
            rageBarFill.fillAmount = Mathf.Lerp(rageBarFill.fillAmount, rageAmnt, Time.deltaTime);
            if(doubledamage){
                doubleDamageObj.SetActive(true);
            }
            else{
                doubleDamageObj.SetActive(false);
            }
        }
        else{
            rageBar.SetActive(false);
            doubleDamageObj.SetActive(false);
        }
    }
    public void UpdateUpgradeTokens(){
        upgradeTokenCount.text = PlayerPrefs.GetInt("UpgradeTokens").ToString();
    }
    public void LateUpdate(){
        UpdateUpgradeTokens();
    }
}
