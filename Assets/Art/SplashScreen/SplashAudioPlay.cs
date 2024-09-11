using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashAudioPlay : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource source;

    // public void Awake(){
    //     source.PlayOneShot(clip);
    // }
    public void PlayClip(){
        source.PlayOneShot(clip);
    }
}
