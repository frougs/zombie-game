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
    [SerializeField] AudioClip repairSound;
    [SerializeField] AudioClip breakSound;
    [SerializeField] AudioSource soundSource;
    [SerializeField] int repairPoints;
    private void Start(){
        currentProgress = barrierCapacity;
        soundSource = this.GetComponent<AudioSource>();
    }
    private void Update(){
        if(soundSource == null){
            soundSource = this.GetComponentInChildren<AudioSource>();
        }
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
        if(currentProgress < barrierCapacity){
            FindObjectOfType<ScoreSystem>().AddToScore(repairPoints);
            currentProgress += progressPerInteract;
            soundSource.PlayOneShot(repairSound);
        }
    }
    public void Damaged(float damage, GameObject attacker, Vector3 pos){
        if(attacker.GetComponent<Dummy>() != null){
            if(barrierActive){
                currentProgress -= damage;
                soundSource.PlayOneShot(breakSound);
            }
        }
    }
}
