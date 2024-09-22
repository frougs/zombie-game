using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayerScript : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip[] backgroundTracks;
    [SerializeField] float trackDelay;
    private bool currentlySelecting = false;
    private void Update(){
        if(!musicSource.isPlaying && !currentlySelecting){
            StartCoroutine(DelayBeforeNextTrack());
        }
    }
    private IEnumerator DelayBeforeNextTrack(){
        currentlySelecting = true;
        yield return new WaitForSeconds(trackDelay);
        StartNextTrack();
    }
    private void StartNextTrack(){
        if(backgroundTracks.Length > 0){
            var index = Random.Range(0, backgroundTracks.Length);
            musicSource.clip = backgroundTracks[index];
            musicSource.Play();
            currentlySelecting = false;
        }
    }
}
