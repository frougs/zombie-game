using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSpawner : MonoBehaviour
{
    [SerializeField] GameObject golem;
    void OnDestroy(){
        Instantiate(golem, this.transform.position, Quaternion.identity);
    }
    public void StartSpawn(){
        Instantiate(golem, this.transform.position, Quaternion.identity);
    }
}
