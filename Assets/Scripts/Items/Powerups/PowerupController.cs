using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerupController : MonoBehaviour
{
    private UIContainer uiStuff;
    [SerializeField] float uiPopupDuration;
    [SerializeField] public float powerupDuration;

    public enum Powerup
    {
        InstaKill,
        Bomb,
        Ammo,
        Hammer,
        MysteryCrate
    }

    [System.Serializable]
    public class PowerupInfo
    {
        public Texture2D powerupIcon;
        public string powerUpName;
        public string powerUpDescription;
        public AudioClip powerUpSound;
        public string powerupScriptClassName;
        public bool isInstant;
    }

    public PowerupInfo[] powerups;

    public void PowerUpCollected(string powerupName)
    {
        foreach (var powerup in powerups)
        {
            if (powerup.powerUpName == powerupName)
            {
                TriggerPowerUp(powerup);
                break;
            }
        }
    }

    public void TriggerPowerUp(PowerupInfo powerup)
    {
        StartCoroutine(PowerUpEffect(powerup.powerupIcon, powerup.powerUpName, powerup.powerUpDescription, powerup, powerup.isInstant));

        if (!string.IsNullOrEmpty(powerup.powerupScriptClassName))
        {
            var scriptType = Type.GetType(powerup.powerupScriptClassName);
            if (scriptType != null && scriptType.IsSubclassOf(typeof(MonoBehaviour)))
            {

                this.gameObject.AddComponent(scriptType);
            }
        }
    }

    public void EndPowerUp(PowerupInfo powerup)
    {
        if (!string.IsNullOrEmpty(powerup.powerupScriptClassName))
        {
            var scriptType = Type.GetType(powerup.powerupScriptClassName);
            if (scriptType != null && scriptType.IsSubclassOf(typeof(MonoBehaviour)))
            {

                var component = this.gameObject.GetComponent(scriptType);
                if (component != null)
                {
                    Destroy(component);
                }
            }
        }
        try{
            this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject.GetComponent<BaseGun>().InstaKillActive = false;
            this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject.GetComponent<BaseGun>().infiniteAmmo = false;
        }
        catch (Exception e){}
        this.GetComponent<WeaponController>().instaKillActive = false;
        var boxes = FindObjectsOfType<MysteryCrate>();
        foreach (var box in boxes){
            box.fireSale = false;
        }
            this.GetComponent<ScoreSystem>().doublePoints = false;
    }

    private IEnumerator PowerUpEffect(Texture2D icon, string name, string description, PowerupInfo powerup, bool isInstant)
    {
        this.GetComponent<AudioSource>().PlayOneShot(powerup.powerUpSound);
        AddPowerUpToUI(icon, name, isInstant);
        TextPopUp(name, description, uiPopupDuration);
        yield return new WaitForSeconds(powerupDuration);
        EndPowerUp(powerup);
        RemovePowerUpFromUI(powerup.powerUpName);
    }

    public void AddPowerUpToUI(Texture2D icon, string name, bool isInstant)
    {
        uiStuff.AddPowerUp(icon, name, isInstant);
    }

    public void RemovePowerUpFromUI(string name)
    {
        uiStuff.RemovePowerUp(name);
    }

    public void TextPopUp(string name, string description, float duration)
    {
        FindObjectOfType<PopupTextScript>().Message(name, description, duration, Color.yellow);
    }
    public void Update(){
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
    }
}
