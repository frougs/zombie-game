using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundsScript : MonoBehaviour
{
    //Most of these are temp Serialized for testing lol
    [SerializeField] public int roundNumber;
    [SerializeField] public int remainingSpawnCount;
    [SerializeField] int totalSpawnCount;
    [SerializeField] int zombieBaseHP;
    [SerializeField] int zombieHpAdditive;
    [SerializeField] int r1SpawnCount;
    [SerializeField] float roundDelay;
    [SerializeField] int maxZombiesAtOnce;
    [SerializeField] public int currentAlive;
    //[SerializeField] GameObject zombiePrefab;
    public List<GameObject> activeSpawners = new List<GameObject>();
    //public int aliveZombies;
    public int roundSpawned;
    private bool canSpawn;
    private UIContainer uiStuff;
    private int triggeredForThisRound = 0;
    [Header("Round color change stuff")]
    public float colorTransitionTime;
    public Color baseColor;
    public Color midColor;
    private TextMeshProUGUI roundText;
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip roundStartClip;
    private bool maxZombiesReached;

    //Testing
    public bool spawningConditionsReached;
    private void Start(){
        if(roundNumber == 0){
            roundNumber = 1;
            totalSpawnCount = r1SpawnCount;
            remainingSpawnCount = totalSpawnCount;
            StartCoroutine(RoundStartDelay());
            StartSpawning();
        }
        if(soundSource == null){
            soundSource = this.GetComponentInChildren<AudioSource>();
        }
    }
private void Update(){
    // Check if round is over
    if (remainingSpawnCount <= 0 && currentAlive <= 0) {
        CalculateNextRound();
    }

    // Ensure remainingSpawnCount does not go below 0
    remainingSpawnCount = Mathf.Max(remainingSpawnCount, 0);

    // Start spawning if conditions are met
    if (canSpawn && roundSpawned < totalSpawnCount && !maxZombiesReached) {
        StartSpawning();
    }

    // Update maxZombiesReached based on currentAlive
    maxZombiesReached = currentAlive >= maxZombiesAtOnce;

    // Update UI and spawning conditions
    if (uiStuff == null) {
        uiStuff = FindObjectOfType<UIContainer>();
    }
    roundText = uiStuff.roundText;
    uiStuff.UpdateRemaining(remainingSpawnCount, totalSpawnCount);

    // Handle upgrade tokens every 10 rounds
    if (roundNumber % 10 == 0 && roundNumber != triggeredForThisRound) {
        uiStuff.GetComponent<UnlockTokens>().AddUpgradeToken(5);
        triggeredForThisRound = roundNumber;
    }

    // Update spawning conditions for UI
    spawningConditionsReached = (roundSpawned < totalSpawnCount) && canSpawn && !maxZombiesReached;
    uiStuff.UpdateTestingUI(currentAlive, canSpawn, spawningConditionsReached, maxZombiesReached, roundSpawned);
}

private void CalculateNextRound(){
    //float nextSpawnAmount = Mathf.Ceil((totalSpawnCount * 2.5f) / 2f);
    float nextSpawnAmount = r1SpawnCount + Mathf.Ceil(0.08f * Mathf.Pow(roundNumber, 2) + 2 * roundNumber);
    roundSpawned = 0;
    roundNumber++;
    uiStuff.UpdateRound(roundNumber);
    zombieBaseHP += zombieHpAdditive;
    totalSpawnCount = (int)nextSpawnAmount;
    remainingSpawnCount = totalSpawnCount;

    // Reset canSpawn for the new round
    canSpawn = false; 
    StartCoroutine(RoundStartDelay());
}

    private void StartSpawning(){
        if(canSpawn){
            foreach(GameObject spawner in activeSpawners){
                if(spawner.GetComponent<SpawnerScript>().onCooldown == false){
                    spawner.GetComponent<SpawnerScript>().Spawn(zombieBaseHP);
                }
            }
        }
    }

    private IEnumerator RoundStartDelay(){
        //Debug.Log("Waiting for round to start");
        StartCoroutine(RoundTextColorTransition());
        canSpawn = false;
        yield return new WaitForSeconds(roundDelay);
        //Debug.Log("Round Starting...");
        canSpawn = true;
        StartSpawning();
    }
    private IEnumerator RoundTextColorTransition(){
        soundSource.PlayOneShot(roundStartClip);
        yield return StartCoroutine(TransitionColor(roundText.color, midColor, colorTransitionTime));
        yield return StartCoroutine(TransitionColor(roundText.color, baseColor, colorTransitionTime));
    }
    private IEnumerator TransitionColor(Color startColor, Color endColor, float duration){
        float elapsedTime = 0f;
        float alpha = startColor.a;
        while (elapsedTime < duration)
        {
            Color newColor= Color.Lerp(startColor, endColor, elapsedTime / duration);
            newColor.a = alpha;
            roundText.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Color finalColor = endColor;
        finalColor.a = alpha;
        roundText.color = finalColor;

    }
}
