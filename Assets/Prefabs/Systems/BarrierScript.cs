using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour, IInteractable, IDamagable
{
    public bool barrierActive;
    [SerializeField] GameObject[] planks;
    private UIContainer uiStuff;
    [SerializeField] public float barrierCapacity;
    [SerializeField] private float currentProgress;
    [SerializeField] public float progressPerInteract;
    private void Start(){
        currentProgress = barrierCapacity;
    }
    private void Update(){
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
        if(currentProgress <= 0){
            currentProgress = 0;
            foreach(GameObject plank in planks){
                plank.SetActive(false);
                barrierActive = false;
            }
        }
        else{
            barrierActive = true;
        }
        float progressRatio = currentProgress / barrierCapacity;
        progressRatio = Mathf.Clamp(progressRatio, 0f, 1f);
        for( int i=0; i < planks.Length; i++){
            float threshold = (i + 1) * 0.2f;
            planks[i].SetActive(progressRatio >= threshold);
        }
    }
     public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        currentProgress += progressPerInteract;
    }
    public void Damaged(float damage, GameObject attacker, Vector3 pos){
        if(attacker.GetComponent<Dummy>() != null){
            if(barrierActive){
                currentProgress -= damage;
            }
        }
    }
}
