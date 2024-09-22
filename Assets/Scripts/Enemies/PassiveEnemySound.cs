using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEnemySound : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip[] passiveSounds;
    [SerializeField] float soundDelay;
    private bool currentlySelecting = false;
    private void Update(){
        if(!soundSource.isPlaying && !currentlySelecting){
            StartCoroutine(DelayBeforeNextSound());
        }
    }
    private IEnumerator DelayBeforeNextSound(){
        currentlySelecting = true;
        yield return new WaitForSeconds(soundDelay);
        StartNextSound();
    }
    private void StartNextSound(){
        if(passiveSounds.Length > 0){
            var index = Random.Range(0, passiveSounds.Length);
            soundSource.clip = passiveSounds[index];
            soundSource.Play();
            currentlySelecting = false;
        }
    }
}
