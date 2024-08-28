using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritSpot : MonoBehaviour, IDamagable
{
    //This is temp until I make actual enemy script! :}
    [SerializeField] Dummy enemyScript;
    private void Start(){
        enemyScript = GetComponentInParent<Dummy>();
    }
     public void Damaged(float damage, GameObject shooter){
        enemyScript.Damaged(damage, shooter);

    }
}
