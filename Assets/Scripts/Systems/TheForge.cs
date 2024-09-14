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
    private int upgradeAmount;
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
            upgradeAmount = 0;
            
        }
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
}
