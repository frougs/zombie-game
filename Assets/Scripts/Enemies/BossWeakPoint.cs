using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPoint : MonoBehaviour, IDamagable
{
    [SerializeField] GolemBossArmor enemyScript;
    private void Start(){
        enemyScript = GetComponentInParent<GolemBossArmor>();
    }
    public void Damaged(float damage, GameObject shooter, Vector3 hitPoint){
        enemyScript.BossWeakSpot(damage, shooter, hitPoint);

    }
}
