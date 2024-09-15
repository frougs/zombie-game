using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukePowerup : MonoBehaviour
{
    private void OnEnable(){
        var enemies = FindObjectsOfType<Dummy>();
        foreach (var enemy in enemies){
            Destroy(enemy.gameObject);
        }
    }
}
