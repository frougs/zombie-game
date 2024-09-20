using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RarityManager : MonoBehaviour
{
    // Define the enum for different rarity levels
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    // The currently selected rarity for this object
    public Rarity itemRarity;
    public Color itemColor;
    public string itemName;
    [SerializeField] TextMeshPro itemPickupName;
    [SerializeField] public string itemFlavorText;
    [HideInInspector] public Color commonColor;
    public Sprite hiddenIMG;
    public Sprite unlockedIMG;
    public string itemDescription;

    // Serializable class to pair rarity with color
    [System.Serializable]
    public class RarityColor
    {
        public Rarity rarity;
        public Color color;
    }

    // Array to configure colors for each rarity
    public RarityColor[] rarityColors;

    // Method to get the color associated with a given rarity
    public Color GetColorForRarity(Rarity rarity)
    {
        foreach (var rarityColor in rarityColors)
        {
            if (rarityColor.rarity == rarity)
            {
                return rarityColor.color;
            }
        }
        return Color.white; // Default color if rarity is not found
    }

    // Example usage: Set the object's color based on its rarity
    private void Start(){
        itemColor = GetColorForRarity(itemRarity);
        commonColor = GetColorForRarity(Rarity.Common);
        if(itemPickupName != null){
            itemPickupName.text = itemName;
            itemPickupName.color = itemColor;
        }
    }
    public Color GetItemColor(){
        return GetColorForRarity(itemRarity);
    }
}
