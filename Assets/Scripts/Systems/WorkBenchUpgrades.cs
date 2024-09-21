using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class WorkBenchUpgrades : Purchasable, IInteractable
{
    [SerializeField] GameObject upgradeUI;
    private PlayerInput _pInput;
    [HideInInspector] public InputAction pause;
    private GameObject player;
    private bool menuOpen;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI critText;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI firerateText;
    private float damageAugmentAmnt;
    private float critAugmentAmnt;
    private float ammoAugmentAmnt;
    private float firerateAugmentAmnt;
    private GameObject currentGun;
    private ScoreSystem scoreSystem;
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip purchase;
    [SerializeField] AudioClip errorPurchase;
    private UIContainer uiStuff;
    private bool powered = false;
    [SerializeField] Button damageButton;
    [SerializeField] Button critButton;
    [SerializeField] Button ammoButton;
    [SerializeField] Button firerateButton;
    [SerializeField] GameObject poweronObj;
    [SerializeField] Material upgradeMaterial;
    [SerializeField] AudioClip upgradeSound;
    [SerializeField] ParticleSystem upgradeParticles;

    public void PoweredOn(){
        powered = true;
        poweronObj.SetActive(false);
    }
    public virtual void Interacted(GameObject gunRoot, InteractionController interactionCon){
        if(powered){
            player = interactionCon.gameObject;
            currentGun = gunRoot.transform.GetChild(0).gameObject;
            scoreSystem = interactionCon.GetComponent<ScoreSystem>();
            OpenMenu();
        }
    }
    private void Update(){
        if(player != null){
        _pInput = player.GetComponent<PlayerInput>();
        pause = _pInput.actions["Pause"];
        }
        if(pause.triggered && menuOpen){
            CloseMenu();
        }

    }
    private void OpenMenu(){
        CheckAugments();
        UpdateMenu();
        CheckUpgrades();
        if(currentGun.GetComponent<BaseGun>() != false){
            currentGun.GetComponent<BaseGun>().canShoot = false;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<ThirdPersonController>().enabled = false;
        FindObjectOfType<Pause>().inMenu = true;
        upgradeUI.SetActive(true);
        menuOpen = true;
    }
    public void CloseMenu(){
        StartCoroutine(GunDelay());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.GetComponent<ThirdPersonController>().enabled = true;
        upgradeUI.SetActive(false);
        FindObjectOfType<Pause>().inMenu = false;
        menuOpen = false;
    }
    private IEnumerator GunDelay(){
        yield return new WaitForSeconds(0.15f);
        if(currentGun.GetComponent<BaseGun>() != false){
            currentGun.GetComponent<BaseGun>().canShoot = true;
        }
    }
    private void UpdateMenu(){
        damageText.text = "+" + damageAugmentAmnt + "%";
        critText.text = "+" + critAugmentAmnt + "%";
        ammoText.text = "+" + ammoAugmentAmnt + "%";
        firerateText.text = "+" + firerateAugmentAmnt + "%";
    }
    private void CheckAugments(){
        if(PlayerPrefs.HasKey("DamageAugment") && PlayerPrefs.GetInt("DamageAugment") != 0){
            damageAugmentAmnt = 20 + PlayerPrefs.GetInt("DamageAugment") * 20;
        }
        else{
            damageAugmentAmnt = 20;
        }
        if(PlayerPrefs.HasKey("CritAugment") && PlayerPrefs.GetInt("CritAugment") != 0){
            critAugmentAmnt = 10 + PlayerPrefs.GetInt("CritAugment") * 10;
        }
        else{
            critAugmentAmnt = 10;
        }
        if(PlayerPrefs.HasKey("AmmoAugment") && PlayerPrefs.GetInt("AmmoAugment") != 0){
            ammoAugmentAmnt = 15 + PlayerPrefs.GetInt("AmmoAugment") * 15;
        }
        else{
            ammoAugmentAmnt = 15;
        }
        if(PlayerPrefs.HasKey("FirerateAugment") && PlayerPrefs.GetInt("FirerateAugment") != 0){
            firerateAugmentAmnt = 10 + PlayerPrefs.GetInt("FirerateAugment") * 10;
        }
        else{
            firerateAugmentAmnt = 10;
        }
    }
    private void UpgradeDamage(){
        currentGun.GetComponent<BaseGun>().RefillAmmo();
        currentGun.GetComponent<BaseGun>().damage = currentGun.GetComponent<BaseGun>().damage + (currentGun.GetComponent<BaseGun>().damage * (damageAugmentAmnt / 100f));
        CloseMenu();
    }
    private void UpgradeCrit(){
        currentGun.GetComponent<BaseGun>().RefillAmmo();
        currentGun.GetComponent<BaseGun>().critMultiplier = currentGun.GetComponent<BaseGun>().critMultiplier + (currentGun.GetComponent<BaseGun>().critMultiplier * (critAugmentAmnt / 100f));
        CloseMenu();
    }
    private void UpgradeAmmo(){
        currentGun.GetComponent<BaseGun>().maxAmmo = currentGun.GetComponent<BaseGun>().maxAmmo + ((int)(currentGun.GetComponent<BaseGun>().maxAmmo * (ammoAugmentAmnt / 100)));
        currentGun.GetComponent<BaseGun>().maxReserveAmmo = currentGun.GetComponent<BaseGun>().maxReserveAmmo + ((int)(currentGun.GetComponent<BaseGun>().maxReserveAmmo * (ammoAugmentAmnt / 100)));
        currentGun.GetComponent<BaseGun>().RefillAmmo();
        CloseMenu();
    }
    private void UpgradeFirerate(){
        currentGun.GetComponent<BaseGun>().RefillAmmo();
        currentGun.GetComponent<BaseGun>().fireRate = currentGun.GetComponent<BaseGun>().fireRate - (currentGun.GetComponent<BaseGun>().fireRate * (firerateAugmentAmnt / 100f));
        CloseMenu();
    }
    public void PurchaseUpgrade(string upgradeName){
        if(scoreSystem.score >= price){

            scoreSystem.SubtractScore(price);
            soundSource.PlayOneShot(purchase);
            currentGun.GetComponent<BaseGun>().soundSource.pitch = 2f;
            UpdateMaterial();
            soundSource.PlayOneShot(upgradeSound);
            upgradeParticles.Play();
            if(upgradeName == "damage"){
                UpgradeDamage();
                //damageButton.interactable = false;
                currentGun.GetComponent<BaseGun>().upgrades.Add(upgradeName);
            }
            if(upgradeName == "crit"){
                UpgradeCrit();
                //critButton.interactable = false;
                currentGun.GetComponent<BaseGun>().upgrades.Add(upgradeName);
            }
            if(upgradeName == "ammo"){
                UpgradeAmmo();
                //ammoButton.interactable = false;
                currentGun.GetComponent<BaseGun>().upgrades.Add(upgradeName);
            }
            if(upgradeName == "firerate"){
                UpgradeFirerate();
                //firerateButton.interactable = false;
                currentGun.GetComponent<BaseGun>().upgrades.Add(upgradeName);
            }
        }
        else{
            soundSource.PlayOneShot(errorPurchase);
        }
    }
    private void UpdateMaterial(){
        MeshRenderer[] meshRenderers = currentGun.GetComponentsInChildren<MeshRenderer>();
        if(meshRenderers != null){
            foreach(MeshRenderer meshRenderer in meshRenderers){
                meshRenderer.material = upgradeMaterial;
                //Debug.Log(meshRenderer.gameObject.name);
            }   
        }
    }
    public void CheckUpgrades(){
        damageButton.interactable = !currentGun.GetComponent<BaseGun>().upgrades.Contains("damage");
        critButton.interactable = !currentGun.GetComponent<BaseGun>().upgrades.Contains("crit");
        ammoButton.interactable = !currentGun.GetComponent<BaseGun>().upgrades.Contains("ammo");
        firerateButton.interactable = !currentGun.GetComponent<BaseGun>().upgrades.Contains("firerate");

    }
    
}
