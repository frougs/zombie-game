using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSoundScript : MonoBehaviour
{
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] AudioClip bloodSplatterSound;
    [SerializeField] AudioSource soundSource;

    private void Start(){
        if (deathSounds.Length > 0)
                {
                    var index = Random.Range(0, deathSounds.Length);
                    soundSource.PlayOneShot(deathSounds[index]);
                    soundSource.PlayOneShot(bloodSplatterSound);
                }
    }
}
