using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayerScript : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip[] backgroundTracks;
    [SerializeField] float trackDelay;
    private bool currentlySelecting = false;
    private bool easterEggPlaying;
    private Coroutine selectingSong;
    private void Update(){
        if(!musicSource.isPlaying && !currentlySelecting && !easterEggPlaying){
            selectingSong = StartCoroutine(DelayBeforeNextTrack());
        }
        if(!musicSource.isPlaying && easterEggPlaying){
            easterEggPlaying = false;
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
    public void PlayEasterEggSong(AudioClip song){
        if(selectingSong != null){
            StopCoroutine(selectingSong);
        }
        easterEggPlaying = true;
        musicSource.clip = song;
        musicSource.Play();
    }
}
