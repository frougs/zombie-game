using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBossArmor : MonoBehaviour
{
    public float currentArmor;
    private float maxArmor;
    private GolemBoss golem;
    [SerializeField] GameObject armorHitSpark;
    private GameObject player;
    private UIContainer uiStuff;
    private void Start(){
        golem = GetComponent<GolemBoss>();
        currentArmor = golem.maxHealth;
        maxArmor = golem.maxHealth;
        player = FindObjectOfType<ThirdPersonController>().gameObject;
        if (uiStuff == null)
        {
            uiStuff = FindObjectOfType<UIContainer>();
        }
    }
    void Update()
    {
        if(currentArmor >= golem.currentHealth){
            golem.isDamagable = false;
        }
        else{
            golem.isDamagable = true;
        }
        uiStuff.UpdateGolemArmor(currentArmor, maxArmor);

    }
    public void BossWeakSpot(float damage, GameObject attacker, Vector3 hitPoint){
        Instantiate(armorHitSpark, hitPoint, Quaternion.LookRotation((player.transform.position - hitPoint).normalized));
        currentArmor -= damage;
    }
}
