using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicLoop : MonoBehaviour
{
    [SerializeField] AudioClip loopingTheme;

    private void Update(){
        if(!this.GetComponent<AudioSource>().isPlaying){
            this.GetComponent<AudioSource>().clip = loopingTheme;
            this.GetComponent<AudioSource>().Play();
            this.GetComponent<AudioSource>().loop = true;
        }
    }
}
