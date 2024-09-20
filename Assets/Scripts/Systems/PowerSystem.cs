using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerSystem : MonoBehaviour, IInteractable
{
    public UnityEvent powerOn;
    private bool powerToggled = false;
    [SerializeField] GameObject lever;
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip powerOnSound;

    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        if(powerToggled == false){
            powerOn?.Invoke();
            lever.transform.rotation = Quaternion.Euler(60.0f, -90f, lever.transform.rotation.z);
            powerToggled = true;
            soundSource.PlayOneShot(powerOnSound);
        }
    }
    private void Start(){
        if(soundSource == null){
            soundSource = this.GetComponentInChildren<AudioSource>();
        }
    }
}
