using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemFootStepsScript : MonoBehaviour
{
    [SerializeField] AudioSource golemSource;
    [SerializeField] AudioClip footstep;

    public void PlayFootstepSound(){
        golemSource.PlayOneShot(footstep);
    }
}
