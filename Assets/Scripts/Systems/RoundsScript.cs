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
    private void Start(){
        if(roundNumber == 0){
            roundNumber = 1;
            totalSpawnCount = r1SpawnCount;
            remainingSpawnCount = totalSpawnCount;
            StartCoroutine(RoundStartDelay());
            StartSpawning();
        }
    }
    private void Update(){
        if(remainingSpawnCount == 0){
            canSpawn = false;
            CalculateNextRound();
        }
        else if(roundSpawned != totalSpawnCount && canSpawn){
            StartSpawning();
        }
        if(currentAlive >= maxZombiesAtOnce){
            canSpawn = false;
        }
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
        else{
            roundText = uiStuff.roundText;
        }
        roundText = uiStuff.roundText;
        uiStuff.UpdateRemaining(remainingSpawnCount, totalSpawnCount);

        if(roundNumber % 10 == 0 && roundNumber != triggeredForThisRound){
            uiStuff.GetComponent<UnlockTokens>().AddUpgradeToken(1);
            triggeredForThisRound = roundNumber;

        }

    }
    private void CalculateNextRound(){
        float nextSpawnAmount = ((totalSpawnCount * 2.5f)/2f);
        roundSpawned = 0;
        roundNumber += 1;
        uiStuff.UpdateRound(roundNumber);
        zombieBaseHP += zombieHpAdditive;
        totalSpawnCount = (int)nextSpawnAmount;
        remainingSpawnCount = (int)totalSpawnCount;
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
