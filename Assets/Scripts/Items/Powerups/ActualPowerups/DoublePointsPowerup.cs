using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePointsPowerup : MonoBehaviour
{
    private void Update(){
        FindObjectOfType<ScoreSystem>().doublePoints = true;
    }
}
