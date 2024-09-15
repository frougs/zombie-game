using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheForge : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip defaultEngulf;
    [SerializeField] AudioClip mythicEngulf;
    [SerializeField] ParticleSystem fireParticles;
    [SerializeField] int upgradeBonus;
    [SerializeField] float powerupDropChance;
    [SerializeField] float tokenDropChance;
    [SerializeField] GameObject[] powerUps;
    [SerializeField] GameObject powerUpSpawnPoint;
    // [SerializeField] Animator chestAnimator;
    private GameObject spawnedPowerUp;
    private int upgradeAmount;
    private UIContainer uiStuff;
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    [System.Serializable]
    public class RarityPrice
    {
        public Rarity itemRarity;
        public int price;
    }

    public RarityPrice[] rarityPrices;

    public int PriceForRarity(Rarity rarity)
    {
        foreach (var rarityPrice in rarityPrices)
        {
            if (rarityPrice.itemRarity == rarity)
            {
                return rarityPrice.price;
            }
        }
        return PriceForRarity(Rarity.Common);
    }

    private void OnTriggerEnter(Collider obj)
    {
        var rarityManager = obj.GetComponentInChildren<RarityManager>();
        if (rarityManager != null)
        {
            fireParticles.Play();
            if(obj.GetComponent<BaseGun>() != null){
                upgradeAmount = (int)(upgradeBonus * obj.GetComponent<BaseGun>().upgrades.Count);
            }
            var pointsForRarity = upgradeAmount + PriceForRarity(ConvertRarity(rarityManager.itemRarity));
            if(rarityManager.itemRarity.ToString() != "Mythic"){
                soundSource.PlayOneShot(defaultEngulf);
            }
            else{
                soundSource.PlayOneShot(mythicEngulf);
            }
            
            Destroy(obj.transform.parent.gameObject);
            StartCoroutine(PointsDelay(pointsForRarity));
            float poweruprandomValue = UnityEngine.Random.value;
            if(poweruprandomValue <= powerupDropChance){
                SpawnPowerUp();
            }
            float tokenuprandomValue = UnityEngine.Random.value;
            if(tokenuprandomValue <= tokenDropChance){
                AddToken();
            }
            upgradeAmount = 0;
            
        }
    }
    public void Start(){
        powerupDropChance += ((PlayerPrefs.GetInt("ForgePowerups") * 10)  * .01f);
        tokenDropChance += ((PlayerPrefs.GetInt("ForgeTokens") * 7)  * .01f);
    }
    private void SpawnPowerUp(){
        var index = UnityEngine.Random.Range(0, powerUps.Length);
        spawnedPowerUp = Instantiate(powerUps[index], powerUpSpawnPoint.transform.position, Quaternion.identity);
        // chestAnimator.SetBool("open", true);
    }
    private void AddToken(){
        uiStuff.GetComponent<UnlockTokens>().AddUpgradeToken(1);
    }
    private IEnumerator PointsDelay(int pointsForRarity){
        yield return new WaitForSeconds(0.15f);
        if(PlayerPrefs.HasKey("ForgeMoney")){
            if(PlayerPrefs.GetInt("ForgeMoney") != 0){
                int newpointsForRarity = (pointsForRarity + (int)(pointsForRarity * ((PlayerPrefs.GetInt("ForgeMoney") * 20) * .01f)) + 1);
                FindObjectOfType<ScoreSystem>().AddToScore(newpointsForRarity);
            }
            else{
                FindObjectOfType<ScoreSystem>().AddToScore(pointsForRarity);
            }
        }
        else{
            FindObjectOfType<ScoreSystem>().AddToScore(pointsForRarity);
        }
    }

    private TheForge.Rarity ConvertRarity(RarityManager.Rarity rarity)
    {
        switch (rarity)
    {
        case RarityManager.Rarity.Common: return TheForge.Rarity.Common;
        case RarityManager.Rarity.Uncommon: return TheForge.Rarity.Uncommon;
        case RarityManager.Rarity.Rare: return TheForge.Rarity.Rare;
        case RarityManager.Rarity.Epic: return TheForge.Rarity.Epic;
        case RarityManager.Rarity.Legendary: return TheForge.Rarity.Legendary;
        case RarityManager.Rarity.Mythic: return TheForge.Rarity.Mythic;
        default: return TheForge.Rarity.Common;
    }
    }
    private void Update(){
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
    }
}
