using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSinglePlayer : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject player;
    public void Spawn(){
        Instantiate(player, spawnPoint.transform.position, Quaternion.identity);
    }
}
