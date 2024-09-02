using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralParticleEmitter : MonoBehaviour
{
    public bool toggle;

public ParticleSystem[] particles;
    private void Update(){
        if(toggle){
            ToggleParticles(toggle);
            toggle = false;
        }
    }
    public void ToggleParticles(bool toggle){
        if(particles != null){
            if(toggle){
                foreach (ParticleSystem particle in particles){
                    particle.Play();
                }
            }
            else{
                foreach (ParticleSystem particle in particles){
                    particle.Stop();
                }
            }
        }
    }
    public void TurnOnParticles(){
        ToggleParticles(true);
    }
    public void TurnOffParticles(){
        ToggleParticles(false);
    }
    public void ParticleBurst(){
        Debug.Log("Being Called");
        foreach (ParticleSystem particle in particles){
            particle.Play();
        }
    }
}