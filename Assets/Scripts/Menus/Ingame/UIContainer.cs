using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIContainer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoText;
    [Header("Health Stuff")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image healthBar;
    [SerializeField] Image delayedHealthBar;
    [SerializeField] float delayedSpeed;
    [SerializeField] Image damagedOLay;
    [Header("Misc")]
    public GameObject crosshair;
    public Image progressBar;
    public GameObject ammoObj;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI remainingText;
    [Header("Score Stuff")]
    [SerializeField] public GameObject scoreParticle;
    [SerializeField] public GameObject spawnScoreParticle;
    [Header("Perk Stuff")]
    [SerializeField] GameObject perkParent;
    [SerializeField] GameObject rageBar;
    [SerializeField] Image rageBarFill;
    [SerializeField] GameObject doubleDamageObj;
    [Header("Upgrade Token Stuff")]
    [SerializeField] TextMeshProUGUI upgradeTokenCount;
    [SerializeField] GameObject spawnTokenPOS;
    [SerializeField] GameObject addedTokenPrefab;

    private AudioSource uiAudio;
    [SerializeField] AudioClip purchase;
    private int lastHealth;
    private int lastMaxHealth;
    private float lastHealthFillAmount;
    [SerializeField] GameObject deadUI;
    [SerializeField] TextMeshProUGUI deadRoundCount;
    private int deathRounds;
    private ThirdPersonController player;
    [SerializeField] public AudioSource menuMusic;
    [SerializeField] AudioSource gameplayMusic;
    [SerializeField] AudioClip deathMusic;
    [SerializeField] AudioClip deathLoop;
    private bool trackAudioComplete;
    [SerializeField] AudioClip[] gameplayMusicClips;
    private bool triggerDeathMusicOnce = true;
    

    public void UpdateMenuMusic(AudioClip music, bool looping){
        menuMusic.clip = music;
        menuMusic.loop = looping;
        menuMusic.Play();
    }
    public void PauseAllSounds(){
        AudioSource[] audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach (AudioSource a in audioSources){
            if(a.tag == "Music" || a.tag == "SFX"){
                if(a.gameObject.layer != LayerMask.NameToLayer("MenuMusic")){
                a.Pause();
                }
                
                
            }
        }
    }
    public void UnpauseAllSounds(){
        AudioSource[] audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach (AudioSource a in audioSources){
                    if(a.tag == "Music" || a.tag == "SFX"){
                        if(a.gameObject.layer != LayerMask.NameToLayer("MenuMusic")){
                        a.UnPause();
                        }
                        
                        
                    }
                }
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo){
        ammoText.text = "Ammo: " + currentAmmo + " | " + maxAmmo.ToString();
    }

    public void ClearAmmo(){
        ammoText.text = "Ammo: 0 | 0";
    }

    public void UpdateHealth(int currentHealth, int maxHealth){
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        float newHealthFillAmount = (float)currentHealth / maxHealth;

        if (currentHealth != lastHealth || maxHealth != lastMaxHealth || Mathf.Abs(delayedHealthBar.fillAmount - newHealthFillAmount) > Mathf.Epsilon)
        {
            healthBar.fillAmount = newHealthFillAmount;

            // if (Mathf.Abs(delayedHealthBar.fillAmount - newHealthFillAmount) > Mathf.Epsilon)
            // {
            //     StartCoroutine(DelayedHealthBarUpdate(currentHealth, maxHealth));
            // }

            lastHealth = currentHealth;
            lastMaxHealth = maxHealth;
            lastHealthFillAmount = newHealthFillAmount;
        }
    }
    private void FixedUpdate(){
        if(delayedHealthBar.fillAmount != healthBar.fillAmount){
            delayedHealthBar.fillAmount = Mathf.Lerp(delayedHealthBar.fillAmount, healthBar.fillAmount, Time.deltaTime * delayedSpeed);
        }
    }

    /*private IEnumerator DelayedHealthBarUpdate(int currentHealth, int maxHealth){
        float elapsedTime = 0f;
        float startFillAmount = delayedHealthBar.fillAmount;
        float targetFillAmount = (float)currentHealth / maxHealth;

        while (elapsedTime < delayedSpeed){
            delayedHealthBar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / delayedSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        delayedHealthBar.fillAmount = targetFillAmount;
    }*/

    public void UpdateScore(int currentScore){
        scoreText.text = "$" + currentScore;
    }
    public void CreateScoreParticle(string prefix, int amount){
        var scorePart = Instantiate(scoreParticle, spawnScoreParticle.transform);
        scorePart.GetComponent<TextMeshProUGUI>().text = prefix + amount.ToString();
        if(prefix == "-"){
            uiAudio.PlayOneShot(purchase);
        }
    }

    public void UpdateRound(int round){
        roundText.text = round.ToString();
        deathRounds = round;
    }

    public void UpdateRemaining(int remaining, int total){
        remainingText.text = "x" + remaining.ToString();
    }

    public void AddPerk(Texture2D icon){
        GameObject perkObject = new GameObject("Perk_UI");
        perkObject.transform.SetParent(perkParent.transform, false);
        Image image = perkObject.AddComponent<Image>();
        if (icon != null){
            image.sprite = TextureToSprite(icon);
        }
    }

    Sprite TextureToSprite(Texture2D texture){
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void UpdateRageBar(bool enable, float rageAmnt, bool doubledamage){
        if (enable){
            rageBar.SetActive(true);
            rageBarFill.fillAmount = Mathf.Lerp(rageBarFill.fillAmount, rageAmnt, Time.deltaTime);
            doubleDamageObj.SetActive(doubledamage);
        }
        else{
            rageBar.SetActive(false);
            doubleDamageObj.SetActive(false);
        }
    }

    public void UpdateUpgradeTokens(){
        upgradeTokenCount.text = PlayerPrefs.GetInt("UpgradeTokens").ToString();
    }

    public void AddedTokenAnimation(){
        // Also add a sound here LOL!
        Instantiate(addedTokenPrefab, spawnTokenPOS.transform);
    }

    public void LateUpdate(){
        UpdateUpgradeTokens();
        if(player == null){
            player = FindObjectOfType<ThirdPersonController>();
        }
    }

    public void UpdateDamagedOverlay(bool enabled, float percent){
        if (enabled){
            damagedOLay.gameObject.SetActive(true);
            float alpha = Mathf.Clamp(percent, 0f, 1f);
            Color color = damagedOLay.color;
            color.a = alpha;
            damagedOLay.color = color; 
        }
        else{
            damagedOLay.gameObject.SetActive(false);
        }
    }
    private void Start(){
        uiAudio = GetComponent<AudioSource>();
    }
    public void DeathUI(){
        Time.timeScale = 0f;
        deadUI.SetActive(true);
        deadRoundCount.text = "HAHAHA U ONLY MADE IT TO ROUND " +deathRounds.ToString();
        FindObjectOfType<Pause>().inMenu = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<ThirdPersonController>().enabled = false;
        player.GetComponent<InteractionController>().enabled = false;
        player.GetComponent<WeaponController>().enabled = false;
        if(triggerDeathMusicOnce == true){
            UpdateMenuMusic(deathMusic, false);
            triggerDeathMusicOnce = false;
            trackAudioComplete = true;
        }
        PauseAllSounds();
        var currentScene = SceneManager.GetActiveScene();
        if(PlayerPrefs.HasKey(currentScene.name)){
            if(PlayerPrefs.GetInt(currentScene.name) < deathRounds){
                PlayerPrefs.SetInt(currentScene.name, deathRounds);
            }
        }
        else{
            PlayerPrefs.SetInt(currentScene.name, deathRounds);
        }
    }
    private void Update(){
        if(trackAudioComplete){
            if(!menuMusic.isPlaying){
                Debug.Log("Death music done playing, starting loop...");
                UpdateMenuMusic(deathLoop, true);
            }
        }
    }
}
