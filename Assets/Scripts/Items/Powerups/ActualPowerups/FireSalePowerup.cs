using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSalePowerup : MonoBehaviour
{
    void Update()
    {
        var boxes = FindObjectsOfType<MysteryCrate>();
        foreach (var box in boxes){
            box.fireSale = true;
        }
    }
}
