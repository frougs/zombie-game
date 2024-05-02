using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource speaker;
    [SerializeField] AudioClip sound;
    public void PlaySound(){
        speaker.PlayOneShot(sound);
    }
}
