using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartSpawning();

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
        canSpawn = false;
        yield return new WaitForSeconds(roundDelay);
        //Debug.Log("Round Starting...");
        canSpawn = true;
    }
}
