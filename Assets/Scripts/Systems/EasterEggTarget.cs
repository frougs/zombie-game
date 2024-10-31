using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggTarget : MonoBehaviour, IDamagable
{
    private EasterEggSong easterEggScript;
    void Start()
    {
        easterEggScript = FindObjectOfType<EasterEggSong>();
    }
    public void Damaged(float damage, GameObject attacker, Vector3 hitPoint){
        easterEggScript.EasterEggProgress();
        Destroy(this.gameObject);
    }
}
