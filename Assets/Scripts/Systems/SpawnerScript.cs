using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public bool roomActive;
    public bool playerActive;
    [SerializeField] float maxPlayerDistance;
    public bool onCooldown;
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] float spawnDelay;
    //[SerializeField] float spawnCDMultiplier;
    [SerializeField] Transform player;
    private RoundsScript roundsScript;
    [SerializeField] float maxXRange;
    [SerializeField] float maxZRange;
    public int room;
    private void Start(){
        player = FindObjectOfType<ThirdPersonController>().transform;
        roundsScript = FindObjectOfType<RoundsScript>();
    }

    public void Spawn(int zombieHP){
        //Debug.Log("Trying to spawn cuh");
        //var randomInRadius = new Vector3(this.transform.position.x + Random.Range(maxXRange * -1, maxXRange), this.transform.position.y, this.transform.position.z +  Random.Range(maxZRange * -1, maxZRange));
        var zombie = Instantiate(zombiePrefab, this.transform.position, Quaternion.identity);
        zombie.GetComponent<Dummy>().maxHealth = zombieHP;
        zombie.GetComponent<Dummy>().spawnedRoom = room;
        roundsScript.roundSpawned += 1;
        roundsScript.currentAlive += 1;
        spawnDelay =  Mathf.Clamp(10 - roundsScript.roundNumber, 2, 10);
        StartCoroutine(SpawnCooldown());

    }
    private IEnumerator SpawnCooldown(){
        onCooldown = true;
        yield return new WaitForSeconds(Random.Range(3, spawnDelay));
        onCooldown = false;
    }
    private void FixedUpdate(){
        /*float distance = Vector3.Distance(player.position, this.transform.position);
        if(distance <= maxPlayerDistance){
            playerActive = true;
        }*/
        var activeSpawnerList = roundsScript.activeSpawners;
        if(playerActive && roomActive == true){
            if(!activeSpawnerList.Contains(this.gameObject)){
                activeSpawnerList.Add(this.gameObject);
            }
        }
        else{
            activeSpawnerList.Remove(this.gameObject);
        }
    }
}
