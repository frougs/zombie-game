using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerSystem : MonoBehaviour, IInteractable
{
    public UnityEvent powerOn;
    private bool powerToggled = false;
    [SerializeField] GameObject lever;
    private AudioSource soundSource;
    [SerializeField] AudioClip powerOnSound;

    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        if(powerToggled == false){
            powerOn?.Invoke();
            lever.transform.rotation = Quaternion.Euler(-30.0f, lever.transform.rotation.y, lever.transform.rotation.z);
            powerToggled = true;
            soundSource.PlayOneShot(powerOnSound);
        }
    }
    private void Start(){
        soundSource = this.GetComponent<AudioSource>();
    }
}
