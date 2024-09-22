using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerupController : MonoBehaviour
{
    private UIContainer uiStuff;
    [SerializeField] float uiPopupDuration;
    [SerializeField] public float powerupDuration;
    [SerializeField] AudioSource soundSource;
    private Coroutine powerupDurationCoroutine;

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
    private Dictionary<string, Coroutine> activePowerups = new Dictionary<string, Coroutine>();

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
        if(powerup.isInstant == false){
            if (activePowerups.TryGetValue(powerup.powerUpName, out Coroutine existingCoroutine))
            {
                // Restart the existing coroutine
                StopCoroutine(existingCoroutine);
            }
            else{
                AddPowerUpToUI(powerup.powerupIcon, powerup.powerUpName, powerup.isInstant);
            }

            // Start a new coroutine for the powerup
            Coroutine newCoroutine = StartCoroutine(PowerUpEffect(powerup.powerupIcon, powerup.powerUpName, powerup.powerUpDescription, powerup, powerup.isInstant));
            activePowerups[powerup.powerUpName] = newCoroutine;
        }
        else{
            StartCoroutine(PowerUpEffect(powerup.powerupIcon, powerup.powerUpName, powerup.powerUpDescription, powerup, powerup.isInstant));
        }


        //powerupDurationCoroutine = StartCoroutine(PowerUpEffect(powerup.powerupIcon, powerup.powerUpName, powerup.powerUpDescription, powerup, powerup.isInstant));
        soundSource.PlayOneShot(powerup.powerUpSound);
        TextPopUp(powerup.powerUpName, powerup.powerUpDescription, uiPopupDuration);

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
